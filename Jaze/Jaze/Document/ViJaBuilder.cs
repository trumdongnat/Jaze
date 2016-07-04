using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.Document.JsonObject;
using Jaze.Model;
using Newtonsoft.Json;

namespace Jaze.Document
{
    static class ViJaBuilder
    {

        public static FlowDocument Build(ViJa vija)
        {
            if (vija == null)
            {
                return null;
            }
            var mean = JsonConvert.DeserializeObject<WordMean[]>(vija.Mean);

            //-------------start build ---------------------------
            FlowDocument document = new FlowDocument()
            {
                PagePadding = new Thickness(20)
            };

            //build document header
            document.Blocks.Add(BuildDocumentHeader(vija.Word));
            //build Vi mean
            document.Blocks.Add(BuildWordMean(vija.Mean));
            return document;
        }

        private static Block BuildDocumentHeader(string word)
        {
            Paragraph paragraph = new Paragraph()
            {
                LineHeight = 30
            };
            paragraph.Inlines.Add(new Bold(new Run(word))
            {
                FontSize = 18,
                Foreground = Brushes.Red
            });
            return paragraph;
        }

        private static Block BuildWordMean(string sMeans)
        {

            if (string.IsNullOrWhiteSpace(sMeans))
            {
                return null;
            }
            var means = JsonConvert.DeserializeObject<WordMean[]>(sMeans);
            //build list mean
            List list = new List()
            {
                MarkerStyle = TextMarkerStyle.Decimal,
                Padding = new Thickness(20, 0, 0, 0)
            };


            foreach (var mean in means)
            {
                ListItem item = new ListItem();

                //add mean
                item.Blocks.Add(new Paragraph()
                {
                    Inlines =
                    {
                        new Run(mean.Kind == null?"":$"({mean.Kind})"),
                        new Run(mean.Mean)
                    }
                });

                //add example
                if (mean.Examples != null && mean.Examples.Count > 0)
                {
                    var examples = BuilderHelper.BuildExamples(mean.Examples.ToArray());
                    examples.Padding = new Thickness(10, 0, 0, 0);
                    item.Blocks.Add(examples);
                }

                //paragragh.Inlines.Add(new LineBreak());
                //TODO add sub list
                //

                list.ListItems.Add(item);
            }
            return list;
        }


    }
}
