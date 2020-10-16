using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Explorer.Service.Contract.Response
{
    public class GetProducerModel
    {
        public string TotalVoteWeight { get; set; }

        public List<GetProducerResponse> List { get; set; }
    }

    public class GetGlobalResponse
    {
        [JsonProperty("total_producer_vote_weight")]
        public string TotalProducerVoteWeight { get; set; }
    }

    public class GetProducerResponse
    {
        [JsonProperty("owner")] public string Owner { get; set; }

        [JsonProperty("total_votes")] public decimal TotalVotes { get; set; }

        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("producer_key")] public string ProducerKey { get; set; }

        [JsonProperty("is_active")] public int IsActive { get; set; }

        [JsonProperty("unpaid_blocks")] public long UnpaidBlocks { get; set; }

        [JsonProperty("last_claim_time")] public DateTime LastClaimTime { get; set; }

        [JsonProperty("location")] public string Location { get; set; }
    }
}