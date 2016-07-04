using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.DAO;
using Jaze.Model;
using Jaze.Util;

namespace Jaze.Document
{
    static class BuilderHelper
    {

        public static string BuildJapaneseSentence(string sentence, string hyperlinkColor = "Black")
        {
            if (string.IsNullOrWhiteSpace(sentence))
            {
                return string.Empty;
            }

            //divide
            //toCharArray not working
            //var arr = sentence.ToCharArray();
            //
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < sentence.Length; i++)
            {
                //s is kanji
                char c = sentence[i];
                if (ConvertStringUtil.IsKanji(c))
                {
                    builder.Append("<Hyperlink Foreground=\"")
                        .Append(hyperlinkColor)
                        .Append("\">")
                        .Append(c)
                        .Append("</Hyperlink>");
                }
                else
                {
                    builder.Append("<Run>").Append(c).Append("</Run>");
                }
            }

            return builder.ToString();

        }

        private static List<JaViExample> LoadExamples(int[] ids)
        {
            if (ids == null)
            {
                return new List<JaViExample>();
            }

            var examples = new List<JaViExample>();

            var context = DatabaseContext.Context;
            examples.AddRange(ids.Select(id => context.JaViExamples.Find(id)).Where(example => example != null));
            return examples;
        }

        public static Block BuildExamples(int[] listExamId)
        {
            var list = new List();
            
            //load example
            foreach (var exam in LoadExamples(listExamId))
            {
                var example = BuildExample(exam);
                if (example != null)
                {
                    list.ListItems.Add(new ListItem(example));
                }
                
            }

            return list;
        }


        private static Paragraph BuildExample(JaViExample exam)
        {
            
            if (string.IsNullOrWhiteSpace(exam?.Japanese))
            {
                return null;
            }
            return new Paragraph()
            {
                Margin = new Thickness(0, 5, 0, 0),
                LineHeight = 20,
                Inlines =
                {
                    new Run(exam.Japanese)
                    {
                        Foreground = Brushes.Blue,
                    },
                    new LineBreak(),
                    new Run(exam.VietNamese)
                    {
                        FontSize = 13,
                        Foreground = Brushes.Gray,
                        FontStyle = FontStyles.Italic
                    }
                }

            };
        }


    }
}
