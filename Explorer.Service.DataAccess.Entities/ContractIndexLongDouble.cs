﻿namespace Explorer.Service.DataAccess.Entities
{
    public partial class ContractIndexLongDouble
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public string Code { get; set; }
        public string Scope { get; set; }
        public string Table { get; set; }
        public decimal PrimaryKey { get; set; }
        public string Payer { get; set; }
        public byte[] SecondaryKey { get; set; }
    }
}
