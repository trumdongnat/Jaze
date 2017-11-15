using System;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class JaviModel
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Kana { get; set; }
        public string Mean { get; set; }

        public static JaviModel Create(JaVi entity)
        {
            return new JaviModel
            {
                Id = entity.Id,
                Word = entity.Word,
                Kana = entity.Kana,
                Mean = entity.Mean,
            };
        }
    }
}