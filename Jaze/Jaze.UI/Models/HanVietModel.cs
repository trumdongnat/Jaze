using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class HanVietModel : ModelBase
    {
        public int Id { get; set; }
        public string Word { get; set; }
        public string Reading { get; set; }
        public string Content { get; set; }

        public static HanVietModel Create(HanViet hanViet)
        {
            return new HanVietModel
            {
                Id = hanViet.Id,
                Word = hanViet.Word,
                Reading = hanViet.Reading,
                Content = hanViet.Content
            };
        }
    }
}