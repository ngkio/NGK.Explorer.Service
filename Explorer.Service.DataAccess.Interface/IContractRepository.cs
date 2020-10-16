using System.Collections.Generic;
using Thor.Framework.Common.Pager;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.DTO.Params;
using Explorer.Service.DataAccess.Entities;

namespace Explorer.Service.DataAccess.Interface
{
    public interface IContractRepository
    {
        PagedResults<ContractInfoModel> Query(QueryContractParam param);

        List<ContractRow> GetAllTokenCodes();

        TokenListModel ListToken();

        List<TokenOfAccountModel> GetTokensOfAccount(string accountName);

        List<TokenAccountModel> GetTokenAccounts(string code, decimal symbolCode);
    }
}