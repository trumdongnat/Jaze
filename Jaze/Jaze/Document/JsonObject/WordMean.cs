using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jaze.Document.JsonObject
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
