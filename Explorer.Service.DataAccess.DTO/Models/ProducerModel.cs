using System;
using System.Collections.Generic;

namespace Explorer.Service.DataAccess.DTO.Models
{
    [Serializable]
    public sealed class ProducersModel
    {
        public string TotalVoteWeight { get; set; }

        public List<ProducerModel> List { get; set; }
    }

    [Serializable]
    public sealed class ProducerModel
    {
        public string Owner { get; set; }

        public string Area { get; set; }

        public decimal TotalVotes { get; set; }

        public long? BlockNum { get; set; }

        private string _blockId;

        public string BlockId
        {
            get => _blockId?.ToLower();
            set => _blockId = value;
        }
    }
}