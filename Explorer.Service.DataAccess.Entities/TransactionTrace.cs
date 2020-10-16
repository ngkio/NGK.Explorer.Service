using System;
using Explorer.Service.DataAccess.Entities.Enums;

namespace Explorer.Service.DataAccess.Entities
{
    public partial class TransactionTrace
    {
        public long BlockNum { get; set; }
        public int TransactionOrdinal { get; set; }
        public string FailedDtrxTrace { get; set; }
        public string Id { get; set; }
        public EnumTransactionStatus status { get; set; }
        public long? CpuUsageUs { get; set; }
        public long? NetUsageWords { get; set; }
        public long? Elapsed { get; set; }
        public decimal? NetUsage { get; set; }
        public bool? Scheduled { get; set; }
        public bool? AccountRamDeltaPresent { get; set; }
        public string AccountRamDeltaAccount { get; set; }
        public long? AccountRamDeltaDelta { get; set; }
        public string Except { get; set; }
        public decimal? ErrorCode { get; set; }
        public bool? PartialPresent { get; set; }
        public DateTime? PartialExpiration { get; set; }
        public int? PartialRefBlockNum { get; set; }
        public long? PartialRefBlockPrefix { get; set; }
        public long? PartialMaxNetUsageWords { get; set; }
        public short? PartialMaxCpuUsageMs { get; set; }
        public long? PartialDelaySec { get; set; }
        public string[] PartialSignatures { get; set; }
        public byte[][] PartialContextFreeData { get; set; }
    }
}