using System;

namespace Explorer.Service.DataAccess.DTO.Models
{
    [Serializable]
    public class HomeStatisticalModel
    {
        public string Node { get; set; }

        public long BlockNum { get; set; }

        public TPSModel Tps { get; set; }

        public long TransactionTotal { get; set; }
    }
}