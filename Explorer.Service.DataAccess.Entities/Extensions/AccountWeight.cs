namespace Explorer.Service.DataAccess.Entities.Extensions
{
    public class AccountWeight
    {
        public PermissionLevel Permission { get; set; }
        public int Weight { get; set; }
    }

    public class PermissionLevel
    {
        public string Actor { get; set; }
        public string Permission { get; set; }
    }
}