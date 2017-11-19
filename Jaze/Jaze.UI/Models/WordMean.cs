using System.Collections.Generic;
using Newtonsoft.Json;

namespace Jaze.UI.Models
{
    public class WordMean
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("examples")]
        public List<int> ExampleIds { get; set; }

        [JsonProperty("mean")]
        public string Mean { get; set; }

        [JsonIgnore]
        public List<ExampleModel> Examples { get; set; }
    }
}