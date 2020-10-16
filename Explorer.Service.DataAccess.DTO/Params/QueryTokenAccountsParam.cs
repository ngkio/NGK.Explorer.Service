using Thor.Framework.Common.Pager;

namespace Explorer.Service.DataAccess.DTO.Params
{
    public sealed class QueryTokenAccountsParam : AdvQueryParam
    {
        public string Code { get; set; }
        public string TokenSymbol { get; set; }
    }
}