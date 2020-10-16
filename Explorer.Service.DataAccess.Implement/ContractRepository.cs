using System;
using System.Collections.Generic;
using System.Linq;
using Explorer.Service.Common;
using Thor.Framework.Common.Pager;
using Thor.Framework.Data.DbContext.Relational;
using Explorer.Service.Contract.Config;
using Explorer.Service.Contract.Providers;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.DTO.Params;
using Explorer.Service.DataAccess.Entities;
using Explorer.Service.DataAccess.Entities.Enums;
using Explorer.Service.DataAccess.Interface;

namespace Explorer.Service.DataAccess.Implement
{
    public class ContractRepository : BaseRepository<ContractRow>, IContractRepository
    {
        public ContractRepository(IDbContextCore dbContext) : base(dbContext)
        {
        }

        public PagedResults<ContractInfoModel> Query(QueryContractParam param)
        {
            var sql = $"select distinct(\"name\") \"name\" from chain.account where encode(abi, 'escape') != ''";
            var configs = DbContext.ExecuteSqlQuery<Account2>(sql)
                .Select(a => new ContractConfig
                {
                    Name = a.name
                }).ToList();

            var pagerInfo = new PagerInfo(param)
            {
                TotalRowCount = configs.Count
            };
            configs = configs.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToList();

            var codes = configs.Select(m => m.Name).ToList();
            var now = DateTime.UtcNow;
            var start = now.AddHours(-24);

            var blocks = DbContext.GetDbSet<BlockInfo>()
                .Where(m => m.Timestamp >= start && m.Timestamp <= now)
                .OrderBy(m => m.BlockNum)
                .Select(m => m.BlockNum);

            var startBlock = blocks.FirstOrDefault();
            var endBlock = blocks.LastOrDefault();

            var accountMetas = DbContext.GetDbSet<AccountMetadata>()
                .Where(m => codes.Contains(m.Name))
                .ToList();

            var sqlContractStatistical = $@"SELECT
	            act_account ""Name"",
                COUNT(act_account) ""TotalCallCount"",
                COUNT(DISTINCT(receiver)) ""TotalActiveAccountCount""
            FROM
                CHAIN.action_trace
            WHERE
                act_account IN({string.Join(',', codes.Select(m => $"'{m}'"))})
                AND block_num >= {startBlock} 
	            AND block_num <= {endBlock} 
            GROUP BY
                act_account";

            var list = DbContext.ExecuteSqlQuery<ContractStatisticalModel>(sqlContractStatistical);

            var pagedData = configs.Select(m =>
                {
                    var temps = list.SingleOrDefault(t => t.Name == m.Name);
                    return new ContractInfoModel
                    {
                        Name = m.Name,
                        TotalCallCount = temps?.TotalCallCount ?? 0,
                        TotalActiveAccountCount = temps?.TotalActiveAccountCount ?? 0,
                        OneTimeCheckState = EnumOneTimeCheckState.Pass,
                        CodeAuditStatus = EnumCodeAuditStatus.HasTheAudit,
                        LastUpdateTime = accountMetas.Where(a => a.Name == m.Name)
                            .OrderByDescending(a => a.BlockNum)
                            .First()
                            .LastCodeUpdate
                    };
                })
                .ToList();

            return new PagedResults<ContractInfoModel>
            {
                PagerInfo = pagerInfo,
                Data = pagedData
            };
        }

        public TokenListModel ListToken()
        {
            var result = new TokenListModel();

            var list = GetAllTokenCodes();

            if (!list.Any()) return result;

            var codes = list.Select(m => m.Code).Distinct().ToList();
            var accounts = DbContext.GetDbSet<Account>()
                .Where(m => codes.Contains(m.Name))
                .ToList();

            var abiSerializationProvider = new AbiSerializationProvider();
            list.ForEach(item =>
            {
                var account = accounts
                    .Where(m => m.Name == item.Code && m.BlockNum <= item.BlockNum)
                    .OrderByDescending(m => m.BlockNum)
                    .First();
                var abi = abiSerializationProvider.DeserializePackedAbi(account.Abi);
                var abiTable = abi.tables.First(t => t.name == item.Table);

                var data = abiSerializationProvider.DeserializeStructData(abiTable.type, item.Value, abi);
                var model = new TokenInfoModel
                {
                    Contract = item.Code,
                    Data = data
                };
                if (!ConfigDataKey.FilterTokenSymbol.Contains(model.TokenName))
                    result.Lists.Add(model);
            });

            result.TotalTokenCount = result.Lists.Count;
            return result;
        }

