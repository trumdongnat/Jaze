using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.Document.JsonObject;
using Jaze.Model;
using Newtonsoft.Json;

namespace Jaze.Document
{
    internal static class JaEnBuilder
    {
        public static FlowDocument Build(JaEn jaen)
        {
            if (jaen == null)
            {
                return null;
            }

            //-------------start build ---------------------------
            FlowDocument document = new FlowDocument()
            {
                PagePadding = new Thickness(20)
            };

            //build document header
            document.Blocks.Add(BuildDocumentHeader(jaen.Word, jaen.Kana));
            //build mean
            if (jaen.Mean != null)
            {
                document.Blocks.Add(BuilderHelper.BuildWordMean(jaen.Mean));
            }
            //TODO verd division


            //return
            return document;
        }

        private static Block BuildDocumentHeader(string word, string kana)
        {
            //paragraph
            Paragraph paragragh = new Paragraph();

            //add word
            paragragh.Inlines.Add(new Bold(new Run(word)
            {
                FontSize = 25,
                Foreground = Brushes.Red
            }));

            if (kana != null)
            {
                paragragh.Inlines.Add(new LineBreak());
                paragragh.Inlines.Add(new Run(string.Concat("「", kana.Replace(" ","; "), "」"))
                {
                    FontStyle = FontStyles.Italic
                });
            }
            return paragragh;
        }

        private static Block BuildWordMean(string json)
        {

            if (string.IsNullOrWhiteSpace(json))
            {
                return null;
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

                //add mean
                item.Blocks.Add(new Paragraph()
                {
                    Inlines =
                    {
                        new Run(mean.Kind == null?"":$"({mean.Kind})")
                        {
                            Foreground = Brushes.Crimson
                        },
                        new LineBreak(),
                        new Run(mean.Mean)
                    }
                });
                list.ListItems.Add(item);
            }
            return list;
        }
    }
}
