﻿using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Jaze.UI.Models;

namespace Jaze.UI.Services.Documents
{
    public class GrammarBuilder : BuilderBase<GrammarModel>
    {
        private Block BuildGrammarDetail(GrammarDetail detail)
        {
            Section section = new Section();
            //Grammar Synopsis
            section.Blocks.Add(new Paragraph()
            {
                Inlines =
                {
                    new Run(detail.Synopsis)
                    {
                        Foreground = Brushes.Red,
                        FontSize = 20,
                    }
                }
            });

            //Grammar Attribute
            section.Blocks.Add(BuildAttribute("Ý nghĩa: ", detail.Mean));
            section.Blocks.Add(BuildAttribute("Giải thích: ", detail.Explain));
            section.Blocks.Add(new Paragraph()
            {
                Foreground = Brushes.Gray,
                FontSize = 14,
                Inlines = { new Run("Ví dụ:") }
            });

            //Grammar ExampleIds
            var examplesBlock = BuildJaviExamples(detail.Examples);
            examplesBlock.Margin = new Thickness(15, 10, 0, 0);
            section.Blocks.Add(examplesBlock);
            section.Blocks.Add(BuildAttribute("Ghi chú: ", detail.Note));
            return section;
        }

        private Block BuildAttribute(string name, string contain)
        {
            Paragraph paragraph = new Paragraph();

            paragraph.Inlines.Add(new Run(name)
            {
                Foreground = Brushes.Gray,
                FontSize = 14
            });
            paragraph.Inlines.Add(new Run(contain));

            return paragraph;
        }

        public override FlowDocument Build(GrammarModel grammar)
        {
            if (grammar == null)
            {
                return null;
            }
            //-------------start build ---------------------------

            FlowDocument document = new FlowDocument()
            {
                PagePadding = new Thickness(10)
            };

            foreach (var item in grammar.Detail)
            {
                document.Blocks.Add(BuildGrammarDetail(item));
            }

            return document;
        }

        public override FlowDocument BuildLite(GrammarModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}