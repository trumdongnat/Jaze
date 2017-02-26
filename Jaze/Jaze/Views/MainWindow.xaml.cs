using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Jaze.Control;
using Jaze.DAO;
using Jaze.Document;
using Jaze.Model;
using Jaze.Search;

namespace Jaze.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public bool IsLoading { get; set; }

        //private const double DOCUMENT_LINE_HEIGHT = 30;

        public MainWindow()
        {
            InitializeComponent();
            InitializeControl();
        }

        private void InitializeControl()
        {
            flowDoc.PreviewMouseUp += FlowDocumentOnPreviewMouseUp;
        }

        private void FlowDocumentOnPreviewMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            var s = flowDoc.Selection?.Text;
            MenuPopup.IsOpen = !string.IsNullOrWhiteSpace(s);
        }

        #region Business Logic

        //private void GoPrevious()
        //{
        //    var data = HistoryManager.GetPrevious();
        //    if (data != null)
        //    {
        //        ShowDocument(data);
        //    }
        //}


        private void UpdateStatus(string status)
        {
            textBlockStatus.Text = status;
        }

        private void ShowDocument(object o)
        {
            var document = Builder.Build(o);
            if (document != null)
            {
                flowDoc.Document = document;
                flowDoc.Tag = o;
                if (flowDoc.Selection != null)
                {
                    flowDoc.Selection.Changed += FlowDocumentSelectionOnChanged;
                }
            }
            
        }

        private void QuickView(string text, object o)
        {
            if (o == null || string.IsNullOrWhiteSpace(text))
            {
                QuickViewPopup.IsOpen = false;
                return;
            }

            //remove white
            text = Regex.Replace(text, @"\s+", " ");
            text = text.Replace("•", "");
            text = text.Trim();
           
            //search in kanji
            if (o is Kanji || o is JaVi || o is JaEn || o is ViJa || o is Grammar)
            {
                if (text.Length == 1 && Util.StringUtil.IsKanji(text[0]))
                {
                    var kanjis = Searcher.SearchKanji(new SearchArgs(text, SearchOption.Exact)).ToArray();
                    
                    QuickViewKanji(kanjis.FirstOrDefault());
                }
                else
                {
                    var javis = Searcher.SearchJaVi(new SearchArgs(text, SearchOption.Exact)).ToArray();
                    QuickViewJapanese(javis.FirstOrDefault());
                   
                }
            }
            //search in hanviet
            else if (o is HanViet)
            {
                var hanViets = Searcher.SearchHanViet(new SearchArgs(text, SearchOption.Exact)).ToArray();
                QuickViewHanViet(hanViets.FirstOrDefault());
            }
        }

        private void QuickViewJapanese(JaVi javi)
        {
            ShowQuickView(JaViBuilder.BuildQuickView(javi));
        }

        private void QuickViewHanViet(HanViet hanViet)
        {
            ShowQuickView(HanVietBuilder.BuildQuickView(hanViet));
        }

        private void QuickViewKanji(Kanji kanji)
        {
            ShowQuickView(KanjiBuilder.BuildQuickView(kanji));
        }

        private void ShowQuickView(FlowDocument document)
        {
            QuickViewPopup.Child = new Border()
            {
                Child = new FlowDocumentScrollViewer()
                {
                    Document = document??new FlowDocument(new Paragraph(new Run("not found!")))
                },
                Background = Brushes.White
            };            
            
            QuickViewPopup.IsOpen = true;
        }


        private void Search()
        {
            var arg = searchBar.SearchArgs;
            arg.Dictionary = listDictionary.CurrentDictionary.Type;
            Search(arg);
        }

        private void Search(SearchArgs arg)
        {
            if (listSearchResult.IsSearching)
            {
                MessageBox.Show("Đang tìm kiếm");
                return;
            }

            listSearchResult.IsSearching = true;
            var searchWorker = new BackgroundWorker();
            searchWorker.DoWork += SearchBackground;
            searchWorker.RunWorkerCompleted += SearchBackgroundComplete;
            UpdateStatus("Searching key = " + arg.SearchKey + " Dictionary = " + arg.Dictionary);
            searchWorker.RunWorkerAsync(arg);
        }
        private void CopySelectedText()
        {
            var s = flowDoc.Selection?.Text;
            if (s != null)
            {
                Clipboard.SetText(s);
                UpdateStatus($"Copied \"{s}\" to Clipboard");
            }
        }
        private void CopyShowingModel()
        {
            var o = flowDoc.Tag;
            if (o == null)
            {
                return;
            }

            string s =string.Empty;
            if (o is Grammar)
            {
                s = (o as Grammar).Struct;
            }

            if (o is HanViet)
            {
                s = (o as HanViet).Word;
            }

            if (o is Kanji)
            {
                s = (o as Kanji).Word;
            }

            if (o is JaVi)
            {
                s = (o as JaVi).Word;
            }

            if (o is ViJa)
            {
                s = (o as ViJa).Word;
            }

            if (o is JaEn)
            {
                s = (o as JaEn).Word;
            }

            Clipboard.SetText(s);
            UpdateStatus($"Copied \"{s}\" to Clipboard");
        }

        private void ShowKanjiPart(Kanji kanji)
        {
            UserControl control;
            if (kanji == null)
            {
                control = new KanjiPartControl();
            }
            else
            {
                control = new KanjiPartControl(kanji);
            }

            Window window = new Window
            {
                Content = control,
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ShowInTaskbar = false
            };
            window.ShowDialog();
        }

        private void ShowKanjiPartOf(Kanji kanji)
        {
            if (kanji != null)
            {
                //var window = new KanjiPartOf(kanji) { Owner = this };
                //window.ShowDialog();
               
                var panel = new KanjiPanel();
                panel.ListKanji.ItemsSource =
                    DatabaseContext.Context.Kanjis.Where(k => k.Component.Contains(kanji.Word)).ToList();
                var window = new Window()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Content = panel,
                    Owner = this,
                    WindowStyle = WindowStyle.ToolWindow,
                    ShowInTaskbar = false
                };
                window.ShowDialog();
            }
        }

        #endregion Business Logic

        #region UI event

        private void ButtonHistory_OnClick(object sender, RoutedEventArgs e)
        {
            //if (listBoxHistory.Visibility == Visibility.Visible)
            //{
            //    listBoxHistory.ItemsSource = null;
            //    listBoxHistory.Visibility = Visibility.Collapsed;
            //    listBoxResult.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    listBoxHistory.ItemsSource = HistoryManager.GetAll();
            //    listBoxHistory.Visibility = Visibility.Visible;
            //    listBoxResult.Visibility = Visibility.Collapsed;
            //}
        }

 
        private void FlowDocumentSelectionOnChanged(object sender, EventArgs eventArgs)
        {
            //if (flowDoc.Selection != null) UpdateStatus(flowDoc.Selection.Text);
        }

        private void ButtonCopyShowingModel_OnClick(object sender, RoutedEventArgs e)
        {
            CopyShowingModel();
        }

        private void ButtonPart_OnClick(object sender, RoutedEventArgs e)
        {
            ShowKanjiPart(flowDoc.Tag as Kanji);
        }

        private void ButtonPartOf_OnClick(object sender, RoutedEventArgs e)
        {
            ShowKanjiPartOf(flowDoc.Tag as Kanji);
        }

        private void ListSearchResult_OnShow(object sender, RoutedEventArgs e)
        {
            ShowDocument(e.OriginalSource);
        }

        private void SearchBar_OnSearch(object sender, RoutedEventArgs e)
        {
            var arg = e.OriginalSource as SearchArgs;
            if (arg != null)
            {
                Search();
            }
            else
            {
                MessageBox.Show("Fail");
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void ButtonCopySelectedText_OnClick(object sender, RoutedEventArgs e)
        {
            CopySelectedText();
        }

        private void ButtonQuickView_OnClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("under working");
            MenuPopup.IsOpen = false;
            QuickView(flowDoc.Selection?.Text,flowDoc.Tag);
        }

        private void ButtonSearchSuggest_OnClick(object sender, RoutedEventArgs e)
        {
            var o = flowDoc.Tag;
            if (o is Kanji)
            {
                PopupSearchSuggest.IsOpen = true;
            }
        }

        private void HyperlinkSearchWordStartWith_OnClick(object sender, RoutedEventArgs e)
        {
            PopupSearchSuggest.IsOpen = false;
            var kanji = flowDoc.Tag as Kanji;
            Search(new SearchArgs(kanji.Word,SearchOption.StartWith,DictionaryType.JaVi));   
        }

        private void HyperlinkSearchWordContain_OnClick(object sender, RoutedEventArgs e)
        {
            PopupSearchSuggest.IsOpen = false;
            var kanji = flowDoc.Tag as Kanji;
            Search(new SearchArgs(kanji.Word, SearchOption.Contain, DictionaryType.JaVi));
        }

        private void HyperlinkSearchWordEndWith_OnClick(object sender, RoutedEventArgs e)
        {
            PopupSearchSuggest.IsOpen = false;
            var kanji = flowDoc.Tag as Kanji;
            Search(new SearchArgs(kanji.Word, SearchOption.EndWith, DictionaryType.JaVi));
        }

        #endregion UI event

        #region Search Background

        private void SearchBackground(object sender, DoWorkEventArgs e)
        {
            var arg = e.Argument as SearchArgs;
            var result = Searcher.Search(arg);
            e.Result = result;
        }

        private void SearchBackgroundComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as IEnumerable<object>;
            if (result != null)
            {
                UpdateStatus("Found: " + result.Count() + " result");
            }
            else
            {
                UpdateStatus("Found: 0 result");
            }
            listSearchResult.ItemsSource = result;
            listSearchResult.IsSearching = false;
        }

        #endregion Search Background

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = sender as Hyperlink;
            var run = hyperlink?.Inlines.FirstInline as Run;
            if (run != null)
            {
                var s = run.Text;
                string text = s;
                if (s.StartsWith("["))
                {
                    var match = Regex.Match(s, @"\[.+\]");
                    text = match.Value;
                    text = text.Length > 2 ? text.Substring(1, text.Length - 2) : string.Empty;
                }
                var o = flowDoc.Tag;
                if (o is HanViet && !string.IsNullOrWhiteSpace(text))
                {
                    var hanviet = DatabaseContext.Context.HanViets.FirstOrDefault(hv => hv.Word == text);
                    QuickViewHanViet(hanviet);
                }

            }
        }
    }
}