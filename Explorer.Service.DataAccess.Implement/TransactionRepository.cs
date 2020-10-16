using System.Collections.Generic;
using System.Linq;
using Thor.Framework.Data.DbContext.Relational;
using Explorer.Service.Common;
using Explorer.Service.Contract.Providers;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.Entities;
using Explorer.Service.DataAccess.Interface;
using ActionTrace = Explorer.Service.DataAccess.Entities.ActionTrace;

namespace Explorer.Service.DataAccess.Implement
{
    public class TransactionRepository : BaseRepository<TransactionTrace>, ITransactionRepository
    {
        public TransactionRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }

        public long GetTotal()
        {
            return DbSet.Count();
        }

        public List<TransactionRecordModel> ListLatest(int total)
        {
            var list = DbContext.GetDbSet<ActionTrace>()
                .OrderByDescending(m => m.BlockNum)
                .Take(total)
                .GroupJoin(DbContext.GetDbSet<BlockInfo>(), m => m.BlockNum, b => b.BlockNum,
                    (m, b) => new {ActionTrace = m, BlockInfo = b})
                .SelectMany(combination => combination.BlockInfo.DefaultIfEmpty(), (m, b) => new TransactionRecordModel
                {
                    BlockNum = m.ActionTrace.BlockNum,
                    TransactionId = m.ActionTrace.TransactionId,
                    Timestamp = b.Timestamp,
                    Type = m.ActionTrace.ActName,
                    ActAccount = m.ActionTrace.ActAccount,
                    ActName = m.ActionTrace.ActName,
                    TempData = m.ActionTrace.ActData,
                })
                .ToList();

            var actAccounts = list.Select(m => m.ActAccount).Distinct().ToList();

            var accounts = DbContext.GetDbSet<Account>()
                .Where(p => actAccounts.Contains(p.Name))
                .ToList();

            var abiSerializationProvider = new AbiSerializationProvider();
            list.ForEach(item =>
            {
                var account = accounts.Where(m => m.Name == item.ActAccount && m.BlockNum <= item.BlockNum)
                    .OrderByDescending(m => m.BlockNum)
                    .FirstOrDefault();
                var abi = abiSerializationProvider.DeserializePackedAbi(account.Abi);
                var action = abi.actions.First(t => t.name == item.ActName);
                var data = abiSerializationProvider.DeserializeStructData(action.type, item.TempData, abi);
                item.ActData = data;
            });

            return list;
        }

