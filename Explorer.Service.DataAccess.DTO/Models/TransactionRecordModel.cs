using System;
using System.Collections.Generic;
using System.Linq;
using Thor.Framework.Common.Helper.Extensions;
using Newtonsoft.Json;

namespace Explorer.Service.DataAccess.DTO.Models
{
    public class TransferRecordModel
    {
        public string From => TempData.GetValueOrDefault("from")?.ToString() ?? TempData.GetValueOrDefault("payer")?.ToString();
        public string To => TempData.GetValueOrDefault("to")?.ToString();
        public string Quantity => TempData.GetValueOrDefault("quantity")?.ToString();
        public string FeeQuantity => TempData.GetValueOrDefault("fee")?.ToString();
        public string Memo => TempData.GetValueOrDefault("memo")?.ToString();

        public string ActionAccount { get; set; }
        public string ActName { get; set; }
        public long BlockNum { get; set; }
        public string Status { get; set; }
        public DateTime? BlockTime { get; set; }
        public string TransactionId { get; set; }

        [JsonIgnore] public byte[] ActData { get; set; }

        [JsonIgnore] public Dictionary<string, object> TempData { get; set; }

        [JsonIgnore] public string Symbol => Quantity?.Split(' ').LastOrDefault();
    }

    public class TransactionDetailModel
    {
        public string TransactionId { get; set; }
        public string ActionAccount { get; set; }
        public string ActName { get; set; }
        public long BlockNum { get; set; }
        public string Status { get; set; }
        public DateTime? BlockTime { get; set; }

        [JsonIgnore] public byte[] TempData { get; set; }

        public Dictionary<string, object> ActData { get; set; }
    }

    [Serializable]
    public class TransactionRecordModel
    {
        private string _transactionId;

        public string TransactionId
        {
            get => _transactionId?.ToLower();
            set => _transactionId = value;
        }

        public long BlockNum { get; set; }

        public DateTime? Timestamp { get; set; }

        public long? TimestampShow => Timestamp?.AddHours(8).ToTimestamp();

        public string Type { get; set; }

        public Dictionary<string, object> ActData { get; set; }

        [JsonIgnore] public string ActName { get; set; }
        public string ActAccount { get; set; }
        [JsonIgnore] public byte[] TempData { get; set; }
    }

    public sealed class TransactionRawDataModel
    {
        [JsonProperty("block_num")] public long BlockNum { get; set; }
        [JsonProperty("transaction_ordinal")] public int TransactionOrdinal { get; set; }
        [JsonProperty("failed_dtrx_trace")] public string FailedDtrxTrace { get; set; }

        private string _id;

        [JsonProperty("id")]
        public string Id
        {
            get => _id?.ToLower();
            set => _id = value;
        }

        [JsonProperty("status")] public string status { get; set; }
        [JsonProperty("cpu_usage_us")] public long? CpuUsageUs { get; set; }
        [JsonProperty("net_usage_words")] public long? NetUsageWords { get; set; }
        [JsonProperty("elapsed")] public long? Elapsed { get; set; }
        [JsonProperty("net_usage")] public decimal? NetUsage { get; set; }
        [JsonProperty("scheduled")] public bool? Scheduled { get; set; }

        [JsonProperty("account_ram_delta_present")]
        public bool? AccountRamDeltaPresent { get; set; }

        [JsonProperty("account_ram_delta_account")]
        public string AccountRamDeltaAccount { get; set; }

        [JsonProperty("account_ram_delta_delta")]
        public long? AccountRamDeltaDelta { get; set; }

        [JsonProperty("except")] public string Except { get; set; }
        [JsonProperty("error_code")] public decimal? ErrorCode { get; set; }
        [JsonProperty("partial_present")] public bool? PartialPresent { get; set; }
        [JsonProperty("partial_expiration")] public DateTime? PartialExpiration { get; set; }

        [JsonProperty("partial_ref_block_num")]
        public int? PartialRefBlockNum { get; set; }

        [JsonProperty("partial_ref_block_prefix")]
        public long? PartialRefBlockPrefix { get; set; }

        [JsonProperty("partial_max_net_usage_words")]
        public long? PartialMaxNetUsageWords { get; set; }

        [JsonProperty("partial_max_cpu_usage_ms")]
        public short? PartialMaxCpuUsageMs { get; set; }

        [JsonProperty("partial_delay_sec")] public long? PartialDelaySec { get; set; }
        [JsonProperty("partial_signatures")] public string[] PartialSignatures { get; set; }

        [JsonProperty("partial_context_free_data")]
        public byte[][] PartialContextFreeData { get; set; }

        [JsonProperty("actions")] public List<ActionTraceRawModel> Actions { get; set; }
    }

    public class ActionTraceRawModel
    {
        [JsonProperty("block_num")] public long BlockNum { get; set; }

        private string _transactionId;

        [JsonProperty("transaction_id")]
        public string TransactionId
        {
            get => _transactionId?.ToLower();
            set => _transactionId = value;
        }

        [JsonProperty("transaction_status")] public string TransactionStatus { get; set; }
        [JsonProperty("action_ordinal")] public long ActionOrdinal { get; set; }

        [JsonProperty("creator_action_ordinal")]
        public long? CreatorActionOrdinal { get; set; }

        [JsonProperty("receipt_present")] public bool? ReceiptPresent { get; set; }
        [JsonProperty("receipt_receiver")] public string ReceiptReceiver { get; set; }
        [JsonProperty("receipt_act_digest")] public string ReceiptActDigest { get; set; }

        [JsonProperty("receipt_global_sequence")]
        public decimal? ReceiptGlobalSequence { get; set; }

        [JsonProperty("receipt_recv_sequence")]
        public decimal? ReceiptRecvSequence { get; set; }

        [JsonProperty("receipt_code_sequence")]
        public long? ReceiptCodeSequence { get; set; }

        [JsonProperty("receipt_abi_sequence")] public long? ReceiptAbiSequence { get; set; }
        [JsonProperty("receiver")] public string Receiver { get; set; }
        [JsonProperty("act_account")] public string ActAccount { get; set; }
        [JsonProperty("act_name")] public string ActName { get; set; }
        [JsonIgnore] public byte[] ActData { get; set; }
        [JsonProperty("context_free")] public bool? ContextFree { get; set; }
        [JsonProperty("elapsed")] public long? Elapsed { get; set; }
        [JsonProperty("console")] public string Console { get; set; }
        [JsonProperty("except")] public string Except { get; set; }
        [JsonProperty("error_code")] public decimal? ErrorCode { get; set; }
        [JsonProperty("act_data")] public Dictionary<string, object> ActTempData { get; set; }
    }

    public class TransactionModel : TransactionRecordModel
    {
        public long ActionOrdinal { get; set; }
        public long? CreatorActionOrdinal { get; set; }

        public override int GetHashCode()
        {
            return JsonConvert.SerializeObject(ActData).GetHashCode();
        }
    }

    public class TransactionOfAccountModel : TransactionRecordModel
    {
    }

    public class TransactionOfTokenModel : TransactionRecordModel
    {
    }
}