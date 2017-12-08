using System.Collections.Generic;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class KanjiModel : ModelBase
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string HanViet { get; set; }
        public string Variant { get; set; }
        public string Onyomi { get; set; }
        public string Kunyomi { get; set; }
        public string VieMeaning { get; set; }
        public string EngMeaning { get; set; }
        public int Stroke { get; set; }
        public string StrokeOrder { get; set; }
        public int Frequence { get; set; }
        public string Similar { get; set; }
        public string Component { get; set; }
        public Level Level { get; set; }
        public Grade Grade { get; set; }

        public RadicalModel Radical { get; set; }
        public List<PartModel> Parts { get; set; }
        public List<JaviModel> JaviModels { get; set; }

        public static KanjiModel Create(Kanji entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new KanjiModel
            {
                Id = entity.Id,
                Word = entity.Word,
                HanViet = entity.HanViet,
                Variant = entity.Variant,
                Onyomi = entity.Onyomi,
                Kunyomi = entity.Kunyomi,
                VieMeaning = entity.VieMeaning,
                EngMeaning = entity.EngMeaning,
                Stroke = entity.Stroke,
                StrokeOrder = entity.StrokeOrder,
                Frequence = entity.Frequence,
                Similar = entity.Similar,
                Component = entity.Component,
                Level = entity.Level,
                Grade = entity.Grade,
            };
        }

        public void Copy(Kanji entity)
        {
            if (entity == null)
            {
                return;
            }

            Id = entity.Id;
            Word = entity.Word;
            HanViet = entity.HanViet;
            Variant = entity.Variant;
            Onyomi = entity.Onyomi;
            Kunyomi = entity.Kunyomi;
            VieMeaning = entity.VieMeaning;
            EngMeaning = entity.EngMeaning;
            Stroke = entity.Stroke;
            StrokeOrder = entity.StrokeOrder;
            Frequence = entity.Frequence;
            Similar = entity.Similar;
            Component = entity.Component;
            Level = entity.Level;
            Grade = entity.Grade;
        }
    }
}