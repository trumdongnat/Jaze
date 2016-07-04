using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using Jaze.DAO;
using Jaze.Document;
using Jaze.Search;

namespace Jaze.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public bool IsLoading { get; set; }

        private const double DOCUMENT_LINE_HEIGHT = 30;

        private Popup _quickViewPopup;

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            var context = DatabaseContext.Context;
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

        //private void DictionaryChange()
        //{
        //    if (searchingIndicator.IsBusy)
        //    {
        //        MessageBox.Show("Đang tìm kiếm");
        //    }
        //    searchingIndicator.IsBusy = true;

        //    var searchWorker = new BackgroundWorker();
        //    searchWorker.DoWork += SearchBackground;
        //    searchWorker.RunWorkerCompleted += SearchBackgroundComplete;
        //    var arg = new SearchArg(TextBoxSearch.Text, Dictionary.GetDictionarys()[ListBoxDics.SelectedIndex].Type);
        //    UpdateStatus("Searching key = " + arg.Key + " Dictionary = " + arg.DicType);
        //    searchWorker.RunWorkerAsync(arg);
        //}
        //private void ShowDocument(object o)
        //{
        //    if (o == null)
        //    {
        //        return;
        //    }
        //    if (loadingIndicator.IsBusy)
        //    {
        //        return;
        //    }
        //    loadingIndicator.IsBusy = true;

        //    //
        //    flowDoc.Tag = o;
        //    //
        //    var buildWorker = new BackgroundWorker();
        //    buildWorker.DoWork += BuildDocumentBackground;
        //    buildWorker.RunWorkerCompleted += BuildDocumentBackgroundComplete;
        //    buildWorker.RunWorkerAsync(o);
        //}

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
        //    ShowQuickViewDialog(string.Format("{0}[{1}]", word, kana), Util.Builder.BuildQuickViewJaVocab(word, kana));
        //}

        //private void QuickViewHanViet(string text)
        //{
        //    //if (text.Length > 1)
        //    //{
        //    //    var match = Regex.Match(text, "^\\[(.*)\\]");
        //    //    text = match.Groups[1].Value;
        //    //}
        //    //NewShowQuickViewDialog(null, null);
        //    ShowQuickViewDialog(text, Util.Builder.BuildQuickViewHanViet(text));

        //}

        //private void QuickViewKanji(string text)
        //{
        //    ShowQuickViewDialog(text, Util.Builder.BuildQuickViewKanji(text));
        //}

        //private void ShowQuickViewDialog(string title, string document)
        //{
        //    //var window = new QuickViewWindow();
        //    //window.Owner = this;
        //    //window.SetDocument(title, document);
        //    //window.ShowDialog();
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

        private void ListBoxHistory_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ShowDocument(listBoxHistory.SelectedItem);
        }

        private void ButtonPrevious_OnClick(object sender, RoutedEventArgs e)
        {
            //GoPrevious();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            //DictionaryChange();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    var link = sender as Hyperlink;
            //    string text = (link.Inlines.FirstInline as Run).Text;
            //    QuickView(text, flowDoc.Tag);
            //}
            //catch
            //{
            //}
        }
        private void ListBoxResult_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //HistoryManager.Add(listBoxResult.SelectedItem as DataModel);
            //ShowDocument(listBoxResult.SelectedItem);
        }

        private void SearchBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.Enter)
            //{
            //    DictionaryChange();
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

        #endregion UI event

        #region DictionaryChange Background Worker

        //private void SearchBackgroundComplete(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    var result = e.Result as List<object>;
        //    UpdateStatus("Found: " + result.Count + " result");
        //    listBoxResult.ItemsSource = result;
        //    searchingIndicator.IsBusy = false;
        //}

        //private void SearchBackground(object sender, DoWorkEventArgs e)
        //{
        //    var arg = e.Argument as SearchArg;
        //    var result = SearchManager.DictionaryChange(arg.Key, arg.DicType);
        //    e.Result = result;
        //}

        //#endregion DictionaryChange Background Worker

        //#region Build Document Background Worker

        //private void BuildDocumentBackground(object sender, DoWorkEventArgs e)
        //{
        //    var arg = e.Argument;
        //    e.Result = Util.Builder.Build(arg);
        //}

        //private void BuildDocumentBackgroundComplete(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    var s = e.Result as string;
        //    flowDoc.Document = null;
        //    try
        //    {
        //        flowDoc.Document = XamlReader.Parse(s) as FlowDocument;
        //        flowDoc.Selection.Changed += FlowDocumentSelectionOnChanged;
        //    }
        //    catch (Exception exception)
        //    {
        //        flowDoc.Document = new FlowDocument(new Paragraph(new Run(exception.Message)));
        //    }

        //    loadingIndicator.IsBusy = false;
        //}

        #endregion Build Document Background Worker

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            
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

        private void Search()
        {
            listSearchResult.ItemsSource = Searcher.Search(listDictionary.CurrentDictionary.Type, searchBar.SearchArg);
        }
        private void ListSearchResult_OnShow(object sender, RoutedEventArgs e)
        {
            flowDoc.Document = Builder.Build(e.OriginalSource);
        }
    }
}