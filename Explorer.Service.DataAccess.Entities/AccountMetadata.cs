using System;

namespace Explorer.Service.DataAccess.Entities
{
    public partial class AccountMetadata
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public string Name { get; set; }
        public bool? Privileged { get; set; }
        public DateTime? LastCodeUpdate { get; set; }
        public bool? CodePresent { get; set; }
        public short? CodeVmType { get; set; }
        public short? CodeVmVersion { get; set; }
        public string CodeCodeHash { get; set; }
    }
}
