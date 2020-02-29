using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.UI.Models;

namespace Jaze.UI.Services.Documents
{
    public class JaenBuilder : BuilderBase<JaenModel>
    {
        private Block BuildDocumentHeader(string word, string kana)
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
                paragragh.Inlines.Add(new Run(string.Concat("「", kana.Replace(" ", "; "), "」"))
                {
                    FontStyle = FontStyles.Italic
                });
            }
            return paragragh;
        }

        public override FlowDocument Build(JaenModel jaen)
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
            if (jaen.MeanText != null)
            {
                document.Blocks.Add(BuildWordMean(jaen.Means, true));
            }

            //TODO kanji in word
            //document.Blocks.Add(BuildWordKanji(jaen.Word));

            //TODO verd division

            //return
            return document;
        }

        public override FlowDocument BuildLite(JaenModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}