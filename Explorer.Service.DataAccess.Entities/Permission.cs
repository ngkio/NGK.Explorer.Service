using System;

namespace Explorer.Service.DataAccess.Entities
{
    public partial class Permission
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public string Owner { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public DateTime? LastUpdated { get; set; }

        public long? AuthThreshold { get; set; }
    }

    public class Permission2
    {
        public string block_num { get; set; }
        public string present { get; set; }
        public string owner { get; set; }
        public string name { get; set; }
        public string auth_keys { get; set; }
    }
}