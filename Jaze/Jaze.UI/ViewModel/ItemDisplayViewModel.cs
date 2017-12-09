using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Documents;
using GalaSoft.MvvmLight.Command;
using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using Jaze.UI.Models;
using Jaze.UI.Services;
using Jaze.UI.Services.Documents;
using System.Windows;
using Jaze.UI.Services.URI;
using System.Linq;

namespace Jaze.UI.ViewModel
{
    public class ItemDisplayViewModel : ViewModelBase
    {
        #region ----- Services -----

        private readonly IMessenger _messenger;
        private readonly ISearchService<GrammarModel> _grammarService;
        private readonly ISearchService<HanVietModel> _hanvietService;
        private readonly ISearchService<JaenModel> _jaenService;
        private readonly ISearchService<JaviModel> _javiService;
        private readonly ISearchService<KanjiModel> _kanjiService;
        private readonly ISearchService<VijaModel> _vijaService;
        private readonly IBuilder<GrammarModel> _grammarBuilder;
        private readonly IBuilder<HanVietModel> _hanvietBuilder;
        private readonly IBuilder<JaenModel> _jaenBuilder;
        private readonly IBuilder<JaviModel> _javiBuilder;
        private readonly IBuilder<KanjiModel> _kanjiBuilder;
        private readonly IBuilder<VijaModel> _vijaBuilder;
        private readonly IUriService _uriService;

        #endregion ----- Services -----

        #region ----- Properties -----

        private DictionaryType _dictionaryType;

        #endregion ----- Properties -----

        #region ----- Item Document -----

        /// <summary>
        /// The <see cref="ItemDocument" /> property's name.
        /// </summary>
        public const string ItemDocumentPropertyName = "ItemDocument";

        private FlowDocument _itemDocument = null;

        /// <summary>
        /// Sets and gets the ItemDocument property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public FlowDocument ItemDocument
        {
            get
            {
                return _itemDocument;
            }
            set
            {
                Set(ItemDocumentPropertyName, ref _itemDocument, value);
            }
        }

        #endregion ----- Item Document -----

        #region ----- Is Loading -----

        /// <summary>
        /// The <see cref="IsLoading" /> property's name.
        /// </summary>
        public const string IsLoadingPropertyName = "IsLoading";

        private bool _isLoading = false;

