using System;
using System.Collections.Generic;
using Jaze.Domain.Entities;
using Newtonsoft.Json;

namespace Jaze.UI.Models
{
    public class GrammarModel
    {
        public int Id { get; set; }
        public string Struct { get; set; }
        public string Meaning { get; set; }
        public string DetailText { get; set; }
        public GrammarDetail[] Detail { get; set; }
        public Level Level { get; set; }

        public static GrammarModel Create(Grammar grammar)
        {
            return new GrammarModel
            {
                Id = grammar.Id,
                Struct = grammar.Struct,
                Meaning = grammar.Meaning,
                DetailText = grammar.Detail,
                Level = grammar.Level
            };
        }
    }

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
        public int[] ExampleIds { get; set; }

        [JsonIgnore]
        public List<ExampleModel> Examples { get; set; }
    }
}