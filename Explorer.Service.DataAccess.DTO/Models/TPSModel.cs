using System;

namespace Explorer.Service.DataAccess.DTO.Models
{
    [Serializable]
    public sealed class TPSModel
    {
        public double Now { get; set; }

        public double Highest { get; set; }
    }
}