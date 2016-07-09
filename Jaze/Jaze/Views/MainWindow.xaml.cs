using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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

        private Popup _quickViewPopup;

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            //var context = DatabaseContext.Context;
            //listSearchResult.ItemsSource = context.ViJas.ToList();
            //flowDoc.Document = Builder.Build(context.JaVis.Find(26462));
        }

        private void UpdateStatus(string status)
        {
            textBlockStatus.Text = status;
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

        

        private void ShowDocument(object o)
        {
            flowDoc.Document = Builder.Build(o);
            flowDoc.Tag = o;
        }

        //private void QuickView(string text, object o)
        //{
        //    //hide current popup
        //    //if (_quickViewPopup != null)
        //    //{
        //    //    _quickViewPopup.IsOpen = false;
        //    //}
        //    try
        //    {
        //        //remove white
        //        text = Regex.Replace(text, @"\s+", "");
        //        text = text.Replace("•", "");
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }
        //    ////////////////////
        //    if (o == null || string.IsNullOrWhiteSpace(text))
        //    {
        //        _quickViewPopup.IsOpen = false;
        //        return;
        //    }


        //    //search in kanji
        //    if (o is Kanji || o is JaVocab || o is ViVocab || o is Grammar)
        //    {
        //        if (text.Length == 1 && Util.ConvertStringUtil.IsKanji(text[0]))
        //        {
        //            QuickViewKanji(text);
        //        }
        //        else
        //        {
        //            if (o is Kanji)
        //            {
        //                //remove unnecessary symbol
        //                string word, kana;
        //                string kanji = (o as Kanji).Word;
        //                int pos = text.IndexOf('.');
        //                if (pos < 0)
        //                {
        //                    word = kanji;
        //                    kana = text;
        //                }
        //                else
        //                {
        //                    word = kanji + text.Substring(pos + 1);
        //                    kana = text.Replace(".", "");
        //                }
        //                QuickViewJapanese(word, kana);
        //            }
        //            else
        //            {
        //                var listJaVocab = DatabaseManager.SearchExactJaVocab(text, 1);
        //                //tam thoi show 1 gia tri
        //                if (listJaVocab.Count == 1)
        //                {
        //                    var jaVocab = listJaVocab[0];
        //                    QuickViewJapanese(jaVocab.Word, jaVocab.Kana);
        //                }
        //                else
        //                {
        //                    QuickViewJapanese(text, text);
        //                }
        //            }
        //        }
        //    }
        //    //search in hanviet
        //    else if (o is HanViet)
        //    {
        //        QuickViewHanViet(text);
        //    }
        //}

        //private void QuickViewJapanese(string word, string kana)
        //{
        //    ShowQuickView(string.Format("{0}[{1}]", word, kana), Util.Builder.BuildQuickViewJaVocab(word, kana));
        //}

        //private void QuickViewHanViet(string text)
        //{
        //    //if (text.Length > 1)
        //    //{
        //    //    var match = Regex.Match(text, "^\\[(.*)\\]");
        //    //    text = match.Groups[1].Value;
        //    //}
        //    //NewShowQuickViewDialog(null, null);
        //    ShowQuickView(text, Util.Builder.BuildQuickViewHanViet(text));

        //}

        //private void QuickViewKanji(string text)
        //{
        //    ShowQuickView(text, Util.Builder.BuildQuickViewKanji(text));
        //}

        //private void ShowQuickView(string title, string document)
        //{
        //    if (_quickViewPopup == null)
        //    {
        //        _quickViewPopup = new Popup()
        //        {
        //            Height = Double.NaN,
        //            Width = Double.NaN,
        //            Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint,
        //            StaysOpen = false,
        //            AllowsTransparency = true,
        //            VerticalOffset = 20,
        //            HorizontalOffset = 20
        //        };
        //    }

        //    var content = new QuickViewControl();
        //    content.SetDocument(title, document);
        //    _quickViewPopup.Child = content;
        //    _quickViewPopup.IsOpen = true;
        //}


        private void Search()
        {
            //listSearchResult.ItemsSource = Searcher.Search(listDictionary.CurrentDictionary.Type, searchBar.SearchArg);
            if (listSearchResult.IsSearching)
            {
                MessageBox.Show("Đang tìm kiếm");
            }
            listSearchResult.IsSearching = true;

            var searchWorker = new BackgroundWorker();
            searchWorker.DoWork += SearchBackground;
            searchWorker.RunWorkerCompleted += SearchBackgroundComplete;
            var arg = searchBar.SearchArg;
            arg.Dictionary = listDictionary.CurrentDictionary.Type;
            UpdateStatus("Searching key = " + arg.SearchKey + " Dictionary = " + arg.Dictionary);
            searchWorker.RunWorkerAsync(arg);
        }

        private void Copy2Clipboard()
        {
            //MessageBox.Show("Under working");
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
            MessageBox.Show("Under working");
        }

        private void ShowKanjiPartOf(Kanji kanji)
        {
            MessageBox.Show("Under working");
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
            //try
            //{
            //    var s = flowDoc.Selection.Text;
            //    QuickView(s, flowDoc.Tag);

            //}
            //catch { }

        }

        private void ButtonCopy_OnClick(object sender, RoutedEventArgs e)
        {
            Copy2Clipboard();
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
            var arg = e.OriginalSource as SearchArg;
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

        #endregion UI event

        #region Search Background

        private void SearchBackgroundComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as IEnumerable<object>;
            if (result != null)
            {
                UpdateStatus("Found: " + result.LongCount() + " result");
            }
            else
            {
                UpdateStatus("Found: 0 result");
            }
            listSearchResult.ItemsSource = result;
            listSearchResult.IsSearching = false;
        }

        private void SearchBackground(object sender, DoWorkEventArgs e)
        {
            var arg = e.Argument as SearchArg;
            var result = Searcher.Search(arg);
            e.Result = result;
        }

        #endregion Search Background
    }
}