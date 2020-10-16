namespace Explorer.Service.DataAccess.Entities
{
    public partial class PermissionLink
    {
        public long BlockNum { get; set; }
        public bool Present { get; set; }
        public string Account { get; set; }
        public string Code { get; set; }
        public string MessageType { get; set; }
        public string RequiredPermission { get; set; }
    }
}
