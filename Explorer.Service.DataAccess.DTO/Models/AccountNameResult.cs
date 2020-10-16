using System.Collections.Generic;
using Newtonsoft.Json;

namespace Explorer.Service.DataAccess.DTO.Models
{
    public class AccountNameResult
    {
        [JsonProperty("account_names")]
        public IList<string> AccountNames { get; set; }
    }
}