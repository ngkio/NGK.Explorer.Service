namespace Explorer.Service.DataAccess.Entities
{
    public partial class ResourceLimits
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public string Owner { get; set; }
        public long? NetWeight { get; set; }
        public long? CpuWeight { get; set; }
        public long? RamBytes { get; set; }
    }
}
