using System.Collections.Generic;
using Jaze.Domain.Definitions;
using Jaze.UI.Definitions;

namespace Jaze.UI.Models
{
    public class Dictionary
    {
        public DictionaryType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        private Dictionary(DictionaryType type, string name, string description)
        {
            Type = type;
            Name = name;
            Description = description;
        }

        private static readonly List<Dictionary> LsDics;

        static Dictionary()
        {
            LsDics = new List<Dictionary>
            {
                new Dictionary(DictionaryType.JaVi, "Nhật Việt",
                    "Từ điển Nhật-Việt.\r\nCó thể tìm kiếm bằng từ vựng hoặc kana."),
                new Dictionary(DictionaryType.HanViet, "Hán Việt",
                    "Từ điển Trung-Việt.\r\nCó thể tìm kiếm bằng từ vựng hoặc cách đọc Hán Việt."),
                new Dictionary(DictionaryType.Kanji, "Kanji",
                    "Danh sách Word.\r\nCó thể tìm kiếm bằng Kanji hoặc cách đọc Hán Việt."),
                new Dictionary(DictionaryType.ViJa, "Việt Nhật",
                    "Từ điển Việt-Nhật.\r\nCó thể tìm kiếm bằng từ vựng tiếng Việt."),
                new Dictionary(DictionaryType.JaEn, "Nhật Anh",
                    "Từ điển Nhật-Anh.\r\nCó thể tìm kiếm bằng từ vựng hoặc kana."),
                new Dictionary(DictionaryType.Grammar, "Ngữ Pháp",
                    "Danh sách ngữ pháp.\r\nCó thể tìm kiếm bằng mẫu ngữ pháp.")
            };
        }

        public static List<Dictionary> GetDictionarys()
        {
            return LsDics;
        }
    }
}