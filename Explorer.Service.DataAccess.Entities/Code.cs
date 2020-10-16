namespace Explorer.Service.DataAccess.Entities
{
    public partial class Code
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public short VmType { get; set; }
        public short VmVersion { get; set; }
        public string CodeHash { get; set; }
        public byte[] Code1 { get; set; }
    }
}
