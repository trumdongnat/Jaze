using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using Jaze.UI.Models;
using Jaze.UI.Services;
using Jaze.UI.Services.Documents;
using Jaze.UI.Util;

namespace Jaze.UI.ViewModel
{
    public class QuickViewViewModel : ViewModelBase
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

        #region ----- Item Documents -----

        /// <summary>
        /// The <see cref="ItemDocuments" /> property's name.
        /// </summary>
        public const string ItemDocumentsPropertyName = "ItemDocuments";

        private List<FlowDocument> _itemDocuments = new List<FlowDocument>();

        /// <summary>
        /// Sets and gets the ItemDocuments property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public List<FlowDocument> ItemDocuments
        {
            get
            {
                return _itemDocuments;
            }
            set
            {
                Set(ItemDocumentsPropertyName, ref _itemDocuments, value);
            }
        }

        #endregion ----- Item Documents -----

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

        public QuickViewViewModel(IMessenger messenger, ISearchService<GrammarModel> grammarService, ISearchService<HanVietModel> hanvietService, ISearchService<JaenModel> jaenService, ISearchService<JaviModel> javiService, ISearchService<KanjiModel> kanjiService, ISearchService<VijaModel> vijaService, IBuilder<GrammarModel> grammarBuilder, IBuilder<HanVietModel> hanvietBuilder, IBuilder<JaenModel> jaenBuilder, IBuilder<JaviModel> javiBuilder, IBuilder<KanjiModel> kanjiBuilder, IBuilder<VijaModel> vijaBuilder)
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
            _messenger.Register<QuickViewMessage>(this, ProcessQuickViewMessage);
        }

        #endregion ----- Contructor -----

        #region ----- Process Event Messages -----

        private void ProcessQuickViewMessage(QuickViewMessage message)
        {
            if (string.IsNullOrWhiteSpace(message.Word))
            {
                return;
            }
            IsLoading = true;
            var word = StringUtil.Trim(message.Word);
            var dictionaryType = message.DictionaryType;
            ItemDocuments = new List<FlowDocument>();
            switch (dictionaryType)
            {
                case DictionaryType.HanViet:
                    Task.Run(() =>
                    {
                        var hanviets = _hanvietService.SearchExact(word);
                        if (hanviets != null && hanviets.Count > 0)
                        {
                            foreach (var hanviet in hanviets)
                            {
                                _hanvietService.LoadFull(hanviet);
                            }
                        }
                        return hanviets;
                    }).ContinueWith(previous =>
                    {
                        var hanviets = previous.Result;
                        var documents = new List<FlowDocument>();
                        if (hanviets != null && hanviets.Count > 0)
                        {
                            foreach (var hanviet in hanviets)
                            {
                                documents.Add(_hanvietBuilder.BuildLite(hanviet));
                            }
                        }
                        ItemDocuments = documents;
                        IsLoading = false;
                    }, TaskScheduler.FromCurrentSynchronizationContext());
                    break;

                case DictionaryType.JaVi:
                case DictionaryType.Kanji:
                case DictionaryType.ViJa:
                case DictionaryType.Grammar:
                    //search in kanji dictionary
                    if (word.Length == 1 && StringUtil.IsKanji(word[0]))
                    {
                    }
                    //search in javi dictionary
                    else if (StringUtil.IsJapanese(word))
                    {
                    }
                    //search in vija dictionary
                    if (word.Split(' ').All(StringUtil.IsVietnameseWord))
                    {
                    }
                    break;

                case DictionaryType.JaEn:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            //switch (message.Item)
            //{
            //    case KanjiModel kanji:
            //        dictionaryType = DictionaryType.Kanji;
            //        Task.Run(() =>
            //        {
            //            _kanjiService.LoadFull(kanji);
            //        }).ContinueWith(previous =>
            //        {
            //            ItemDocument = _kanjiBuilder.Build(kanji);
            //            IsLoading = false;
            //        }, TaskScheduler.FromCurrentSynchronizationContext());
            //        break;

            //    case GrammarModel grammar:
            //        dictionaryType = DictionaryType.Grammar;
            //        Task.Run(() =>
            //        {
            //            _grammarService.LoadFull(grammar);
            //        }).ContinueWith(previous =>
            //        {
            //            ItemDocument = _grammarBuilder.Build(grammar);
            //            IsLoading = false;
            //        }, TaskScheduler.FromCurrentSynchronizationContext());
            //        break;

            //    case HanVietModel hanviet:
            //        dictionaryType = DictionaryType.HanViet;
            //        Task.Run(() =>
            //        {
            //            _hanvietService.LoadFull(hanviet);
            //        }).ContinueWith(previous =>
            //        {
            //            ItemDocument = _hanvietBuilder.Build(hanviet);
            //            IsLoading = false;
            //        }, TaskScheduler.FromCurrentSynchronizationContext());
            //        break;

            //    case JaenModel jaen:
            //        dictionaryType = DictionaryType.JaEn;
            //        Task.Run(() =>
            //        {
            //            _jaenService.LoadFull(jaen);
            //        }).ContinueWith(previous =>
            //        {
            //            ItemDocument = _jaenBuilder.Build(jaen);
            //            IsLoading = false;
            //        }, TaskScheduler.FromCurrentSynchronizationContext());
            //        break;

            //    case JaviModel javi:
            //        dictionaryType = DictionaryType.JaVi;
            //        Task.Run(() =>
            //        {
            //            _javiService.LoadFull(javi);
            //        }).ContinueWith(previous =>
            //        {
            //            ItemDocument = _javiBuilder.Build(javi);
            //            IsLoading = false;
            //        }, TaskScheduler.FromCurrentSynchronizationContext());
            //        break;

            //    case VijaModel vija:
            //        dictionaryType = DictionaryType.ViJa;
            //        Task.Run(() =>
            //        {
            //            _vijaService.LoadFull(vija);
            //        }).ContinueWith(previous =>
            //        {
            //            ItemDocument = _vijaBuilder.Build(vija);
            //            IsLoading = false;
            //        }, TaskScheduler.FromCurrentSynchronizationContext());
            //        break;
            //}
        }

        #endregion ----- Process Event Messages -----
    }
}