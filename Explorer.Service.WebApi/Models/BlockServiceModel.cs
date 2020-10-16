using System.Threading.Tasks;
using Explorer.Service.Common;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.Interface;
using Explorer.Service.WebApi.Common;

namespace Explorer.Service.WebApi.Models
{
    public class BlockServiceModel : IServiceModel
    {
        private readonly IBlockInfoRepository _blockInfoRepository;
        private readonly ITransactionRepository _transactionRepository;

        public BlockServiceModel(IBlockInfoRepository blockInfoRepository,
            ITransactionRepository transactionRepository)
        {
            _blockInfoRepository = blockInfoRepository;
            _transactionRepository = transactionRepository;
        }

        public HomeStatisticalModel HomeStatistical()
        {
            if (!RedisCacheHelper.TryGetCache(ConfigDataKey.LatestBlockCacheKey, out HomeStatisticalModel model) ||
                model == null)
                model = GetLatestStatistical();
            return model;
        }

        public HomeStatisticalModel GetLatestStatistical()
        {
            var model = new HomeStatisticalModel();

            model.Tps = _blockInfoRepository.GetTps();

            var total = _transactionRepository.GetTotal();
            model.TransactionTotal = total;

            var lastBlock = _blockInfoRepository.GetLastBlock();
            model.BlockNum = lastBlock.BlockNum;
            model.Node = lastBlock.Producer;

            RedisCacheHelper.TrySetCache(ConfigDataKey.LatestBlockCacheKey, model);

            return model;
        }

        public async Task<BlockListModel> ListLatest()
        {
            if (!RedisCacheHelper.TryGetCache(ConfigDataKey.LatestBlockListCacheKey, out BlockListModel model)
                || model == null)
                model = await GetListLatest();
            return model;
        }

        public async Task<BlockListModel> GetListLatest()
        {
            var model = await _blockInfoRepository.ListLatest(ConfigDataKey.BlockInfoListLatestTotal);
            RedisCacheHelper.TrySetCache(ConfigDataKey.LatestBlockListCacheKey, model);
            return model;
        }

        public BlockInfoDetailModel GetDetail(string blockKey)
        {
            var info = _blockInfoRepository.GetBlockDetail(blockKey);
            return info;
        }

        public async Task<ProducersModel> ListProducers()
        {
            if (!RedisCacheHelper.TryGetCache(ConfigDataKey.LatestProducerListCacheKey, out ProducersModel model)
                || model == null)
                model = await GetListProducers();
            return model;
        }

        public async Task<ProducersModel> GetListProducers()
        {
            var model = await _blockInfoRepository.ListProducers();
            RedisCacheHelper.TrySetCache(ConfigDataKey.LatestProducerListCacheKey, model);
            return model;
        }
    }
}