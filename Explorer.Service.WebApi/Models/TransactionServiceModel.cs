using System.Collections.Generic;
using Explorer.Service.Common;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.Interface;
using Explorer.Service.WebApi.Common;

namespace Explorer.Service.WebApi.Models
{
    public sealed class TransactionServiceModel : IServiceModel
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionServiceModel(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public List<TransactionRecordModel> ListLatest(int total)
        {
            if (!RedisCacheHelper.TryGetCache(ConfigDataKey.LatestTransactionListCacheKey,
                    out List<TransactionRecordModel> list)
                || list == null)
                list = GetListLatest(total);
            return list;
        }

        public List<TransactionRecordModel> GetListLatest(int total)
        {
            var list = _transactionRepository.ListLatest(total);
            RedisCacheHelper.TrySetCache(ConfigDataKey.LatestTransactionListCacheKey, list);
            return list;
        }

        public List<TransactionModel> ListTransaction(string txId, string blockKey)
        {
            return _transactionRepository.ListTransaction(txId, blockKey);
        }

        public TransactionRawDataModel GetRawData(string txId)
        {
            return _transactionRepository.GetRawData(txId);
        }

        public bool Search(string key)
        {
            return _transactionRepository.Search(key);
        }

        public List<TransactionOfAccountModel> ListOfAccount(string accountName, int page)
        {
            return _transactionRepository.ListOfAccount(accountName, page);
        }
    }
}