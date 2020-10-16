using System.ComponentModel.DataAnnotations;
using Thor.Framework.Common.Pager;

namespace Explorer.Service.DataAccess.DTO.Params
{
    public class TransferQueryParams : QueryParam
    {
        [Required]
        public string AccountName { get; set; }

        public string TokenSymbol { get; set; }

        public string TokenCode { get; set; }

        public long? BlockStart { get; set; }
        public long? BlockEnd { get; set; }
    }
}