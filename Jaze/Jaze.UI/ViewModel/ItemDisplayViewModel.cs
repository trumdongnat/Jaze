using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using Jaze.UI.Models;
using Jaze.UI.Services;
using Jaze.UI.Services.Documents;
using System.Windows;
using Jaze.UI.Services.URI;
using System.Linq;
using Prism.Commands;
using Prism.Events;

namespace Jaze.UI.ViewModel
{
    public class ItemDisplayViewModel : ViewModelBase
    {
        #region ----- Services -----

        private readonly IEventAggregator _messenger;
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

        private FlowDocument _itemDocument = null;

        public FlowDocument ItemDocument
        {
            get => _itemDocument;
            set => SetProperty(ref _itemDocument, value);
        }

        #endregion ----- Item Document -----

        #region ----- Is Loading -----

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion ----- Is Loading -----

        #region ----- Hyperlink click command -----

        private DelegateCommand<Hyperlink> _hyperlinkClickCommand;

        public DelegateCommand<Hyperlink> HyperlinkClickCommand => _hyperlinkClickCommand ?? (_hyperlinkClickCommand = new DelegateCommand<Hyperlink>(ExecuteHyperlinkClickCommand));

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
                        _messenger.GetEvent<PubSubEvent<QuickViewMessage>>().Publish(new QuickViewMessage(_dictionaryType, text));
                    }
                }
            }
            else
            {
                var (action, parameter) = _uriService.Parse(hyperlink.NavigateUri);
                switch (action)
                {
                    case UriAction.Unknown:
                        break;

                    case UriAction.QuickView:
                        if (!string.IsNullOrWhiteSpace(parameter))
                        {
                            _messenger.GetEvent<PubSubEvent<QuickViewMessage>>().Publish(new QuickViewMessage(_dictionaryType, parameter));
                        }
                        break;

                    case UriAction.ShowParts:
                        if (!string.IsNullOrWhiteSpace(parameter))
                        {
                            var parts = parameter.ToCharArray().Select(c => c.ToString()).ToList();
                            _messenger.GetEvent<PubSubEvent<ShowPartsMessage>>().Publish(new ShowPartsMessage(parts));
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        #endregion ----- Hyperlink click command -----

        #region ----- Quick View Command -----

        private DelegateCommand<string> _quickViewCommand;

        public DelegateCommand<string> QuickViewCommand => _quickViewCommand ?? (_quickViewCommand = new DelegateCommand<string>(
                                                               ExecuteQuickViewCommand,
                                                               CanExecuteQuickViewCommand));

        private void ExecuteQuickViewCommand(string parameter)
        {
            if (!string.IsNullOrWhiteSpace(parameter))
            {
                _messenger.GetEvent<PubSubEvent<QuickViewMessage>>().Publish(new QuickViewMessage(_dictionaryType, parameter));
            }
        }

        private bool CanExecuteQuickViewCommand(string parameter)
        {
            return true;
        }

        #endregion ----- Quick View Command -----

        #region ----- Copy Text -----

        private DelegateCommand<string> _copyTextCommand;

        public DelegateCommand<string> CopyTextCommand => _copyTextCommand ?? (_copyTextCommand = new DelegateCommand<string>(
                                                              ExecuteCopyTextCommand,
                                                              CanExecuteCopyTextCommand));

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

        public ItemDisplayViewModel(IEventAggregator messenger, IUriService uriService, ISearchService<GrammarModel> grammarService, ISearchService<HanVietModel> hanvietService, ISearchService<JaenModel> jaenService, ISearchService<JaviModel> javiService, ISearchService<KanjiModel> kanjiService, ISearchService<VijaModel> vijaService, IBuilder<GrammarModel> grammarBuilder, IBuilder<HanVietModel> hanvietBuilder, IBuilder<JaenModel> jaenBuilder, IBuilder<JaviModel> javiBuilder, IBuilder<KanjiModel> kanjiBuilder, IBuilder<VijaModel> vijaBuilder)
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
            _messenger.GetEvent<PubSubEvent<ShowItemMessage>>().Subscribe(ProcessShowItemMessage);
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