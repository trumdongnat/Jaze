using System;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class VijaModel
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Mean { get; set; }

        public static VijaModel Create(ViJa entity)
        {
            return new VijaModel
            {
                Id = entity.Id,
                Word = entity.Word,
                Mean = entity.Mean,
            };
        }
    }
}