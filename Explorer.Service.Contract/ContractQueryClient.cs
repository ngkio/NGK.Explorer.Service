using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Thor.Framework.Ext.NGKSharp;
using Thor.Framework.Ext.NGKSharp.Core;
using Explorer.Service.Common;
using Explorer.Service.Contract.Config;
using Explorer.Service.Contract.Response;
using Microsoft.Extensions.Options;
using Thor.Framework.Ext.NGKSharp.Core.Api.v1;

namespace Explorer.Service.Contract
{
    public class ContractQueryClient : BaseClient
    {
        public ContractQueryClient(IOptions<NgkConfig> options) : base(options)
        {
        }

        public async Task<GetProducerModel> GetProducersAsync()
        {
            var model = new GetProducerModel();
            var global = (await Client.GetTableRows<GetGlobalResponse>(new GetTableRowsRequest
            {
                json = true,
                code = "ngk",
                scope = "ngk",
                table = "global",
                limit = 1
            })).rows.FirstOrDefault();
            model.TotalVoteWeight = global?.TotalProducerVoteWeight;

            var table = await Client.GetTableRows<GetProducerResponse>(new GetTableRowsRequest
            {
                json = true,
                code = "ngk",
                scope = "ngk",
                table = "producers",
                limit = 21
            });
            model.List = table.rows.Where(m => m.IsActive == 1)
                .OrderByDescending(m => m.TotalVotes)
                .ToList();

            return model;
        }

        public async Task<List<BlockInfoOfNodeResponse>> GetBlockOfNodes(List<ProducerNodeConfig> configs)
        {
            var tasks = new List<Task>();
            var list = new ConcurrentBag<BlockInfoOfNodeResponse>();
            foreach (var config in configs)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var client = new Ngk(new ChainConfigurator
                    {
                        HttpEndpoint = config.HttpAddress,
                        ChainId = NgkConfig.ChainId,
                        ExpireSeconds = config.TimeOut
                    });
                    var info = await client.GetInfo();
                    list.Add(new BlockInfoOfNodeResponse
                    {
                        Owner = config.Owner,
                        Block = info
                    });
                }));
            }

            Task.WaitAll(tasks.ToArray());
            return list.ToList();
        }

        public async Task<bool> AccountExist(string accountName)
        {
            try
            {
                var account = await Client.GetAccount(accountName);
                return account != null;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<GetAssetResponse> GetAsset(string accountName)
        {
            try
            {
                var account = await Client.GetAccount(accountName);
                if (account == null) return null;
                var balance = await Client.GetCurrencyBalance(ConfigDataKey.DefaultTokenCode, accountName,
                    ConfigDataKey.DefaultTokenSymbol);

                var respone = new GetAssetResponse
                {
                    account_name = account.account_name,
                    CoreLiquidBalance = balance.FirstOrDefault(),
                    head_block_num = account.head_block_num,
                    head_block_time = account.head_block_time,
                    privileged = account.privileged,
                    last_code_update = account.last_code_update,
                    created = account.created,
                    cpu_limit = account.cpu_limit,
                    cpu_weight = account.cpu_weight,
                    net_limit = account.net_limit,
                    net_weight = account.net_weight,
                    ram_quota = account.ram_quota,
                    ram_usage = account.ram_usage,
                    permissions = account.permissions,
                    refund_request = account.refund_request,
                    self_delegated_bandwidth = account.self_delegated_bandwidth,
                    total_resources = account.total_resources,
                    voter_info = account.voter_info
                };

                if (string.IsNullOrWhiteSpace(respone.CoreLiquidBalance))
                {
                    respone.CoreLiquidBalance = $"0.0000 {ConfigDataKey.DefaultTokenSymbol}";
                }

                return respone;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}