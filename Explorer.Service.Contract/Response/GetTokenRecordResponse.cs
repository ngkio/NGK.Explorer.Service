using Newtonsoft.Json;

namespace Explorer.Service.Contract.Response
{
    public class GetTokenRecordResponse
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        
        [JsonProperty("trx_id")]
        public string TrxId { get; set; }
        
        [JsonProperty("type")]
        public int Type { get; set; }

        [JsonProperty("from_account_id")]
        public long FromAccountId { get; set; }
        
        [JsonProperty("from_account_name")]
        public string FromAccountName { get; set; }

        [JsonProperty("to_account_id")]
        public long ToAccountId { get; set; }
        
        [JsonProperty("to_account_name")]
        public string ToAccountName { get; set; }
        
        [JsonProperty("integral")]
        public string Integral { get; set; }
        
        [JsonProperty("memo")]
        public string Memo { get; set; }
        
        [JsonProperty("time")]
        public long Time { get; set; }
    }
}