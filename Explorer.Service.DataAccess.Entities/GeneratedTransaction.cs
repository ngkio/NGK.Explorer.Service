namespace Explorer.Service.DataAccess.Entities
{
    public partial class GeneratedTransaction
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public string Sender { get; set; }
        public decimal SenderId { get; set; }
        public string Payer { get; set; }
        public string TrxId { get; set; }
        public byte[] PackedTrx { get; set; }
    }
}
