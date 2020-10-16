using System.ComponentModel;

namespace Explorer.Service.DataAccess.Entities.Enums
{
    public enum EnumIssuerProperty
    {
        [Description("企业")]
        Enterprise = 1,

        [Description("政府")]
        Government = 2,

        [Description("事业单位")]
        Institution = 3,

        [Description("个体")]
        Personal = 4
    }
}