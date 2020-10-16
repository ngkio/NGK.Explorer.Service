using System;
using System.Collections.Generic;
using Thor.Framework.Common.Helper.Extensions;

namespace Explorer.Service.DataAccess.DTO.Models
{
    [Serializable]
    public class BlockListModel
    {
        public long TotalTokenSupply { get; set; }
        public List<BlockInfoModel> List { get; set; }
    }

    [Serializable]
    public class BlockInfoModel
    {
        public long BlockNum { get; set; }

        private string _blockId;

        public string BlockId
        {
            get => _blockId?.ToLower();
            set => _blockId = value;
        }

        public DateTime? Timestamp { get; set; }
        public long? TimestampShow => Timestamp?.AddHours(8).ToTimestamp();
        public string Producer { get; set; }
        public long TransactionTotal { get; set; }
        public long ActionTotal { get; set; }
    }

    public class BlockInfoDetailModel : BlockInfoModel
    {
        private string _previous;

        public string Previous
        {
            get => _previous?.ToLower();
            set => _previous = value;
        }
    }
}