using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.DAO;
using Jaze.Document.JsonObject;
using Jaze.Model;
using Newtonsoft.Json;

namespace Jaze.Document
{
    static class BuilderHelper
    {
        public static Block BuildWordMean(string json)
        {

            if (string.IsNullOrWhiteSpace(json))
            {
                return new Section();
            }
            var means = JsonConvert.DeserializeObject<WordMean[]>(json);
            //build list mean
            List list = new List()
            {
                MarkerStyle = TextMarkerStyle.Decimal,
                Padding = new Thickness(20, 0, 0, 0),
            };


            foreach (var mean in means)
            {
                ListItem item = new ListItem();
                //item.Margin = new Thickness(0);
                list.StartIndex = 2;
                //add mean
                var paragragh = new Paragraph();
                if (!string.IsNullOrWhiteSpace(mean.Kind))
                {
                    paragragh.Inlines.Add(new Run($"({mean.Kind})")
                    {
                        Foreground = Brushes.Crimson
                    });
                    //paragragh.Inlines.Add(new LineBreak());
                }
                paragragh.Inlines.Add(new Run(mean.Mean));
                item.Blocks.Add(paragragh);
                
                //add example
                if (mean.Examples != null && mean.Examples.Count > 0)
                {
                    var examples = BuildJaViExamples(mean.Examples.ToArray());
                    examples.Padding = new Thickness(10, 0, 0, 0);
                    item.Blocks.Add(examples);
                }

                list.ListItems.Add(item);
            }
            return list;
        }

        public static Block BuildJaViExamples(int[] listExamId)
        {
            var list = new List();
            
            //load example
            foreach (var exam in LoadJaViExamples(listExamId))
            {
                var example = BuildJaViExample(exam);
                if (example != null)
                {
                    list.ListItems.Add(new ListItem(example));
                }
                
            }

            return list;
        }

        private static List<JaViExample> LoadJaViExamples(int[] ids)
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


        private static Paragraph BuildJaViExample(JaViExample exam)
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
