using System;
using System.Linq;
using System.Threading.Tasks;
using Thor.Framework.Data.DbContext.Relational;
using Explorer.Service.Common;
using Explorer.Service.Contract;
using Explorer.Service.Contract.Config;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.Entities;
using Explorer.Service.DataAccess.Interface;
using Microsoft.Extensions.Configuration;

namespace Explorer.Service.DataAccess.Implement
{
    public class BlockInfoRepository : BaseRepository<BlockInfo>, IBlockInfoRepository
    {
        private readonly ContractQueryClient _queryClient;
        private readonly IConfiguration _configuration;

        public BlockInfoRepository(IDbContextCore dbContext, ContractQueryClient queryClient, IConfiguration configuration) :
            base(dbContext)
        {
            _queryClient = queryClient;
            _configuration = configuration;
        }

        public BlockInfo GetLastBlock()
        {
            return DbSet.OrderByDescending(m => m.BlockNum).FirstOrDefault();
        }

        public TPSModel GetTps()
        {
            const string tpsHighestKey = ConfigDataKey.TpsHighestTotal;
            TryGetCache(tpsHighestKey, out double highestTps);
            var model = new TPSModel {Highest = highestTps};
            var dateNow = DateTime.UtcNow;
            var dateStart = dateNow.AddHours(-ConfigDataKey.TpsCalculationPeriod);

            var blockNums = DbSet.Where(m => m.Timestamp >= dateStart && m.Timestamp <= dateNow)
                .Select(m => m.BlockNum)
                .Distinct()
                .ToList();

            if (!blockNums.Any()) return model;

            var transactionTotal = DbContext.GetDbSet<TransactionTrace>().Count(m => blockNums.Contains(m.BlockNum));
            var tps = transactionTotal * 1.0 / (ConfigDataKey.TpsCalculationPeriod * 3600);
            if (tps > highestTps)
            {
                highestTps = tps;
                TrySetCache(tpsHighestKey, highestTps);
            }

            model.Now = tps;
            model.Highest = highestTps;

            return model;
        }

        public async Task<BlockListModel> ListLatest(int total)
        {
            var totalTokenSupply = 0;

            var list = DbSet.OrderByDescending(m => m.BlockNum)
                .Take(total)
                .Select(m => new BlockInfoModel
                {
                    BlockNum = m.BlockNum,
                    BlockId = m.BlockId,
                    Producer = m.Producer,
                    Timestamp = m.Timestamp,
                    TransactionTotal = DbContext.GetDbSet<TransactionTrace>().Count(p => p.BlockNum == m.BlockNum),
                    ActionTotal = DbContext.GetDbSet<ActionTrace>().Count(p => p.BlockNum == m.BlockNum)
                }).ToList();

            return new BlockListModel
            {
                TotalTokenSupply = totalTokenSupply,
                List = list
            };
        }

        public BlockInfoDetailModel GetBlockDetail(string blockKey)
        {
            BlockInfo info = null;
            var transDbSet = DbContext.GetDbSet<TransactionTrace>();
            var model = new BlockInfoDetailModel();

            if (!string.IsNullOrWhiteSpace(blockKey))
            {
                if (blockKey.IsBlockNum())
                {
                    info = DbSet.SingleOrDefault(m => m.BlockNum == long.Parse(blockKey));
                }
                else
                {
                    blockKey = blockKey.ToUpper();
                    info = DbSet.SingleOrDefault(m => m.BlockId == blockKey);

                    if (info == null)
                    {
                        var blockNum = transDbSet.FirstOrDefault(m => m.Id == blockKey)?.BlockNum;
                        if (blockNum.HasValue)
                            info = DbSet.SingleOrDefault(m => m.BlockNum == blockNum.Value);
                    }
                }
            }

            if (info == null) return model;

            model.BlockNum = info.BlockNum;
            model.TransactionTotal = transDbSet.Count(m => m.BlockNum == info.BlockNum);
            model.BlockId = info.BlockId;
            model.Timestamp = info.Timestamp;
            model.Previous = info.Previous;
            model.Producer = info.Producer;

            return model;
        }

        public async Task<ProducersModel> ListProducers()
        {
            var result = new ProducersModel();

            var model = await _queryClient.GetProducersAsync();
            result.TotalVoteWeight = model.TotalVoteWeight;

            var config = _configuration.GetSection("NgkConfig").Get<NgkConfig>();

            var configs = model.List.Select(m => new ProducerNodeConfig
            {
                Owner = m.Owner,
                HttpAddress = config.ProducerNodes.FirstOrDefault()?.HttpAddress,
                TimeOut = 60
            }).ToList();
            var blocks = await _queryClient.GetBlockOfNodes(configs);

            result.List = model.List.Select(m =>
                {
                    var info = blocks.SingleOrDefault(b => b.Owner == m.Owner);
                    return new ProducerModel
                    {
                        Owner = m.Owner,
                        Area = m.Location,
                        TotalVotes = m.TotalVotes,
                        BlockNum = info?.Block.head_block_num,
                        BlockId = info?.Block.head_block_id.ToUpper()
                    };
                })
                .OrderByDescending(m => m.TotalVotes).ToList();
            return result;
        }

        public bool Search(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return false;

            return key.IsBlockNum()
                ? DbSet.Any(m => m.BlockNum == long.Parse(key))
                : DbSet.Any(m => m.BlockId == key.ToUpper());
        }
    }
}