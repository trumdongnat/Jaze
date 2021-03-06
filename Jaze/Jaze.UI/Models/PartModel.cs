﻿using System.Collections.Generic;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class PartModel : ModelBase
    {
        public int Id { get; set; }
        public string Value { get; set; }
        public string Word { get; set; }
        public int Stroke { get; set; }

        public ICollection<KanjiModel> Kanjis { get; set; }

        public static PartModel Create(Part entity)
        {
            return new PartModel
            {
                Id = entity.Id,
                Value = entity.Value,
                Word = entity.Word,
                Stroke = entity.Stroke,
            };
        }
    }
}