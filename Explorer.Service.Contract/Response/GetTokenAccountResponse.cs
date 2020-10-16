using Newtonsoft.Json;

namespace Explorer.Service.Contract.Response
{
    public class GetTokenAccountResponse
    {
        [JsonProperty("balance")]
        public string Balance { get; set; }
        
        [JsonProperty("account_name")]
        public string AccountName { get; set; }

        [JsonProperty("account_id")]
        public long AccountId { get; set; }

        [JsonProperty("last_modify_time")]
        public long LastModifyTime { get; set; }
    }
}