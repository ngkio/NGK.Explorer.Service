using Explorer.Service.Common;
using Explorer.Service.WebApi.Models;
using Thor.Framework.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.Service.WebApi.Controllers
{
    public class TransactionController : BaseController
    {
        private readonly TransactionServiceModel _transactionServiceModel;

        public TransactionController(TransactionServiceModel transactionServiceModel)
        {
            _transactionServiceModel = transactionServiceModel;
        }

        [HttpGet]
        public ExcutedResult ListLatest()
        {
            var list = _transactionServiceModel.ListLatest(ConfigDataKey.TransactionListLatestTotal);

            return ExcutedResult.SuccessResult(list);
        }

        [HttpGet]
        public ExcutedResult ListTransaction(string txId, string blockKey)
        {
            var list = _transactionServiceModel.ListTransaction(txId, blockKey);
            return ExcutedResult.SuccessResult(list);
        }

        [HttpGet]
        public ExcutedResult GetRawData(string txId)
        {
            var data = _transactionServiceModel.GetRawData(txId);
            return ExcutedResult.SuccessResult(data);
        }

        [HttpGet]
        public ExcutedResult ListOfAccount(string accountName, int page)
        {
            var list = _transactionServiceModel.ListOfAccount(accountName, page);
            return ExcutedResult.SuccessResult(list);
        }
    }
}