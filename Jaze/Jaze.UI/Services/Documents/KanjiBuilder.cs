using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.UI.Models;
using Jaze.UI.Services.URI;
using Jaze.UI.Util;

namespace Jaze.UI.Services.Documents
{
    public class KanjiBuilder : BuilderBase<KanjiModel>
    {
        private IUriService _uriService;

        public KanjiBuilder(IUriService uriService)
        {
            _uriService = uriService;
        }

        #region ----- Build Full Infomation -----

        public override FlowDocument Build(KanjiModel kanji)
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
            document.Blocks.Add(BuildGeneralInfo(kanji));
            document.Blocks.Add(BuildWords(kanji.JaviModels));
            return document;
        }

        private Block BuildGeneralInfo(KanjiModel kanji)
        {
            Table table = new Table() { CellSpacing = 0 };

            table.RowGroups.Add(new TableRowGroup());
            table.RowGroups[0].Rows.Add(new TableRow()
            {
                Cells =
                {
                    new TableCell(BuildAttributeSet(kanji)),
                    new TableCell(BuildKanjiStroke(kanji.Word, kanji.HanViet))
                }
            });
            //table.RowGroups[0].Rows.Add(new TableRow()
            //{
            //    Cells =
            //    {
            //        new TableCell(new Paragraph()
            //        {
            //            Inlines =
            //            {
            //                new Hyperlink(new Run($"Từ bắt đầu bằng {kanji.Word}"))
            //                {
            //                    NavigateUri = _uriService.Create(Definitions.UriAction.WordStartWith, kanji.Word)
            //                },
            //                new Hyperlink(new Run($"Từ chứa {kanji.Word}"))
            //                {
            //                    NavigateUri = _uriService.Create(Definitions.UriAction.WordContain, kanji.Word)
            //                },
            //                new Hyperlink(new Run($"Từ kết thúc bằng {kanji.Word}"))
            //                {
            //                    NavigateUri = _uriService.Create(Definitions.UriAction.WordEndWith, kanji.Word)
            //                },
            //            }
            //        })
            //    }
            //});
            table.RowGroups[0].Rows.Add(new TableRow()
            {
                Cells =
                {
                    new TableCell(BuildViMean(kanji.VieMeaning)),
                    new TableCell(BuildEnMean(kanji.EngMeaning))
                }
            });
            return table;
        }

        private Block BuildWords(List<JaviModel> javiModels)
        {
            if (javiModels == null || javiModels.Count == 0)
            {
                return new Section();
            }
            Table table = new Table() { CellSpacing = 0 };
            table.Columns.Add(new TableColumn { Width = new GridLength(80) });
            table.Columns.Add(new TableColumn { Width = new GridLength(100) });
            table.Columns.Add(new TableColumn { Width = GridLength.Auto });
            table.RowGroups.Add(new TableRowGroup());

            foreach (var javiModel in javiModels)
            {
                table.RowGroups[0].Rows.Add(new TableRow()
                {
                    Cells =
                    {
                        new TableCell(new Paragraph(new Run(javiModel.Word))),
                        new TableCell(new Paragraph(new Run(javiModel.Kana))),
                        new TableCell(BuildJaViMean(javiModel.Means))
                    }
                });
            }
            foreach (var row in table.RowGroups[0].Rows)
            {
                foreach (var cell in row.Cells)
                {
                    cell.BorderThickness = new Thickness(1);
                    cell.BorderBrush = Brushes.Black;
                    cell.Padding = new Thickness(1.5);
                }
            }
            return table;
        }

        private Block BuildJaViMean(List<WordMean> means)
        {
            if (means == null || means.Count == 0)
            {
                return new Section();
            }
            var paragraph = new Paragraph();
            foreach (var mean in means)
            {
                paragraph.Inlines.Add(new Run("- " + mean.Mean));
                paragraph.Inlines.Add(new LineBreak());
            }
            return paragraph;
        }

        private Block BuildAttributeSet(KanjiModel kanji)
        {
            List list = new List()
            {
                MarkerStyle = TextMarkerStyle.Box
            };

            //add list attribute
            list.ListItems.Add(BuildAttribute("Bộ thủ: ", $"{kanji.Radical.Word}({kanji.Radical.HanViet})"));
            list.ListItems.Add(BuildAttributeComponents("Cách viết khác: ", kanji.Variant));
            list.ListItems.Add(BuildAttribute("Onyomi: ", kanji.Onyomi));
            list.ListItems.Add(BuildAttributeKunyomi("Kunyomi: ", kanji.Word, kanji.Kunyomi));
            list.ListItems.Add(BuildAttribute("Số nét: ", "" + kanji.Stroke));
            list.ListItems.Add(BuildAttribute("Độ phổ biến: ",
                kanji.Frequence == int.MaxValue ? "?/2500" : "" + kanji.Frequence + "/2500"));
            list.ListItems.Add(BuildAttribute("Trình độ: ", kanji.Level.ToString()));
            list.ListItems.Add(BuildAttributeComponents("Thành phần: ", kanji.Component));
            list.ListItems.Add(BuildAttributeComponents("Gần giống: ", kanji.Similar));
            list.ListItems.Add(BuildAttributeParts("Cấu tạo: ", kanji.Parts));
            return list;
        }

