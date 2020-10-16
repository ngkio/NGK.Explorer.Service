using System;

namespace Explorer.Service.DataAccess.Entities
{
    public partial class BlockInfo
    {
        public long BlockNum { get; set; }
        public string BlockId { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Producer { get; set; }
        public int? Confirmed { get; set; }
        public string Previous { get; set; }
        public string TransactionMroot { get; set; }
        public string ActionMroot { get; set; }
        public long? ScheduleVersion { get; set; }
        public long? NewProducersVersion { get; set; }
    }
}
