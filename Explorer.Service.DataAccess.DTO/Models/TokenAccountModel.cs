using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Explorer.Service.DataAccess.DTO.Models
{
    public class TokenAccountTempModel
    {
        public decimal Balance
        {
            get
            {
                var str = Data.GetValueOrDefault("balance")?.ToString().Split(' ').FirstOrDefault();
                decimal.TryParse(str, out var balance);
                return balance;
            }
        }

        public string AccountName { get; set; }

        [JsonIgnore] public Dictionary<string, object> Data { get; set; }
    }

    [Serializable]
    public class TokenAccountModel
    {
        public decimal Balance { get; set; }
        public string AccountName { get; set; }
    }
}