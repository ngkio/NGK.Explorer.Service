using System.ComponentModel;

namespace Explorer.Service.DataAccess.Entities.Enums
{
    public enum EnumRecordType
    {
        [Description("初始化")]
        Init = 0,

        [Description("创建")]
        Create = 1,

        [Description("发行")]
        Issue = 2,

        [Description("转移")]
        Transfer = 3,

        [Description("收回")]
        Reduce = 4,

        [Description("缩减")]
        Retire = 5
    }
}