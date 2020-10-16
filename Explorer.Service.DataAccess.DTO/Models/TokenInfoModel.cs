using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Explorer.Service.DataAccess.DTO.Models
{
    public class TokenInfoModel
    {
        public string Contract { get; set; }
        public ulong TokenId { get; set; }
        public string TokenName => SupplyStr.Split(" ")[1];
        public int TokenPrecision => SupplyStr.Split(" ")[0].Split(".")[1].Length;
        public string IssuerName => Data.GetValueOrDefault("issuer")?.ToString();
        public string IssuerPropertyShow { get; set; }
        public int IssuerProperty => Convert.ToInt32(Data.GetValueOrDefault("issuer_property"));
        public string TokenTypeShow { get; set; }
        public int TokenType => Convert.ToInt32(Data.GetValueOrDefault("token_type"));
        public decimal Supply => decimal.Parse(SupplyStr.Split(" ")[0]);
        public decimal MaxSupply => decimal.Parse(MaxSupplyStr.Split(" ")[0]);
        public string BusinessValue => Data.GetValueOrDefault("business_value")?.ToString();

        [JsonIgnore] public string SupplyStr => Data.GetValueOrDefault("supply")?.ToString();

        [JsonIgnore] public string MaxSupplyStr => Data.GetValueOrDefault("max_supply")?.ToString();

        [JsonIgnore] public Dictionary<string, object> Data { get; set; }
    }
}