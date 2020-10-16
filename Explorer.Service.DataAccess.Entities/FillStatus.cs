namespace Explorer.Service.DataAccess.Entities
{
    public partial class FillStatus
    {
        public long Head { get; set; }

        public string HeadId { get; set; }

        public long Irreversible { get; set; }

        public string IrreversibleId { get; set; }

        public long First { get; set; }
    }
}