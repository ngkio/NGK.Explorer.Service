using Newtonsoft.Json;

namespace Explorer.Service.DataAccess.DTO.Models
{
    public class KeyModel
    {
        [JsonProperty("public_key")]
        public string PublicKey { get; set; }
    }
}