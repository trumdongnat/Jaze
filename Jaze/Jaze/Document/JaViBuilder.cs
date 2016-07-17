using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.Document.JsonObject;
using Jaze.Model;
using Newtonsoft.Json;

namespace Jaze.Document
{
    internal static class JaViBuilder
    {

        public static FlowDocument Build(JaVi javi)
        {
            if (javi == null)
            {
                return null;
            }

            //-------------start build ---------------------------
            FlowDocument document = new FlowDocument()
            {
                PagePadding = new Thickness(20)
            };

            //build document header
            document.Blocks.Add(BuildDocumentHeader(javi.Word, javi.Kana));
            //build Vi mean
            if (javi.Mean != null)
            {
                document.Blocks.Add(BuilderHelper.BuildWordMean(javi.Mean));
            }

            document.Blocks.Add(BuilderHelper.BuildWordKanji(javi.Word));

            //TODO verd division
            

            //return
            return document;
        }

        private static Block BuildDocumentHeader(string word, string kana)
        {
            //paragraph
            Paragraph paragragh = new Paragraph()
            {
                LineHeight = 30
            };

            //add word
            paragragh.Inlines.Add(new Bold(new Run(word)
            {
                FontSize = 25,
                Foreground = Brushes.Red
            }));

            if (kana != null)
            {
                paragragh.Inlines.Add(new LineBreak());
                paragragh.Inlines.Add(new Run(string.Concat("「",kana, "」"))
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
            var means =  JsonConvert.DeserializeObject<WordMean[]>(json);
            //build list mean
            List list = new List()
            {
                MarkerStyle = TextMarkerStyle.Decimal,
                Padding = new Thickness(20,0,0,0)
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
                    var examples = BuilderHelper.BuildJaViExamples(mean.Examples.ToArray());
                    examples.Padding = new Thickness(10,0,0,0);
                    item.Blocks.Add(examples);
                }

                //paragragh.Inlines.Add(new LineBreak());
                //TODO add sub list
                //
                
                list.ListItems.Add(item);
            }
            return list;
        }

        //internal static string BuildQuickView(JaVocab jaVocab)
        //{
        //    if (jaVocab == null)
        //    {
        //        return null;
        //    }

        //    //load full javocab
        //    DatabaseManager.LoadFullJaVocab(jaVocab);

        //    //-------------start build ---------------------------
        //    StringBuilder doc = new StringBuilder("<FlowDocument " + xmls + " PagePadding=\"10\">");

        //    //build document header
        //    doc.Append(BuildDocumentHeader(jaVocab.Word, jaVocab.Kana));
        //    //build Vi mean
        //    if (jaVocab.ViDetail != null)
        //    {
        //        doc.Append(BuildQuickViewViMean(jaVocab.ViDetail));
        //    }
        //    //build En mean
        //    //build verd division

        //    //return
        //    doc.Append("</FlowDocument>");
        //    return doc.ToString();
        //}

        //private static string BuildQuickViewViMean(string json)
        //{
        //    return "<Section Margin=\"0\">" + BuildTitle("Nghĩa Tiếng Việt") + BuildQuickViewViMeanContent(json) + "</Section >";
        //}

        //private static string BuildQuickViewViMeanContent(string json)
        //{
        //    //load json
        //    var ja = JsonConvert.DeserializeObject<JaVocabJson>(json);
        //    //build list mean
        //    StringBuilder listMean = new StringBuilder("<List MarkerStyle=\"Decimal\" Margin=\"0\" Padding=\"30,0,0,0\">");

        //    foreach (var mean in ja.Means)
        //    {
        //        listMean.Append("<ListItem>");
        //        //add mean
        //        listMean.Append("<Paragraph>");
        //        if (!string.IsNullOrWhiteSpace(mean.Kind))
        //        {
        //            listMean.Append("(" + mean.Kind + ") ");
        //        }
        //        listMean.Append(mean.Mean);
        //        listMean.Append("</Paragraph>");

        //        //TODO add sub list
        //        //
        //        listMean.Append("</ListItem>");
        //    }
        //    listMean.Append("</List>");
        //    return listMean.ToString();
        //}
    }
}