using Explorer.Service.DataAccess.Entities.Enums;

namespace Explorer.Service.DataAccess.Entities
{
    public partial class ActionTraceAuthSequence
    {
        public long BlockNum { get; set; }
        public string TransactionId { get; set; }
        public int ActionOrdinal { get; set; }
        public int Ordinal { get; set; }

        public EnumTransactionStatus TransactionStatus { get; set; }
        public string Account { get; set; }
        public decimal? Sequence { get; set; }
    }
}
