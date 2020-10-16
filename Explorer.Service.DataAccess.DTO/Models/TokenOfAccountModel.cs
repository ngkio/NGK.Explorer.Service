using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Explorer.Service.DataAccess.DTO.Models
{
    public class TokenOfAccountModel
    {
        public string Symbol => Balance?.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1];

        public string Contract { get; set; }

        public decimal? Amount => decimal.Parse(Balance.Split(" ", StringSplitOptions.RemoveEmptyEntries)[0]);

        public int Precision { get; set; }

        public string Balance => Data.GetValueOrDefault("balance")?.ToString();

        [JsonIgnore] public Dictionary<string, object> Data { get; set; }
    }
}