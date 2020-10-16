namespace Explorer.Service.DataAccess.Entities
{
    public partial class ResourceLimitsConfig
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public decimal? CpuLimitParametersTarget { get; set; }
        public decimal? CpuLimitParametersMax { get; set; }
        public long? CpuLimitParametersPeriods { get; set; }
        public long? CpuLimitParametersMaxMultiplier { get; set; }
        public decimal? CpuLimitParametersContractRateNumerator { get; set; }
        public decimal? CpuLimitParametersContractRateDenominator { get; set; }
        public decimal? CpuLimitParametersExpandRateNumerator { get; set; }
        public decimal? CpuLimitParametersExpandRateDenominator { get; set; }
        public decimal? NetLimitParametersTarget { get; set; }
        public decimal? NetLimitParametersMax { get; set; }
        public long? NetLimitParametersPeriods { get; set; }
        public long? NetLimitParametersMaxMultiplier { get; set; }
        public decimal? NetLimitParametersContractRateNumerator { get; set; }
        public decimal? NetLimitParametersContractRateDenominator { get; set; }
        public decimal? NetLimitParametersExpandRateNumerator { get; set; }
        public decimal? NetLimitParametersExpandRateDenominator { get; set; }
        public long? AccountCpuUsageAverageWindow { get; set; }
        public long? AccountNetUsageAverageWindow { get; set; }
    }
}
