using System.Threading.Tasks;
using Explorer.Service.DataAccess.DTO.Models;
using Explorer.Service.DataAccess.Entities;

namespace Explorer.Service.DataAccess.Interface
{
    public interface IBlockInfoRepository
    {
        BlockInfo GetLastBlock();

        TPSModel GetTps();

        Task<BlockListModel> ListLatest(int total);

        BlockInfoDetailModel GetBlockDetail(string blockKey);

        Task<ProducersModel> ListProducers();

        bool Search(string key);
    }
}