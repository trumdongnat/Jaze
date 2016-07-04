using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.Model;

namespace Jaze.Document
{
    internal static class KanjiBuilder
    {
        #region Build Full Infomation

        public static FlowDocument Build(Kanji kanji)
        {
            if (kanji == null)
            {
                return null;
            }

            //start build
            FlowDocument document = new FlowDocument()
            {
                PagePadding = new Thickness(10)
            };

            Table table = new Table();
            document.Blocks.Add(table);
            table.RowGroups.Add(new TableRowGroup());
            table.RowGroups[0].Rows.Add(new TableRow()
            {
                Cells =
                {
                    new TableCell(BuildAttributeSet(kanji)),
                    new TableCell(BuildKanjiStroke(kanji.Word, kanji.HanViet))
                }
            });

            table.RowGroups[0].Rows.Add(new TableRow()
            {
                Cells =
                {
                    new TableCell(BuildViMean(kanji.VieMeaning)),
                    new TableCell(BuildEnMean(kanji.EngMeaning))
                }
            });

            return document;
        }

   

        private static Block BuildAttributeSet(Kanji kanji)
        {
            List list = new List()
            {
                MarkerStyle = TextMarkerStyle.Box
            };

            //add list attribute
            list.ListItems.Add(BuildAttribute("Bộ thủ: ", $"{kanji.Radical.Word}({kanji.Radical.HanViet})"));
            list.ListItems.Add(BuildAttribute("Cách viết khác: ", kanji.Variant));
            list.ListItems.Add(BuildAttribute("Onyomi: ", kanji.Onyomi));
            list.ListItems.Add(BuildAttribute("Kunyomi: ", kanji.Kunyomi));
            list.ListItems.Add(BuildAttribute("Số nét: ", "" + kanji.Stroke));
            list.ListItems.Add(BuildAttribute("Độ phổ biến: ",
                kanji.Frequence == int.MaxValue ? "?/2500" : "" + kanji.Frequence + "/2500"));
            list.ListItems.Add(BuildAttribute("Trình độ: ", kanji.Level == null ? "": kanji.Level.Name));
            list.ListItems.Add(BuildAttribute("Thành phần: ", kanji.Component));
            list.ListItems.Add(BuildAttribute("Gần giống: ", kanji.Similar));
            return list;
        }

        private static ListItem BuildAttribute(string name, string contain)
        {
            ListItem item = new ListItem();
            item.Blocks.Add(new Paragraph()
            {
                Inlines =
                {
                    new Run(name)
                    {
                        Foreground = Brushes.Gray,
                        FontSize = 14
                    },
                    new Run(contain)
                }
            });

            return item;
        }

        private static Block BuildKanjiStroke(string kanji, string hanviet)
        {
            Paragraph paragraph = new Paragraph()
            {
                TextAlignment = TextAlignment.Center,
                Inlines = { new Run(kanji)
                {
                    FontSize = 150,
                    FontFamily = new FontFamily("KanjiStrokeOrders")
                } ,
                new LineBreak(),
                new Run(hanviet)
                }
            };
            return paragraph;
        }

        private static Block BuildViMean(string viMean)
        {
            if (string.IsNullOrWhiteSpace(viMean))
            {
                return new Section();
            }
            Section section = new Section();
            //title
            section.Blocks.Add(new Paragraph(new Run("Nghĩa tiếng Việt")));
            //list mean
            List list = new List()
            {
                MarkerStyle =  TextMarkerStyle.Square,
                MarkerOffset = 5
            };

            //build list hv + mean
            var dic = AnalyseVimean(viMean);
            foreach (var key in dic.Keys)
            {
                ListItem item = new ListItem();
                item.Blocks.Add(new Paragraph(new Run(key)));
                
                //add list mean for each key
                List subList = new List();
                
                foreach (var mean in dic[key])
                {
                    subList.ListItems.Add(new ListItem()
                    {
                        Blocks = { new Paragraph(new Run(mean))}
                    });
                    
                }
                item.Blocks.Add(subList);
                list.ListItems.Add(item);
            }
            section.Blocks.Add(list);
            return section;
        }

