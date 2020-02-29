using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class ExampleModel
    {
        public int Id { get; set; }

        public string Japanese { get; set; }

        public string VietNamese { get; set; }

        public static ExampleModel Create(JaViExample jaVi)
        {
            return new ExampleModel
            {
                Id = jaVi.Id,
                Japanese = jaVi.Japanese,
                VietNamese = jaVi.VietNamese
            };
        }

        public static ExampleModel Create(JaEnExample jaen)
        {
            return new ExampleModel
            {
                Id = jaen.Id,
                Japanese = jaen.Japanese,
                VietNamese = jaen.English
            };
        }
    }
}