using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.UI.Models;

namespace Jaze.UI.Services.Documents
{
    public abstract class BuilderBase<TModel> : IBuilder<TModel>
    {
        public abstract FlowDocument Build(TModel model);

        public abstract FlowDocument BuildLite(TModel model);

        protected Block BuildJaviExamples(List<ExampleModel> examples)
        {
            var list = new List();

            //load example
            foreach (var example in examples)
            {
                var exampleView = BuildJaviExample(example);
                if (exampleView != null)
                {
                    list.ListItems.Add(new ListItem(exampleView));
                }
            }
            return list;
        }

        private Paragraph BuildJaviExample(ExampleModel example)
        {
            if (string.IsNullOrWhiteSpace(example?.Japanese))
            {
                return null;
            }
            return new Paragraph()
            {
                Margin = new Thickness(0, 5, 0, 0),
                LineHeight = 20,
                Inlines =
                {
                    new Run(example.Japanese)
                    {
                        Foreground = Brushes.Blue,
                    },
                    new LineBreak(),
                    new Run(example.VietNamese)
                    {
                        FontSize = 13,
                        Foreground = Brushes.Gray,
                        FontStyle = FontStyles.Italic
                    }
                }
            };
        }

        protected Block BuildWordMean(List<WordMean> means, bool buildExample)
        {
            if (means == null || means.Count == 0)
            {
                return new Section();
            }
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
                if (buildExample && mean.Examples != null && mean.Examples.Count > 0)
                {
                    var examples = BuildJaviExamples(mean.Examples);
                    examples.Padding = new Thickness(10, 0, 0, 0);
                    item.Blocks.Add(examples);
                }

                list.ListItems.Add(item);
            }
            return list;
        }

        //public static Block BuildWordKanji(string word)
        //{
        //    if (string.IsNullOrWhiteSpace(word))
        //    {
        //        return new Section();
        //    }

        //    Table table = new Table() { CellSpacing = 0 };
        //    table.Columns.Add(new TableColumn() { Width = new GridLength(80) });
        //    table.Columns.Add(new TableColumn() { Width = new GridLength(100) });
        //    table.Columns.Add(new TableColumn() { Width = GridLength.Auto });
        //    table.RowGroups.Add(new TableRowGroup());

        //    var kanjis = StringUtil.FilterCharsInString(word, CharSet.Kanji);
        //    foreach (var c in kanjis)
        //    {
        //        string s = c.ToString();
        //        var kanji = JazeDatabaseContext.Context.Kanjis.FirstOrDefault(k => k.Word == s);
        //        if (kanji != null)
        //        {
        //            table.RowGroups[0].Rows.Add(new TableRow()
        //            {
        //                Cells =
        //                    {
        //                        new TableCell(new Paragraph(new Run(kanji.Word))),
        //                        new TableCell(new Paragraph(new Run(kanji.HanViet))),
        //                        new TableCell(KanjiBuilder.BuildListViMean(kanji.VieMeaning))
        //                    }
        //            });
        //        }
        //        else
        //        {
        //            table.RowGroups[0].Rows.Add(new TableRow()
        //            {
        //                Cells =
        //                    {
        //                        new TableCell(new Paragraph(new Run(s))),
        //                        new TableCell(),
        //                        new TableCell()
        //                    }
        //            });
        //        }
        //    }

        //    foreach (var row in table.RowGroups[0].Rows)
        //    {
        //        foreach (var cell in row.Cells)
        //        {
        //            cell.BorderThickness = private newThickness(1);

        //            cell.BorderBrush = Brushes.Black;

        //            cell.Padding = new Thickness(3);
        //        }
        //    }

        //    return table;
        //}
    }
}