        /// <summary>
        /// Sets and gets the IsLoading property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                Set(IsLoadingPropertyName, ref _isLoading, value);
            }
        }

        #endregion ----- Is Loading -----

        #region ----- Hyperlink click command -----

        private RelayCommand<Hyperlink> _hyperlinkClickCommand;

        /// <summary>
        /// Gets the HyperlinkClickCommand.
        /// </summary>
        public RelayCommand<Hyperlink> HyperlinkClickCommand
        {
            get
            {
                return _hyperlinkClickCommand ?? (_hyperlinkClickCommand = new RelayCommand<Hyperlink>(ExecuteHyperlinkClickCommand));
            }
        }

        private void ExecuteHyperlinkClickCommand(Hyperlink hyperlink)
        {
            if (hyperlink.NavigateUri == null)
            {
                if (hyperlink?.Inlines.FirstInline is Run run)
                {
                    var s = run.Text;
                    string text = s;
                    if (s.StartsWith("["))
                    {
                        var match = Regex.Match(s, @"\[.+\]");
                        text = match.Value;
                        text = text.Length > 2 ? text.Substring(1, text.Length - 2) : string.Empty;
                    }
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        _messenger.Send(new QuickViewMessage(_dictionaryType, text));
                    }
                }
            }
            else
            {
                var parsedResult = _uriService.Parse(hyperlink.NavigateUri);
                var action = parsedResult.Item1;
                var parameter = parsedResult.Item2;
                switch (action)
                {
                    case UriAction.Unknown:
                        break;

                    case UriAction.QuickView:
                        if (!string.IsNullOrWhiteSpace(parameter))
                        {
                            _messenger.Send(new QuickViewMessage(_dictionaryType, parameter));
                        }
                        break;

                    case UriAction.ShowParts:
                        if (!string.IsNullOrWhiteSpace(parameter))
                        {
                            var parts = parameter.ToCharArray().Select(c => c.ToString()).ToList();
                            _messenger.Send(new ShowPartsMessage(parts));
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        #endregion ----- Hyperlink click command -----

        #region ----- Quick View Command -----

        private RelayCommand<string> _quickViewCommand;

        /// <summary>
        /// Gets the QuickViewCommand.
        /// </summary>
        public RelayCommand<string> QuickViewCommand
        {
            get
            {
                return _quickViewCommand ?? (_quickViewCommand = new RelayCommand<string>(
                    ExecuteQuickViewCommand,
                    CanExecuteQuickViewCommand));
            }
        }

        private void ExecuteQuickViewCommand(string parameter)
        {
            if (!string.IsNullOrWhiteSpace(parameter))
            {
                _messenger.Send(new QuickViewMessage(_dictionaryType, parameter));
            }
        }

        private bool CanExecuteQuickViewCommand(string parameter)
        {
            return true;
        }

        #endregion ----- Quick View Command -----

        #region ----- Copy Text -----

        private RelayCommand<string> _copyTextCommand;

        /// <summary>
        /// Gets the CopyTextCommand.
        /// </summary>
        public RelayCommand<string> CopyTextCommand
        {
            get
            {
                return _copyTextCommand ?? (_copyTextCommand = new RelayCommand<string>(
                    ExecuteCopyTextCommand,
                    CanExecuteCopyTextCommand));
            }
        }

        private void ExecuteCopyTextCommand(string parameter)
        {
            Clipboard.SetText(parameter);
        }

        private bool CanExecuteCopyTextCommand(string parameter)
        {
            return true;
        }

        #endregion ----- Copy Text -----

        #region ----- Contructor -----

        public ItemDisplayViewModel(IMessenger messenger, IUriService uriService, ISearchService<GrammarModel> grammarService, ISearchService<HanVietModel> hanvietService, ISearchService<JaenModel> jaenService, ISearchService<JaviModel> javiService, ISearchService<KanjiModel> kanjiService, ISearchService<VijaModel> vijaService, IBuilder<GrammarModel> grammarBuilder, IBuilder<HanVietModel> hanvietBuilder, IBuilder<JaenModel> jaenBuilder, IBuilder<JaviModel> javiBuilder, IBuilder<KanjiModel> kanjiBuilder, IBuilder<VijaModel> vijaBuilder)
        {
            _messenger = messenger;
            _uriService = uriService;
            _grammarService = grammarService;
            _hanvietService = hanvietService;
            _jaenService = jaenService;
            _javiService = javiService;
            _kanjiService = kanjiService;
            _vijaService = vijaService;
            _grammarBuilder = grammarBuilder;
            _hanvietBuilder = hanvietBuilder;
            _jaenBuilder = jaenBuilder;
            _javiBuilder = javiBuilder;
            _kanjiBuilder = kanjiBuilder;
            _vijaBuilder = vijaBuilder;

            //register message
            _messenger.Register<ShowItemMessage>(this, ProcessShowItemMessage);
        }

        #endregion ----- Contructor -----

        #region ----- Process Event Messages -----

        private void ProcessShowItemMessage(ShowItemMessage message)
        {
            if (message?.Item == null)
            {
                return;
            }
            IsLoading = true;

            switch (message.Item)
            {
                case KanjiModel kanji:
                    _dictionaryType = DictionaryType.Kanji;
                    Task.Run(() =>
                    {
                        _kanjiService.LoadFull(kanji);
                    }).ContinueWith(previous =>
                    {
                        ItemDocument = _kanjiBuilder.Build(kanji);
                        IsLoading = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;

                case GrammarModel grammar:
                    _dictionaryType = DictionaryType.Grammar;
                    Task.Run(() =>
                    {
                        _grammarService.LoadFull(grammar);
                    }).ContinueWith(previous =>
                    {
                        ItemDocument = _grammarBuilder.Build(grammar);
                        IsLoading = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;

                case HanVietModel hanviet:
                    _dictionaryType = DictionaryType.HanViet;
                    Task.Run(() =>
                    {
                        _hanvietService.LoadFull(hanviet);
                    }).ContinueWith(previous =>
                    {
                        ItemDocument = _hanvietBuilder.Build(hanviet);
                        IsLoading = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;

                case JaenModel jaen:
                    _dictionaryType = DictionaryType.JaEn;
                    Task.Run(() =>
                    {
                        _jaenService.LoadFull(jaen);
                    }).ContinueWith(previous =>
                    {
                        ItemDocument = _jaenBuilder.Build(jaen);
                        IsLoading = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;

                case JaviModel javi:
                    _dictionaryType = DictionaryType.JaVi;
                    Task.Run(() =>
                    {
                        _javiService.LoadFull(javi);
                    }).ContinueWith(previous =>
                    {
                        ItemDocument = _javiBuilder.Build(javi);
                        IsLoading = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;

                case VijaModel vija:
                    _dictionaryType = DictionaryType.ViJa;
                    Task.Run(() =>
                    {
                        _vijaService.LoadFull(vija);
                    }).ContinueWith(previous =>
                    {
                        ItemDocument = _vijaBuilder.Build(vija);
                        IsLoading = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;
            }
        }

        #endregion ----- Process Event Messages -----
    }
}