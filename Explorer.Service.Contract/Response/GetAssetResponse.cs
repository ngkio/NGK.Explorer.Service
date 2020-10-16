using System;
using System.Linq;
using Thor.Framework.Ext.NGKSharp.Core.Api.v1;
using Newtonsoft.Json;

namespace Explorer.Service.Contract.Response
{
    public class GetAssetResponse : GetAccountResponse
    {
        [JsonProperty("core_liquid_balance")] public string CoreLiquidBalance { get; set; }

        public DateTime CreatedShow => created.AddHours(8);

        public string Owner => permissions.FirstOrDefault(m => m.perm_name == "owner")?.required_auth.keys
            .FirstOrDefault()?.key;

        public string Active => permissions.FirstOrDefault(m => m.perm_name == "active")?.required_auth.keys
            .FirstOrDefault()?.key;
    }
}