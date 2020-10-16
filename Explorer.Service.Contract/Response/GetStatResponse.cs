using Newtonsoft.Json;

namespace Explorer.Service.Contract.Response
{
    public class GetStatResponse
    {
        [JsonProperty("supply")]
        public string SupplyStr { get; set; }

        [JsonProperty("max_supply")]
        public string MaxSupplyStr { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        public decimal Supply
        {
            get
            {
                decimal.TryParse(SupplyStr.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)[0], out var amount);
                return amount;
            }
        }

        public decimal MaxSupply
        {
            get
            {
                decimal.TryParse(MaxSupplyStr.Split(' ', System.StringSplitOptions.RemoveEmptyEntries)[0], out var amount);
                return amount;
            }
        }
    }
}