        public List<ContractRow> GetAllTokenCodes()
        {
            var sql = $@"SELECT
	                            c2.block_num ""BlockNum"",
                                c2.present ""Present"",
                                c2.code ""Code"",
                                c2.""scope"" ""Scope"",
                                c2.""table"" ""Table"",
                                c2.primary_key ""PrimaryKey"",
                                c2.payer ""Payer"",
                                c2.""value"" ""Value""
                            FROM
	                            ( SELECT code, ""scope"", MAX ( block_num ) block_num FROM CHAIN.contract_row WHERE ""table"" = 'stat' GROUP BY code, ""scope"" ) c1
                                        JOIN chain.contract_row c2 ON c1.code = c2.code
                                        AND c1.""scope"" = c2.""scope""
                                        AND c1.block_num = c2.block_num";

            return DbContext.ExecuteSqlQuery<ContractRow>(sql).Where(m => m.Present).ToList();
        }

        public List<TokenOfAccountModel> GetTokensOfAccount(string accountName)
        {
            var result = new List<TokenOfAccountModel>();

            var list = DbSet.Where(m => m.Table == "accounts" && m.Scope == accountName)
                .GroupBy(m => new {m.Code, m.PrimaryKey})
                .Select(m => new
                {
                    Data = m.OrderByDescending(c => c.BlockNum).First()
                })
                .Where(p => p.Data.Present)
                .ToList();

            if (!list.Any()) return result;

            var codes = list.Select(m => m.Data.Code).Distinct().ToList();
            var accounts = DbContext.GetDbSet<Account>()
                .Where(m => codes.Contains(m.Name))
                .ToList();

            var abiSerializationProvider = new AbiSerializationProvider();
            list.ForEach(item =>
            {
                var account = accounts
                    .Where(m => m.Name == item.Data.Code && m.BlockNum <= item.Data.BlockNum)
                    .OrderByDescending(m => m.BlockNum)
                    .First();
                var abi = abiSerializationProvider.DeserializePackedAbi(account.Abi);
                var abiTable = abi.tables.First(t => t.name == item.Data.Table);

                var data = abiSerializationProvider.DeserializeStructData(abiTable.type, item.Data.Value, abi);
                var model = new TokenOfAccountModel
                {
                    Contract = item.Data.Code,
                    Data = data
                };
                if (!ConfigDataKey.FilterTokenSymbol.Contains(model.Symbol))
                    result.Add(model);
            });

            return result;
        }

        public List<TokenAccountModel> GetTokenAccounts(string code, decimal symbolCode)
        {
            var tempList = new List<TokenAccountTempModel>();

            var list = DbSet.Where(m => m.Table == "accounts" && m.Code == code && m.PrimaryKey == symbolCode)
                .GroupBy(m => m.Scope)
                .Select(m => new
                {
                    AccountName = m.Key,
                    Data = m.OrderByDescending(c => c.BlockNum).First()
                })
                .Where(p => p.Data.Present)
                .ToList();

            if (!list.Any()) return new List<TokenAccountModel>();

            var codes = list.Select(m => m.Data.Code).Distinct().ToList();
            var accounts = DbContext.GetDbSet<Account>()
                .Where(m => codes.Contains(m.Name))
                .ToList();

            var abiSerializationProvider = new AbiSerializationProvider();
            list.ForEach(item =>
            {
                var account = accounts
                    .Where(m => m.Name == item.Data.Code && m.BlockNum <= item.Data.BlockNum)
                    .OrderByDescending(m => m.BlockNum)
                    .First();
                var abi = abiSerializationProvider.DeserializePackedAbi(account.Abi);
                var abiTable = abi.tables.First(t => t.name == item.Data.Table);

                var data = abiSerializationProvider.DeserializeStructData(abiTable.type, item.Data.Value, abi);
                tempList.Add(new TokenAccountTempModel
                {
                    AccountName = item.AccountName,
                    Data = data
                });
            });

            return tempList.Select(m => new TokenAccountModel
                {
                    Balance = m.Balance,
                    AccountName = m.AccountName
                })
                .Where(m => m.Balance > 0)
                .OrderByDescending(m => m.Balance)
                .ToList();
        }
    }
}