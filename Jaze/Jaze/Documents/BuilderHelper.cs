using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.Documents.JsonObject;
using Jaze.Domain;
using Jaze.Domain.Entities;
using Jaze.Util;
using Newtonsoft.Json;

namespace Jaze.Documents
{
    static class BuilderHelper
    {
        public static Block BuildWordMean(string json, bool buildExample)
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
                    var examples = BuildJaViExamples(mean.Examples.ToArray());
                    examples.Padding = new Thickness(10, 0, 0, 0);
                    item.Blocks.Add(examples);
                }

                list.ListItems.Add(item);
            }
            return list;
        }

        public static Block BuildWordKanji(string word)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return new Section();
            }

            Table table = new Table() { CellSpacing = 0 };
            table.Columns.Add(new TableColumn() { Width = new GridLength(80) });
            table.Columns.Add(new TableColumn() { Width = new GridLength(100) });
            table.Columns.Add(new TableColumn() { Width = GridLength.Auto });
            table.RowGroups.Add(new TableRowGroup());

            var kanjis = StringUtil.FilterCharsInString(word, CharSet.Kanji);
            foreach (var c in kanjis)
            {
                string s = c.ToString();
                var kanji = JazeDatabaseContext.Context.Kanjis.FirstOrDefault(k => k.Word == s);
                if (kanji != null)
                {
                    table.RowGroups[0].Rows.Add(new TableRow()
                    {
                        Cells =
                        {
                            new TableCell(new Paragraph(new Run(kanji.Word))),
                            new TableCell(new Paragraph(new Run(kanji.HanViet))),
                            new TableCell(KanjiBuilder.BuildListViMean(kanji.VieMeaning))
                        }
                    });
                }
                else
                {
                    table.RowGroups[0].Rows.Add(new TableRow()
                    {
                        Cells =
                        {
                            new TableCell(new Paragraph(new Run(s))),
                            new TableCell(),
                            new TableCell()
                        }
                    });
                }
            }

            foreach (var row in table.RowGroups[0].Rows)
            {
                foreach (var cell in row.Cells)
                {
                    cell.BorderThickness = new Thickness(1);
                    cell.BorderBrush = Brushes.Black;
                    cell.Padding = new Thickness(3);
                }
            }
            return table;
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
            var context = JazeDatabaseContext.Context;
            return context.JaViExamples.Where(o => ids.Contains(o.Id)).ToList();
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
