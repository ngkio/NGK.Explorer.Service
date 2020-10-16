using System;
using System.Threading.Tasks;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.Entities.Enums;
using Explorer.Service.DataAccess.Interface;
using Explorer.Service.WebApi.Models;
using Thor.Framework.Data;
using Thor.Framework.Data.Model;

namespace Explorer.Service.WebApi.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IBlockInfoRepository _blockInfoRepository;
        private readonly TransactionServiceModel _transactionServiceModel;
        private readonly AccountServiceModel _accountServiceModel;

        public SearchController(IBlockInfoRepository blockInfoRepository,
            TransactionServiceModel transactionServiceModel, AccountServiceModel accountServiceModel)
        {
            _blockInfoRepository = blockInfoRepository;
            _transactionServiceModel = transactionServiceModel;
            _accountServiceModel = accountServiceModel;
        }

        public async Task<ExcutedResult> Query(string key)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    if (await _accountServiceModel.AccountExist(key))
                    {
                        return ExcutedResult.SuccessResult(new SearchResultModel
                        {
                            ResultType = (int) EnumSearchType.Account,
                            SearchKey = key
                        });
                    }

                    if (_blockInfoRepository.Search(key))
                    {
                        return ExcutedResult.SuccessResult(new SearchResultModel
                        {
                            ResultType = (int) EnumSearchType.Block,
                            SearchKey = key
                        });
                    }

                    if (_transactionServiceModel.Search(key))
                    {
                        return ExcutedResult.SuccessResult(new SearchResultModel
                        {
                            ResultType = (int) EnumSearchType.Transaction,
                            SearchKey = key
                        });
                    }
                }
            }
            catch (Exception e)
            {
            }

            return ExcutedResult.FailedResult(SysResultCode.DataNotExist, string.Empty);
        }
    }
}