        private static Dictionary<string, List<string>> AnalyseVimean(string vimean)
        {
            var result = new Dictionary<string, List<string>>();
            var matches = Regex.Matches(vimean, "{(.*?)}");

            foreach (Match match in matches)
            {
                var s = match.Groups[1].Value;
                //TODO get Han Viet + list mean
                //var m = Regex.Match(s, "^\\((.*(?)\\)(.*)$");
                //string hv = m.Groups[1].Value;
                //string means = m.Groups[2].Value;
                int pos = s.IndexOf(")");
                string hv = s.Substring(1, pos - 1);
                string means = s.Substring(pos + 1);
                var arr = Regex.Split(means, "##").ToList();
                result.Add(hv, arr);
            }
            return result;
        }

        private static Block BuildEnMean(string enMean)
        {
            if (string.IsNullOrWhiteSpace(enMean))
            {
                return null;
            }
            Section section = new Section();
            section.Blocks.Add(new Paragraph(new Run("English meaning")));
            List list = new List();
            ////
            foreach (var item in Regex.Split(enMean, "##"))
            {
                list.ListItems.Add(new ListItem(new Paragraph(new Run(item))));
            }
            ///////
            section.Blocks.Add(list);
            return section;
        }

        #endregion

        #region Build Quick View

        //public static string BuildQuickView(Kanji kanji)
        //{
        //    if (kanji == null)
        //    {
        //        return null;
        //    }
        //    DatabaseManager.LoadFullKanji(kanji);

        //    //start build
        //    StringBuilder doc = new StringBuilder();
        //    doc.Append("<FlowDocument " + xmls + " PagePadding=\"10\">")
        //        .Append(BuildQuickViewHeader(kanji.Word, kanji.HanViet))
        //        .Append(BuildQuickViewAtributeSet(kanji))
        //        .Append(BuildViMean(kanji.ViMean))
        //        .Append(BuildEnMean(kanji.EnMean))
        //        .Append("</FlowDocument>");
        //    return doc.ToString();
        //}

        //private static string BuildQuickViewHeader(string kanji, string hanviet)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("<Paragraph>")
        //        .Append("<Run FontSize=\"20\" Foreground=\"Red\">").Append(kanji).Append("</Run>")
        //        .Append("<Run>").Append("[" + hanviet + "]").Append("</Run>")
        //        .Append("</Paragraph>");
        //    return builder.ToString();
        //}

        //private static string BuildQuickViewAtributeSet(Kanji kanji)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("<Paragraph>");
        //    builder.Append((string) BuildQuickViewAtribute("Bộ thủ", kanji.Radical)).Append("<LineBreak/>");
        //    builder.Append((string) BuildQuickViewAtribute("Onyomi", kanji.Onyomi)).Append("<LineBreak/>");
        //    builder.Append((string) BuildQuickViewAtribute("Kunyomi", kanji.Kunyomi)).Append("<LineBreak/>");
        //    builder.Append((string) BuildQuickViewAtribute("Trình độ", kanji.JLPT)).Append("<LineBreak/>");
        //    builder.Append((string) BuildQuickViewAtribute("Thành phần", kanji.Component));
        //    builder.Append("</Paragraph>");
        //    return builder.ToString();
        //}

        //private static string BuildQuickViewAtribute(string name, string contain)
        //{
        //    StringBuilder builder = new StringBuilder();
        //    builder.Append("<Run Foreground=\"Gray\" FontSize=\"14\" Text=\"").Append(name).Append(": \" />");
        //    builder.Append("<Run>").Append(contain).Append("</Run>");
        //    return builder.ToString();
        //}

        #endregion
    }
}