using Thor.Framework.Ext.NGKSharp.Core.Api.v1;

namespace Explorer.Service.Contract.Response
{
    public class BlockInfoOfNodeResponse
    {
        public string Owner { get; set; }
        public GetInfoResponse Block { get; set; }
    }
}