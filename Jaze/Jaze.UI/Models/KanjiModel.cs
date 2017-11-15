using System;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class KanjiModel
    {
        public string Word { get; set; }

        public static KanjiModel Create(Kanji entity)
        {
            throw new NotImplementedException();
        }
    }
}