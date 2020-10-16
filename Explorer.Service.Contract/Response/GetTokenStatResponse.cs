using Newtonsoft.Json;

namespace Explorer.Service.Contract.Response
{
    public class GetTokenStatResponse
    {
        [JsonProperty("supply")]
        public string Supply { get; set; }
        
        [JsonProperty("max_supply")]
        public string MaxSupply { get; set; }

        [JsonProperty("issuer_id")]
        public long IssuerId { get; set; }

        [JsonProperty("issuer_name")]
        public string IssuerName { get; set; }

        [JsonProperty("issuer_property")]
        public int IssuerProperty { get; set; }

        [JsonProperty("token_type")]
        public int TokenType { get; set; }

        [JsonProperty("business_value")]
        public string BusinessValue { get; set; }
    }
}