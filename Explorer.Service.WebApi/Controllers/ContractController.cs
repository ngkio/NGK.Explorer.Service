using System.Threading.Tasks;
using Explorer.Service.DataAccess.DTO.Params;
using Explorer.Service.WebApi.Models;
using Thor.Framework.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.Service.WebApi.Controllers
{
    public class ContractController : BaseController
    {
        private readonly ContractServiceModel _contractServiceModel;

        public ContractController(ContractServiceModel contractServiceModel)
        {
            _contractServiceModel = contractServiceModel;
        }

        [HttpGet]
        public async Task<ExcutedResult> Query(QueryContractParam param)
        {
            var pagedResults = await _contractServiceModel.Query(param);
            return ExcutedResult.SuccessResult(pagedResults);
        }
    }
}