        public List<TransactionModel> ListTransaction(string txId, string blockKey)
        {
            var result = new List<TransactionModel>();
            if (!string.IsNullOrWhiteSpace(txId) || !string.IsNullOrWhiteSpace(blockKey))
            {
                if (!string.IsNullOrWhiteSpace(txId))
                {
                    txId = txId.ToUpper();
                    var list = DbContext.GetDbSet<ActionTrace>()
                        .Where(m => m.TransactionId == txId)
                        .OrderBy(m => m.ActionOrdinal)
                        .GroupJoin(DbContext.GetDbSet<BlockInfo>(), m => m.BlockNum, b => b.BlockNum,
                            (m, b) => new {ActionTrace = m, BlockInfo = b})
                        .SelectMany(combination => combination.BlockInfo.DefaultIfEmpty(), (m, b) =>
                            new TransactionModel
                            {
                                BlockNum = m.ActionTrace.BlockNum,
                                TransactionId = m.ActionTrace.TransactionId,
                                ActionOrdinal = m.ActionTrace.ActionOrdinal,
                                CreatorActionOrdinal = m.ActionTrace.CreatorActionOrdinal,
                                Timestamp = b.Timestamp,
                                Type = m.ActionTrace.ActName,
                                ActAccount = m.ActionTrace.ActAccount,
                                ActName = m.ActionTrace.ActName,
                                TempData = m.ActionTrace.ActData,
                            })
                        .ToList();

                    if (!list.Any()) return result;

                    var actAccounts = list.Select(m => m.ActAccount).Distinct().ToList();
                    var accounts = DbContext.GetDbSet<Account>()
                        .Where(p => actAccounts.Contains(p.Name))
                        .ToList();

                    var abiSerializationProvider = new AbiSerializationProvider();
                    list.ForEach(item =>
                    {
                        var account = accounts.Where(m =>
                                m.Name == item.ActAccount && m.BlockNum <= item.BlockNum)
                            .OrderByDescending(m => m.BlockNum)
                            .First();
                        var abi = abiSerializationProvider.DeserializePackedAbi(account.Abi);
                        var action = abi.actions.First(t => t.name == item.ActName);
                        var data = abiSerializationProvider.DeserializeStructData(action.type, item.TempData,
                            abi);
                        item.ActData = data;
                        if (result.All(m => m.GetHashCode() != item.GetHashCode()))
                            result.Add(item);
                    });
                }
                else
                {
                    BlockInfo blockInfo;
                    var blcokDbSet = DbContext.GetDbSet<BlockInfo>();
                    if (blockKey.IsBlockNum())
                    {
                        blockInfo = blcokDbSet.SingleOrDefault(m => m.BlockNum == long.Parse(blockKey));
                    }
                    else
                    {
                        blockKey = blockKey.ToUpper();
                        blockInfo = blcokDbSet.SingleOrDefault(m => m.BlockId == blockKey);
                    }

                    if (blockInfo == null) return result;

                    var list = DbContext.GetDbSet<ActionTrace>()
                        .Where(m => m.BlockNum == blockInfo.BlockNum && m.CreatorActionOrdinal == 0)
                        .ToList();

                    if (!list.Any()) return result;

                    var actAccounts = list.Select(m => m.ActAccount).Distinct().ToList();
                    var accounts = DbContext.GetDbSet<Account>()
                        .Where(p => actAccounts.Contains(p.Name))
                        .ToList();

                    var abiSerializationProvider = new AbiSerializationProvider();
                    list.ForEach(item =>
                    {
                        var account = accounts.Where(m =>
                                m.Name == item.ActAccount && m.BlockNum <= item.BlockNum)
                            .OrderByDescending(m => m.BlockNum)
                            .First();
                        var abi = abiSerializationProvider.DeserializePackedAbi(account.Abi);
                        var action = abi.actions.First(t => t.name == item.ActName);
                        var data = abiSerializationProvider.DeserializeStructData(action.type, item.ActData,
                            abi);
                        result.Add(new TransactionModel
                        {
                            BlockNum = item.BlockNum,
                            TransactionId = item.TransactionId,
                            Timestamp = blockInfo.Timestamp,
                            Type = item.ActName,
                            ActAccount = item.ActAccount,
                            ActName = item.ActName,
                            ActData = data,
                        });
                    });
                }
            }

            return result;
        }

