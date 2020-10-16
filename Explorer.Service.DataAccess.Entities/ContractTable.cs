namespace Explorer.Service.DataAccess.Entities
{
    public partial class ContractTable
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public string Code { get; set; }
        public string Scope { get; set; }
        public string Table { get; set; }
        public string Payer { get; set; }
    }
}
