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

            if (message.Item is KanjiModel kanji)
            {
                _kanjiService.LoadFull(kanji);
                ItemDocument = _kanjiBuilder.Build(kanji);
            }

            if (message.Item is GrammarModel grammar)
            {
                _grammarService.LoadFull(grammar);
                ItemDocument = _grammarBuilder.Build(grammar);
            }

            if (message.Item is HanVietModel hanviet)
            {
                _hanvietService.LoadFull(hanviet);
                ItemDocument = _hanvietBuilder.Build(hanviet);
            }

            if (message.Item is JaenModel jaen)
            {
                _jaenService.LoadFull(jaen);
                ItemDocument = _jaenBuilder.Build(jaen);
            }

            if (message.Item is JaviModel javi)
            {
                _javiService.LoadFull(javi);
                ItemDocument = _javiBuilder.Build(javi);
            }

            if (message.Item is VijaModel vija)
            {
                _vijaService.LoadFull(vija);
                ItemDocument = _vijaBuilder.Build(vija);
            }
        }

        #endregion ----- Process Event Messages -----
    }
}