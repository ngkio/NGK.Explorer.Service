using System.Linq;
using System.Threading.Tasks;
using Explorer.Service.Contract;
using Explorer.Service.Contract.Providers;
using Explorer.Service.Contract.Response;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.DTO.Params;
using Explorer.Service.DataAccess.Entities;
using Explorer.Service.WebApi.Common;
using Thor.Framework.Common.Pager;
using Thor.Framework.Data.DbContext.Relational;

namespace Explorer.Service.WebApi.Models
{
    public class AccountServiceModel : IServiceModel
    {
        private readonly IDbContextCore _dbContext;
        private readonly ContractQueryClient _contractQueryClient;

        public AccountServiceModel(IDbContextCore dbContext, ContractQueryClient contractQueryClient)
        {
            _dbContext = dbContext;
            _contractQueryClient = contractQueryClient;
        }

        public async Task<GetAssetResponse> GetAsset(string accountName)
        {
            return await _contractQueryClient.GetAsset(accountName);
        }

        public async Task<bool> AccountExist(string accountName)
        {
            return await _contractQueryClient.AccountExist(accountName);
        }

        public PagedResults<TransferRecordModel> GetTransferRecord(TransferQueryParams queryParams)
        {
            var result = new PagedResults<TransferRecordModel>
            {
                PagerInfo = new PagerInfo(queryParams)
                {
                    TotalRowCount = 0
                }
            };

            var queryable = _dbContext.GetDbSet<ActionTrace>().AsQueryable();
            queryable = queryable.Where(m => m.Receiver == queryParams.AccountName);

            if (queryParams.BlockStart.HasValue)
            {
                queryable = queryable.Where(p => p.BlockNum >= queryParams.BlockStart);
            }

            if (!string.IsNullOrEmpty(queryParams.TokenCode))
            {
                queryable = queryable.Where(p => p.ActAccount == queryParams.TokenCode);
            }

            if (queryParams.BlockEnd.HasValue)
            {
                queryable = queryable.Where((p => p.BlockNum <= queryParams.BlockEnd));
            }

            queryable = queryable.OrderByDescending(m => m.BlockNum);

            if (string.IsNullOrWhiteSpace(queryParams.TokenSymbol))
            {
                result.PagerInfo.TotalRowCount = queryable.Count();
                queryable = queryable.Skip((queryParams.PageIndex - 1) * queryParams.PageSize)
                    .Take(queryParams.PageSize);
            }

            var paged = GetTransferRecord(queryable, queryParams, result);

            return paged;
        }

        public TransactionDetailModel GetTransfer(string trxId)
        {
            var transfer = _dbContext.GetDbSet<ActionTrace>()
                .Where(m => m.TransactionId == trxId.ToUpper() && m.ActionOrdinal == 1)
                .GroupJoin(_dbContext.GetDbSet<BlockInfo>(), m => m.BlockNum, b => b.BlockNum,
                    (m, b) => new {ActionTrace = m, BlockInfo = b})
                .SelectMany(combination => combination.BlockInfo.DefaultIfEmpty(), (m, b) =>
                    new TransactionDetailModel
                    {
                        TempData = m.ActionTrace.ActData,
                        BlockNum = m.ActionTrace.BlockNum,
                        Status = m.ActionTrace.transaction_status.ToString(),
                        BlockTime = b.Timestamp,
                        ActName = m.ActionTrace.ActName,
                        ActionAccount = m.ActionTrace.ActAccount,
                        TransactionId = m.ActionTrace.TransactionId
                    })
                .FirstOrDefault();

            if (transfer == null) return transfer;

            var accounts = _dbContext.GetDbSet<Account>()
                .Where(p => p.Name == transfer.ActionAccount)
                .ToList();

            var abiSerializationProvider = new AbiSerializationProvider();
            var account = accounts.Where(m => m.Name == transfer.ActionAccount && m.BlockNum <= transfer.BlockNum)
                .OrderByDescending(m => m.BlockNum)
                .FirstOrDefault();
            var abi = abiSerializationProvider.DeserializePackedAbi(account.Abi);
            var action = abi.actions.First(t => t.name == transfer.ActName);
            var data = abiSerializationProvider.DeserializeStructData(action.type, transfer.TempData, abi);
            transfer.ActData = data;

            return transfer;
        }

        private PagedResults<TransferRecordModel> GetTransferRecord(IQueryable<ActionTrace> queryable,
            TransferQueryParams queryParams, PagedResults<TransferRecordModel> result)
        {
            var list = queryable
                .GroupJoin(_dbContext.GetDbSet<BlockInfo>(), m => m.BlockNum, b => b.BlockNum,
                    (m, b) => new {ActionTrace = m, BlockInfo = b})
                .SelectMany(combination => combination.BlockInfo.DefaultIfEmpty(), (m, b) =>
                    new TransferRecordModel
                    {
                        ActData = m.ActionTrace.ActData,
                        BlockNum = m.ActionTrace.BlockNum,
                        Status = m.ActionTrace.transaction_status.ToString(),
                        BlockTime = b.Timestamp,
                        ActName = m.ActionTrace.ActName,
                        ActionAccount = m.ActionTrace.ActAccount,
                        TransactionId = m.ActionTrace.TransactionId
                    })
                .ToList();

            var actAccounts = list.Select(m => m.ActionAccount).Distinct().ToList();
            var accounts = _dbContext.GetDbSet<Account>()
                .Where(p => actAccounts.Contains(p.Name))
                .ToList();

            var abiSerializationProvider = new AbiSerializationProvider();
            list.ForEach(item =>
            {
                var account = accounts.Where(m => m.Name == item.ActionAccount && m.BlockNum <= item.BlockNum)
                    .OrderByDescending(m => m.BlockNum)
                    .FirstOrDefault();
                var abi = abiSerializationProvider.DeserializePackedAbi(account.Abi);
                var action = abi.actions.First(t => t.name == item.ActName);
                var data = abiSerializationProvider.DeserializeStructData(action.type, item.ActData, abi);
                item.TempData = data;
            });

            if (string.IsNullOrWhiteSpace(queryParams.TokenSymbol))
            {
                result.Data = list;
            }
            else
            {
                list = list.Where(m => m.Symbol == queryParams.TokenSymbol).ToList();
                result.PagerInfo.TotalRowCount = list.Count;
                result.Data = list.Skip((queryParams.PageIndex - 1) * queryParams.PageSize).Take(queryParams.PageSize).ToList();
            }

            return result;
        }
    }
}