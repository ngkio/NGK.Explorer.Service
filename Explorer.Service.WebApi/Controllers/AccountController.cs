using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.DTO.Params;
using Explorer.Service.DataAccess.Entities;
using Explorer.Service.WebApi.Models;
using Thor.Framework.Common.Pager;
using Thor.Framework.Data;
using Thor.Framework.Data.DbContext.Relational;
using Thor.Framework.Data.Model;
using Thor.Framework.Ext.NGKSharp.Core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.Service.WebApi.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IDbContextCore _dbContext;
        private readonly AccountServiceModel _accountServiceModel;

        public AccountController(IDbContextCore dbContext, AccountServiceModel accountServiceModel)
        {
            _dbContext = dbContext;
            _accountServiceModel = accountServiceModel;
        }

        [HttpGet]
        public async Task<ExcutedResult> GetAsset(string accountName)
        {
            var account = await _accountServiceModel.GetAsset(accountName);
            return ExcutedResult.SuccessResult(account);
        }

        [HttpGet]
        public ExcutedResult<PagedResults<TransferRecordModel>> GetTransferRecord(TransferQueryParams queryParams)
        {
            if (!ModelState.IsValid)
                return ExcutedResult<PagedResults<TransferRecordModel>>.FailedResult(SysResultCode.ParameterInvalid,
                    "参数无效或错误");

            var result = _accountServiceModel.GetTransferRecord(queryParams);
            return ExcutedResult<PagedResults<TransferRecordModel>>.SuccessResult(result);
        }

        [HttpGet]
        public ExcutedResult<TransactionDetailModel> GetTransfer(string trxId)
        {
            if (string.IsNullOrWhiteSpace(trxId))
                return ExcutedResult<TransactionDetailModel>.FailedResult(SysResultCode.ParameterInvalid, "参数无效或错误");

            var transfer = _accountServiceModel.GetTransfer(trxId);
            return ExcutedResult<TransactionDetailModel>.SuccessResult(transfer);
        }

        [HttpPost]
        public AccountNameResult GetAccountByPubKey([FromBody] KeyModel keyModel)
        {
            var result = GetPermissionByPubKey(keyModel.PublicKey);
            return new AccountNameResult
            {
                AccountNames = result.Status == EnumStatus.Success
                    ? result.Data.Select(p => p.owner).Distinct().OrderBy(p => p).ToList()
                    : new List<string>()
            };
        }

        private ExcutedResult<IList<Permission2>> GetPermissionByPubKey(string pubKey)
        {
            var newKey = CryptoHelper.ConvertLegacyPubKey(pubKey);

            var query = _dbContext.ExecuteSqlQuery<Permission2>(
                $"select j.a->>'block_num' as block_num, j.a->>'present' as present, j.a->>'name' as name,j.a->>'owner' as owner, j.a->>'auth_keys' as auth_keys FROM (select to_jsonb(x) as a from \"chain\".\"permission\" as x) as j where j.a @> '{{\"auth_keys\":[{{\"key\":\"{newKey}\"}}]}}'");

            var result = query.GroupBy(k => new {k.name, k.owner},
                    (k, r) => r.OrderByDescending(p => p.block_num).FirstOrDefault())
                .Where(p => bool.Parse(p.present))
                .ToList();
            return ExcutedResult<IList<Permission2>>.SuccessResult(result);
        }
    }
}