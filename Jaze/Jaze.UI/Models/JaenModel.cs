using System;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class JaenModel
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Kana { get; set; }
        public string Mean { get; set; }

        public static JaenModel Create(JaEn entity)
        {
            return new JaenModel
            {
                Id = entity.Id,
                Word = entity.Word,
                Kana = entity.Kana,
                Mean = entity.Mean,
            };
        }
    }
}