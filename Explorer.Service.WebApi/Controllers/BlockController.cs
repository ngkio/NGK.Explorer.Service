using System.Threading.Tasks;
using Explorer.Service.WebApi.Models;
using Thor.Framework.Data.Model;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.Service.WebApi.Controllers
{
    public class BlockController : BaseController
    {
        private readonly BlockServiceModel _blockServiceModel;

        public BlockController(BlockServiceModel blockServiceModel)
        {
            _blockServiceModel = blockServiceModel;
        }

        [HttpGet]
        public ExcutedResult HomeStatistical()
        {
            var model = _blockServiceModel.HomeStatistical();

            return ExcutedResult.SuccessResult(model);
        }

        [HttpGet]
        public async Task<ExcutedResult> ListLatest()
        {
            var model = await _blockServiceModel.ListLatest();
            return ExcutedResult.SuccessResult(model);
        }

        [HttpGet]
        public ExcutedResult GetDetail(string blockKey)
        {
            var info = _blockServiceModel.GetDetail(blockKey);
            return ExcutedResult.SuccessResult(info);
        }

        [HttpGet]
        public async Task<ExcutedResult> ListProducers()
        {
            var model = await _blockServiceModel.ListProducers();
            return ExcutedResult.SuccessResult(model);
        }
    }
}