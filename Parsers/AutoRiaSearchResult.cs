using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoDealersHelper.Parsers
{
    public class AutoRiaSearchResult
    {
        [JsonProperty("ids")]
        public List<int> Ids { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("last_id")]
        public int LastId { get; set; }
    }
}
