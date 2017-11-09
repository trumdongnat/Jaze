using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.Document.JsonObject;
using Jaze.Domain.Entity;
using Jaze.Model;
using Newtonsoft.Json;

namespace Jaze.Document
{
    public static class JaViBuilder
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
                document.Blocks.Add(BuilderHelper.BuildWordMean(javi.Mean,true));
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

        public static FlowDocument BuildQuickView(JaVi javi)
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
                document.Blocks.Add(BuilderHelper.BuildWordMean(javi.Mean,false));
            }

            document.Blocks.Add(BuilderHelper.BuildWordKanji(javi.Word));

            //return
            return document;
        }
    }
}