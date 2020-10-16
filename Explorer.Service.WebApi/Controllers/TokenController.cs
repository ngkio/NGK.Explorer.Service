using System.Threading.Tasks;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.DTO.Params;
using Explorer.Service.WebApi.Models;
using Thor.Framework.Common.Pager;
using Thor.Framework.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.Service.WebApi.Controllers
{
    public class TokenController : BaseController
    {
        private readonly TokenServiceModel _tokenServiceModel;

        public TokenController(TokenServiceModel tokenServiceModel)
        {
            _tokenServiceModel = tokenServiceModel;
        }

        public async Task<ExcutedResult> GetTokenList()
        {
            return await _tokenServiceModel.GetTokenList();
        }

        public ExcutedResult GetTokenDetails(string symbol, string contract)
        {
            var details = _tokenServiceModel.GetTokenDetails(symbol, contract);
            return ExcutedResult.SuccessResult(details);
        }

        [HttpGet]
        public async Task<PagedResults<TokenAccountModel>> GetTokenAccounts(QueryTokenAccountsParam request)
        {
            var detailResult = await _tokenServiceModel.GetTokenAccounts(request);
            return detailResult;
        }

        [HttpGet]
        public ExcutedResult GetTokenAccountsPieData(string symbol, string contract)
        {
            var list = _tokenServiceModel.GetTokenAccountsPieData(symbol, contract);
            return ExcutedResult.SuccessResult(list);
        }

        [HttpGet]
        public ExcutedResult GetTokensOfAccount(string accountName)
        {
            var list = _tokenServiceModel.GetTokensOfAccount(accountName);
            return ExcutedResult.SuccessResult(list);
        }
    }
}