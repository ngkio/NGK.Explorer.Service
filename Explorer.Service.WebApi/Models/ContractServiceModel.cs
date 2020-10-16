using System.Threading.Tasks;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.DTO.Params;
using Explorer.Service.DataAccess.Interface;
using Explorer.Service.WebApi.Common;
using Thor.Framework.Common.Pager;

namespace Explorer.Service.WebApi.Models
{
    public class ContractServiceModel : IServiceModel
    {
        private readonly IContractRepository _contractRepository;

        public ContractServiceModel(IContractRepository contractRepository)
        {
            _contractRepository = contractRepository;
        }

        public async Task<PagedResults<ContractInfoModel>> Query(QueryContractParam param)
        {
            return _contractRepository.Query(param);
        }
    }
}