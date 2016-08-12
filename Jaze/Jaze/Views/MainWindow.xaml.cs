using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private Popup _menuPopup;

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
            menuPopup.IsOpen = !string.IsNullOrWhiteSpace(s);
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
                quickViewPopup.IsOpen = false;
                return;
            }

            
            //remove white
            text = Regex.Replace(text, @"\s+", " ");
            text = text.Replace("•", "");
            text = text.Trim();
           
            //search in kanji
            if (o is Kanji || o is JaVi || o is JaEn || o is ViJa || o is Grammar)
            {
                if (text.Length == 1 && Util.ConvertStringUtil.IsKanji(text[0]))
                {
                    var kanjis = Searcher.SearchKanji(new SearchArg(text, SearchOption.Exact)).ToArray();
                    
                    QuickViewKanji(kanjis.FirstOrDefault());
                }
                else
                {
                    var javis = Searcher.SearchJaVi(new SearchArg(text, SearchOption.Exact)).ToArray();
                    QuickViewJapanese(javis.FirstOrDefault());
                   
                }
            }
            //search in hanviet
            else if (o is HanViet)
            {
                var hanViets = Searcher.SearchHanViet(new SearchArg(text, SearchOption.Exact)).ToArray();
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
            quickViewPopup.Child = new Border()
            {
                Child = new FlowDocumentScrollViewer()
                {
                    Document = document??new FlowDocument(new Paragraph(new Run("not found!")))
                },
                Background = Brushes.White
            };            
            
            quickViewPopup.IsOpen = true;
        }


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
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            window.ShowDialog();
        }

        private void ShowKanjiPartOf(Kanji kanji)
        {
            if (kanji != null)
            {
                //var window = new KanjiPartOf(kanji) { Owner = this };
                //window.ShowDialog();
               
                var panel = new Control.KanjiPanel();
                panel.ListKanji.ItemsSource =
                    DatabaseContext.Context.Kanjis.Where(k => k.Component.Contains(kanji.Word)).ToList();
                var window = new Window()
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Content = panel,
                    Owner = this,
                    WindowStyle = WindowStyle.ToolWindow
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
            //try
            //{
            //    var s = flowDoc.Selection.Text;
            //    QuickView(s, flowDoc.Tag);

            //}
            //catch { }
            if (flowDoc.Selection != null) UpdateStatus(flowDoc.Selection.Text);
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

        private void ButtonCopySelectedText_OnClick(object sender, RoutedEventArgs e)
        {
            CopySelectedText();
        }

        private void ButtonQuickView_OnClick(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("under working");
            menuPopup.IsOpen = false;
            QuickView(flowDoc.Selection?.Text,flowDoc.Tag);
        }

        #endregion UI event

        #region Search Background

        private void SearchBackground(object sender, DoWorkEventArgs e)
        {
            var arg = e.Argument as SearchArg;
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
    }
}