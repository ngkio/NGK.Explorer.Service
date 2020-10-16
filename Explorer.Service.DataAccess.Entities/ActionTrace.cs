using Explorer.Service.DataAccess.Entities.Enums;

namespace Explorer.Service.DataAccess.Entities
{
    public partial class ActionTrace
    {
        public long BlockNum { get; set; }
        public string TransactionId { get; set; }
        public EnumTransactionStatus transaction_status { get; set; }
        public long ActionOrdinal { get; set; }
        public long? CreatorActionOrdinal { get; set; }
        public bool? ReceiptPresent { get; set; }
        public string ReceiptReceiver { get; set; }
        public string ReceiptActDigest { get; set; }
        public decimal? ReceiptGlobalSequence { get; set; }
        public decimal? ReceiptRecvSequence { get; set; }
        public long? ReceiptCodeSequence { get; set; }
        public long? ReceiptAbiSequence { get; set; }
        public string Receiver { get; set; }
        public string ActAccount { get; set; }
        public string ActName { get; set; }
        public byte[] ActData { get; set; }
        public bool? ContextFree { get; set; }
        public long? Elapsed { get; set; }
        public string Console { get; set; }
        public string Except { get; set; }
        public decimal? ErrorCode { get; set; }
    }
}
