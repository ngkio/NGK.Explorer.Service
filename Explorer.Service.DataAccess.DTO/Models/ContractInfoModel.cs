using System;
using System.Collections.Generic;
using Explorer.Service.DataAccess.Entities;
using Explorer.Service.DataAccess.Entities.Enums;
using Thor.Framework.Common.Helper.Extensions;

namespace Explorer.Service.DataAccess.DTO.Models
{
    public class ContractRowTempModel
    {
        public ContractRow ContractRow { get; set; }
        public Dictionary<string, object> Data { get; set; }

        public string Account => Data?.GetValueOrDefault("from_account_name")?.ToString();
    }

    public class ContractStatisticalModel
    {
        public string Name { get; set; }
        public long TotalActiveAccountCount { get; set; }
        public long TotalCallCount { get; set; }
    }

    public class ContractInfoModel : ContractStatisticalModel
    {
        public string Actions { get; set; }
        public EnumOneTimeCheckState OneTimeCheckState { get; set; }
        public EnumCodeAuditStatus CodeAuditStatus { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public long? LastUpdateTimeShow => LastUpdateTime?.AddHours(8).ToTimestamp();
    }
}