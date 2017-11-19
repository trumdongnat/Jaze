using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.UI.Models;

namespace Jaze.UI.Services.Documents
{
    public class VijaBuilder : BuilderBase<VijaModel>
    {
        public override FlowDocument Build(VijaModel vija)
        {
            if (vija == null)
            {
                return null;
            }

            //-------------start build ---------------------------
            FlowDocument document = new FlowDocument()
            {
                PagePadding = new Thickness(20)
            };

            //build document header
            document.Blocks.Add(BuildDocumentHeader(vija.Word));
            //build Vi mean
            document.Blocks.Add(BuildWordMean(vija.Means, true));
            return document;
        }

        public override FlowDocument BuildLite(VijaModel model)
        {
            throw new System.NotImplementedException();
        }

        private Block BuildDocumentHeader(string word)
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

        //private Block BuildWordMean(List<WordMean> means)
        //{
        //    if (means == null)
        //    {
        //        return null;
        //    }
        //    //build list mean
        //    List list = new List()
        //    {
        //        MarkerStyle = TextMarkerStyle.Decimal,
        //        Padding = new Thickness(20, 0, 0, 0)
        //    };

        //    foreach (var mean in means)
        //    {
        //        ListItem item = new ListItem();

        //        //add mean
        //        item.Blocks.Add(new Paragraph()
        //        {
        //            Inlines =
        //            {
        //                new Run(mean.Kind == null?"":$"({mean.Kind})"),
        //                new Run(mean.Mean)
        //            }
        //        });

        //        //add example
        //        if (mean.Examples != null && mean.Examples.Count > 0)
        //        {
        //            var examples = BuildJaviExamples(mean.Examples);
        //            examples.Padding = new Thickness(10, 0, 0, 0);
        //            item.Blocks.Add(examples);
        //        }

        //        //paragragh.Inlines.Add(new LineBreak());
        //        //TODO add sub list
        //        //

        //        list.ListItems.Add(item);
        //    }
        //    return list;
        //}
    }
}