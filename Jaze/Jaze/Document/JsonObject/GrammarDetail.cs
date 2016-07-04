using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Jaze.Document.JsonObject
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
