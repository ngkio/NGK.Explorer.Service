using System;

namespace Explorer.Service.DataAccess.DTO.Models
{
    public class TokenRecordModel
    {
        public long Id { get; set; }
        
        public string TrxId { get; set; }
        
        public int Type { get; set; }

        public string TypeShow { get; set; }

        public long FromAccountId { get; set; }
        
        public string FromAccountName { get; set; }

        public long ToAccountId { get; set; }
        
        public string ToAccountName { get; set; }
        
        public decimal Integral { get; set; }
        
        public string Memo { get; set; }
        
        public DateTime Time { get; set; }
    }
}