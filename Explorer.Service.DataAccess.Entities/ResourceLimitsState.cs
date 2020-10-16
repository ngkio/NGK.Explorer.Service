namespace Explorer.Service.DataAccess.Entities
{
    public partial class ResourceLimitsState
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public long? AverageBlockNetUsageLastOrdinal { get; set; }
        public decimal? AverageBlockNetUsageValueEx { get; set; }
        public decimal? AverageBlockNetUsageConsumed { get; set; }
        public long? AverageBlockCpuUsageLastOrdinal { get; set; }
        public decimal? AverageBlockCpuUsageValueEx { get; set; }
        public decimal? AverageBlockCpuUsageConsumed { get; set; }
        public decimal? TotalNetWeight { get; set; }
        public decimal? TotalCpuWeight { get; set; }
        public decimal? TotalRamBytes { get; set; }
        public decimal? VirtualNetLimit { get; set; }
        public decimal? VirtualCpuLimit { get; set; }
    }
}
