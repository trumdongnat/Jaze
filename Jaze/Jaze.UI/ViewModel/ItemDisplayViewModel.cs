using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Documents;
using Jaze.UI.Messages;
using Jaze.UI.Models;
using Jaze.UI.Services;
using Jaze.UI.Services.Documents;

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

        #endregion ----- Services -----

        #region ----- Properties -----

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

        #region ----- Contructor -----

        public ItemDisplayViewModel(IMessenger messenger, ISearchService<GrammarModel> grammarService, ISearchService<HanVietModel> hanvietService, ISearchService<JaenModel> jaenService, ISearchService<JaviModel> javiService, ISearchService<KanjiModel> kanjiService, ISearchService<VijaModel> vijaService, IBuilder<GrammarModel> grammarBuilder, IBuilder<HanVietModel> hanvietBuilder, IBuilder<JaenModel> jaenBuilder, IBuilder<JaviModel> javiBuilder, IBuilder<KanjiModel> kanjiBuilder, IBuilder<VijaModel> vijaBuilder)
        {
            _messenger = messenger;
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