        private ListItem BuildAttribute(string name, string contain)
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

        private ListItem BuildAttributeParts(string name, List<PartModel> parts)
        {
            ListItem item = new ListItem();
            var partsStr = string.Join("", parts.Select(part => part.Word).ToArray());
            item.Blocks.Add(new Paragraph()
            {
                Inlines =
                {
                    new Run(name)
                    {
                        Foreground = Brushes.Gray,
                        FontSize = 14
                    },
                    new Hyperlink(new Run(partsStr))
                    {
                        NavigateUri = _uriService.Create(Definitions.UriAction.ShowParts, partsStr)
                    }
                }
            });

            return item;
        }

        private ListItem BuildAttributeKunyomi(string name, string kanji, string contain)
        {
            ListItem item = new ListItem();
            var paragragh = new Paragraph()
            {
                Inlines =
                {
                    new Run(name)
                    {
                        Foreground = Brushes.Gray,
                        FontSize = 14
                    }
                }
            };

            //split contain
            if (!string.IsNullOrWhiteSpace(contain))
            {
                var kuns = Regex.Split(contain, "、 ");
                foreach (var s in kuns)
                {
                    if (s.Contains('-'))
                    {
                        paragragh.Inlines.Add(new Run(s + "、 "));
                        continue;
                    }
                    var arr = s.Split('.');
                    if (arr.Length == 2)
                    {
                        paragragh.Inlines.Add(new Hyperlink(new Run(s))
                        {
                            NavigateUri = _uriService.Create(Definitions.UriAction.QuickView, kanji + arr[1])
                        });
                    }
                    else
                    {
                        paragragh.Inlines.Add(new Hyperlink(new Run(s))
                        {
                            NavigateUri = _uriService.Create(Definitions.UriAction.QuickView, kanji)
                        });
                    }
                    paragragh.Inlines.Add(new Run("、 ")
                    {
                        FontSize = 15
                    });
                }
            }

            item.Blocks.Add(paragragh);
            return item;
        }

        private ListItem BuildAttributeComponents(string name, string content)
        {
            ListItem item = new ListItem();
            var kanjis = StringUtil.FilterCharsInString(content, CharSet.Kanji).Select(c => c.ToString()).ToArray();
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(new Run(name)
            {
                Foreground = Brushes.Gray,
                FontSize = 14
            });
            foreach (var kanji in kanjis)
            {
                paragraph.Inlines.Add(new Hyperlink(new Run(kanji))
                {
                    NavigateUri = _uriService.Create(Definitions.UriAction.QuickView, kanji)
                });
            }
            item.Blocks.Add(paragraph);
            return item;
        }

        private Block BuildKanjiStroke(string kanji, string hanviet)
        {
            Paragraph paragraph = new Paragraph()
            {
                TextAlignment = TextAlignment.Center,
                Inlines = {
                    new Run(kanji)
                    {
                        FontSize = 150,
                        FontFamily = new FontFamily("KanjiStrokeOrders")
                    } ,
                    new LineBreak(),
                    new Run(hanviet),
                }
            };
            return paragraph;
        }

        private Block BuildViMean(string viMean)
        {
            if (string.IsNullOrWhiteSpace(viMean))
            {
                return new Section();
            }
            Section section = new Section();
            //title
            section.Blocks.Add(new Paragraph(new Run("Nghĩa tiếng Việt")));
            section.Blocks.Add(BuildListViMean(viMean));
            return section;
        }

        public static Block BuildListViMean(string viMean)
        {
            if (string.IsNullOrWhiteSpace(viMean))
            {
                return new Section();
            }
            List list = new List()
            {
                MarkerStyle = TextMarkerStyle.Square,
                MarkerOffset = 5,
                Margin = new Thickness(0),
                Padding = new Thickness(20, 0, 0, 0)
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
                        Blocks = { new Paragraph(new Run(mean)) }
                    });
                }
                item.Blocks.Add(subList);
                list.ListItems.Add(item);
            }
            return list;
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

        private Block BuildEnMean(string enMean)
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

        #endregion ----- Build Full Infomation -----

        #region Build Quick View

        public override FlowDocument BuildLite(KanjiModel model)
        {
            return Build(model);
        }

        #endregion Build Quick View
    }
}