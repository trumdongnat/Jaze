using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.UI.Models;

namespace Jaze.UI.Services.Documents
{
    public class JaviBuilder : BuilderBase<JaviModel>
    {
        public override FlowDocument Build(JaviModel javi)
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
            if (javi.MeanText != null)
            {
                document.Blocks.Add(BuildWordMean(javi.Means, true));
            }

            //TODO kanji in word
            //document.Blocks.Add(BuilderHelper.BuildWordKanji(javi.Word));

            //TODO verd division

            //return
            return document;
        }

        public override FlowDocument BuildLite(JaviModel javi)
        {
            //return
            return Build(javi);
        }

        private Block BuildDocumentHeader(string word, string kana)
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
                paragragh.Inlines.Add(new Run(string.Concat("「", kana, "」"))
                {
                    FontStyle = FontStyles.Italic
                });
            }
            return paragragh;
        }
    }
}