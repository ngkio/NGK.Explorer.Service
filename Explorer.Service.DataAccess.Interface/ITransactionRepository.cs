using System.Collections.Generic;
using Explorer.Service.DataAccess.DTO.Models;

namespace Explorer.Service.DataAccess.Interface
{
    public interface ITransactionRepository
    {
        long GetTotal();

        List<TransactionRecordModel> ListLatest(int total);

        List<TransactionModel> ListTransaction(string txId, string blockKey);

        TransactionRawDataModel GetRawData(string txId);

        bool Search(string key);

        List<TransactionOfAccountModel> ListOfAccount(string accountName, int page);
    }
}