using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Explorer.Service.Common;
using Explorer.Service.Contract.DataModel;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.DTO.Params;
using Explorer.Service.DataAccess.Entities;
using Explorer.Service.DataAccess.Interface;
using Explorer.Service.WebApi.Common;
using Thor.Framework.Common.Pager;
using Thor.Framework.Data.Model;

namespace Explorer.Service.WebApi.Models
{
    public class TokenServiceModel : IServiceModel
    {
        private readonly IContractRepository _contractRepository;

        public TokenServiceModel(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public List<ContractRow> GetAllTokenCodes()
        {
            return _contractRepository.GetAllTokenCodes();
        }

        public async Task<ExcutedResult> GetTokenList()
        {
            var result = _contractRepository.ListToken();

            return ExcutedResult.SuccessResult(result);
        }

        public TokenInfoModel GetTokenDetails(string symbol, string contract)
        {
            var list = _contractRepository.ListToken();
            return list.Lists?.SingleOrDefault(m => m.TokenName == symbol && m.Contract == contract);
        }

        public async Task<PagedResults<TokenAccountModel>> GetTokenAccounts(QueryTokenAccountsParam param)
        {
            var list = GetTokenAccounts(param.TokenSymbol, param.Code);
            var result = new PagedResults<TokenAccountModel>
            {
                PagerInfo = new PagerInfo(param)
                {
                    TotalRowCount = list.Count
                },
                Data = list.Skip((param.PageIndex - 1) * param.PageSize).Take(param.PageSize).ToList()
            };

            return result;
        }

        public List<TokenAccountsPieModel> GetTokenAccountsPieData(string symbol, string code)
        {
            var list = GetTokenAccounts(symbol, code);
            var result = list.OrderByDescending(m => m.Balance)
                .Take(ConfigDataKey.TokenAccountsPieRank)
                .Select(m =>
                    new TokenAccountsPieModel
                    {
                        Name = m.AccountName,
                        NameEn = m.AccountName,
                        Value = m.Balance
                    })
                .ToList();

            result.Add(new TokenAccountsPieModel
            {
                Name = "其他",
                NameEn = "Other",
                Value = list.Sum(m => m.Balance) - result.Sum(m => m.Value)
            });
            return result;
        }

        private List<TokenAccountModel> GetTokenAccounts(string symbol, string code)
        {
            decimal symbolCode;
            try
            {
                symbolCode = Convert.ToDecimal(new SymbolCode(symbol).Value);
            }
            catch (Exception e)
            {
                return new List<TokenAccountModel>();
            }

            var cacheKey = GetTokenAccountsCacheKey(code, symbolCode);
            if (!RedisCacheHelper.TryGetCache(cacheKey, out List<TokenAccountModel> list)
                || list == null)
                list = GetAccountsOfToken(code, symbolCode);
            return list;
        }

        public List<TokenAccountModel> GetAccountsOfToken(string code, decimal symbolCode)
        {
            var cacheKey = GetTokenAccountsCacheKey(code, symbolCode);
            var list = _contractRepository.GetTokenAccounts(code, symbolCode);
            if (list != null && list.Any())
                RedisCacheHelper.TrySetCache(cacheKey, list, TimeSpan.FromSeconds(ConfigDataKey.TokenAccountsListCacheExpired));
            return list;
        }

        public List<TokenOfAccountModel> GetTokensOfAccount(string accountName)
        {
            return _contractRepository.GetTokensOfAccount(accountName);
        }

        private string GetTokenAccountsCacheKey(string code, decimal symbolCode)
        {
            return $"{ConfigDataKey.TokenAccountsListCacheKey}_{code}_{symbolCode}";
        }
    }
}