        public TransactionRawDataModel GetRawData(string txId)
        {
            txId = txId?.ToUpper();
            var transaction = DbSet.Where(m => m.Id == txId).Select(m => new TransactionRawDataModel
                {
                    Id = m.Id,
                    BlockNum = m.BlockNum,
                    TransactionOrdinal = m.TransactionOrdinal,
                    FailedDtrxTrace = m.FailedDtrxTrace,
                    status = m.status.ToString(),
                    CpuUsageUs = m.CpuUsageUs,
                    NetUsageWords = m.NetUsageWords,
                    Elapsed = m.Elapsed,
                    NetUsage = m.NetUsage,
                    Scheduled = m.Scheduled,
                    AccountRamDeltaAccount = m.AccountRamDeltaAccount,
                    AccountRamDeltaDelta = m.AccountRamDeltaDelta,
                    AccountRamDeltaPresent = m.AccountRamDeltaPresent,
                    ErrorCode = m.ErrorCode,
                    PartialExpiration = m.PartialExpiration,
                    PartialPresent = m.PartialPresent,
                    PartialSignatures = m.PartialSignatures,
                    PartialDelaySec = m.PartialDelaySec,
                    PartialContextFreeData = m.PartialContextFreeData,
                    PartialRefBlockNum = m.PartialRefBlockNum,
                    PartialRefBlockPrefix = m.PartialRefBlockPrefix,
                    PartialMaxCpuUsageMs = m.PartialMaxCpuUsageMs,
                    PartialMaxNetUsageWords = m.PartialMaxNetUsageWords,
                    Except = m.Except,
                    Actions = DbContext.GetDbSet<ActionTrace>().Where(a => a.TransactionId == txId)
                        .OrderBy(a => a.ActionOrdinal).Select(a => new ActionTraceRawModel
                        {
                            BlockNum = a.BlockNum,
                            TransactionId = a.TransactionId,
                            TransactionStatus = a.transaction_status.ToString(),
                            ActionOrdinal = a.ActionOrdinal,
                            CreatorActionOrdinal = a.CreatorActionOrdinal,
                            ReceiptPresent = a.ReceiptPresent,
                            ReceiptReceiver = a.ReceiptReceiver,
                            ReceiptActDigest = a.ReceiptActDigest,
                            ReceiptGlobalSequence = a.ReceiptGlobalSequence,
                            ReceiptRecvSequence = a.ReceiptRecvSequence,
                            ReceiptCodeSequence = a.ReceiptCodeSequence,
                            ReceiptAbiSequence = a.ReceiptAbiSequence,
                            Receiver = a.Receiver,
                            ActAccount = a.ActAccount,
                            ActName = a.ActName,
                            ActData = a.ActData,
                            ContextFree = a.ContextFree,
                            Elapsed = a.Elapsed,
                            Console = a.Console,
                            Except = a.Except,
                            ErrorCode = a.ErrorCode
                        }).ToList()
                })
                .SingleOrDefault();

            if (transaction != null)
            {
                var actAccounts = transaction.Actions.Select(m => m.ActAccount).Distinct().ToList();
                var accounts = DbContext.GetDbSet<Account>()
                    .Where(p => actAccounts.Contains(p.Name))
                    .ToList();

                var abiSerializationProvider = new AbiSerializationProvider();

                transaction.Actions.ForEach(item =>
                {
                    var account = accounts.Where(m => m.Name == item.ActAccount && m.BlockNum <= item.BlockNum)
                        .OrderByDescending(m => m.BlockNum)
                        .First();
                    var abi = abiSerializationProvider.DeserializePackedAbi(account.Abi);
                    var action = abi.actions.First(t => t.name == item.ActName);
                    var data = abiSerializationProvider.DeserializeStructData(action.type, item.ActData, abi);
                    item.ActTempData = data;
                });
            }

            return transaction;
        }

        public bool Search(string key)
        {
            return DbSet.Any(m => m.Id == key.ToUpper());
        }

        public List<TransactionOfAccountModel> ListOfAccount(string accountName, int page)
        {
            var pageSize = 10;
            var list = DbContext.GetDbSet<ActionTrace>()
                .Where(m => m.Receiver == accountName)
                .OrderByDescending(m => m.BlockNum)
                .ThenBy(m => m.TransactionId)
                .ThenBy(m => m.ActionOrdinal)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .GroupJoin(DbContext.GetDbSet<BlockInfo>(), m => m.BlockNum, b => b.BlockNum,
                    (m, b) => new {ActionTrace = m, BlockInfo = b})
                .SelectMany(combination => combination.BlockInfo.DefaultIfEmpty(), (m, b) =>
                    new TransactionOfAccountModel
                    {
                        BlockNum = m.ActionTrace.BlockNum,
                        TransactionId = m.ActionTrace.TransactionId,
                        Timestamp = b.Timestamp,
                        Type = m.ActionTrace.ActName,
                        ActAccount = m.ActionTrace.ActAccount,
                        ActName = m.ActionTrace.ActName,
                        TempData = m.ActionTrace.ActData,
                    })
                .ToList();

            var actAccounts = list.Select(m => m.ActAccount).Distinct().ToList();
            var accounts = DbContext.GetDbSet<Account>()
                .Where(p => actAccounts.Contains(p.Name))
                .ToList();

            var abiSerializationProvider = new AbiSerializationProvider();
            list.ForEach(item =>
            {
                var account = accounts.Where(m => m.Name == item.ActAccount && m.BlockNum <= item.BlockNum)
                    .OrderByDescending(m => m.BlockNum)
                    .FirstOrDefault();
                var abi = abiSerializationProvider.DeserializePackedAbi(account.Abi);
                var action = abi.actions.First(t => t.name == item.ActName);
                var data = abiSerializationProvider.DeserializeStructData(action.type, item.TempData, abi);
                item.ActData = data;
            });

            return list;
        }
    }
}