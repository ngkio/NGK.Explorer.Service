using System;
using System.Collections.Generic;

namespace Explorer.Service.DataAccess.DTO.Models
{
    public class TokenListModel
    {
        public TokenListModel()
        {
            Lists = new List<TokenInfoModel>();
            LastUpdateTime = DateTime.Now;
        }

        public DateTime LastUpdateTime { get; set; }

        public int TotalTokenCount { get; set; }

        public List<TokenInfoModel> Lists { get; set; }
    }
}