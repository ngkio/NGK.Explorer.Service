namespace Explorer.Service.DataAccess.Entities
{
    public partial class ResourceUsage
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public string Owner { get; set; }
        public long? NetUsageLastOrdinal { get; set; }
        public decimal? NetUsageValueEx { get; set; }
        public decimal? NetUsageConsumed { get; set; }
        public long? CpuUsageLastOrdinal { get; set; }
        public decimal? CpuUsageValueEx { get; set; }
        public decimal? CpuUsageConsumed { get; set; }
        public decimal? RamUsage { get; set; }
    }
}
