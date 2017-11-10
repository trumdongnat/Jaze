using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jaze.Documents.JsonObject
{
    public class WordMean
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("examples")]
        public List<int> Examples { get; set; }

        [JsonProperty("mean")]
        public string Mean { get; set; }
    }
}
