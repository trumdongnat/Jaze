using Newtonsoft.Json;

namespace Jaze.Documents.JsonObject
{
    public class GrammarDetail
    {
        [JsonProperty("synopsis")]
        public string Synopsis { get; set; }

        [JsonProperty("explain")]
        public string Explain { get; set; }

        [JsonProperty("mean")]
        public string Mean { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("examples")]
        public int[] Examples { get; set; }
    }
}
