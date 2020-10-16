using System;

namespace Explorer.Service.DataAccess.Entities
{
    public partial class Account
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public string Name { get; set; }
        public DateTime? CreationDate { get; set; }
        public byte[] Abi { get; set; }
    }

    public class Account2
    {
        public string name { get; set; }
    }
}