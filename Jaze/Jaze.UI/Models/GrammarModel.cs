using System;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class GrammarModel
    {
        public int Id { get; set; }
        public string Struct { get; set; }
        public string Meaning { get; set; }
        public string Detail { get; set; }
        public Level Level { get; set; }

        public static GrammarModel Create(Grammar grammar)
        {
            return new GrammarModel
            {
                Id = grammar.Id,
                Struct = grammar.Struct,
                Meaning = grammar.Meaning,
                Detail = grammar.Detail,
                Level = grammar.Level
            };
        }
    }
}