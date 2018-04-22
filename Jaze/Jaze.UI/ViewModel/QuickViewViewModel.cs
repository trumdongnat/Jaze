﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using Jaze.UI.Models;
using Jaze.UI.Services;
using Jaze.UI.Services.Documents;
using Jaze.UI.Util;
using Prism.Events;

namespace Jaze.UI.ViewModel
{
    public class QuickViewViewModel : ViewModelBase
    {
        #region ----- Services -----

        private readonly IEventAggregator _eventAggregator;
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

        private List<FlowDocument> _itemDocuments = new List<FlowDocument>();

        public List<FlowDocument> ItemDocuments
        {
            get => _itemDocuments;
            set => SetProperty(ref _itemDocuments, value);
        }

        #endregion ----- Item Documents -----

        #region ----- Is Loading -----

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion ----- Is Loading -----

        #region ----- Contructor -----

        public QuickViewViewModel(IEventAggregator eventAggregator, ISearchService<GrammarModel> grammarService, ISearchService<HanVietModel> hanvietService, ISearchService<JaenModel> jaenService, ISearchService<JaviModel> javiService, ISearchService<KanjiModel> kanjiService, ISearchService<VijaModel> vijaService, IBuilder<GrammarModel> grammarBuilder, IBuilder<HanVietModel> hanvietBuilder, IBuilder<JaenModel> jaenBuilder, IBuilder<JaviModel> javiBuilder, IBuilder<KanjiModel> kanjiBuilder, IBuilder<VijaModel> vijaBuilder)
        {
            _eventAggregator = eventAggregator;
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
            _eventAggregator.GetEvent<PubSubEvent<QuickViewMessage>>().Subscribe(ProcessQuickViewMessage);
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
            if (word.StartsWith("•"))
            {
                word = word.Remove(0, 1);
                word = StringUtil.Trim(word);
            }
            var notFoundDocuments = new List<FlowDocument>()
            {
                new FlowDocument(new Paragraph(new Run($"Không tìm thấy: 「{word}」")))
            };

            var dictionaryType = message.DictionaryType;
            ItemDocuments = new List<FlowDocument>();
            switch (dictionaryType)
            {
                case DictionaryType.HanViet:
                    SearchAndView<HanVietModel>(_hanvietService, _hanvietBuilder, word, notFoundDocuments);
                    break;

                case DictionaryType.JaVi:
                case DictionaryType.Kanji:
                case DictionaryType.ViJa:
                case DictionaryType.Grammar:
                    //search in kanji dictionary
                    if (word.Length == 1 && StringUtil.IsKanji(word[0]))
                    {
                        SearchAndView<KanjiModel>(_kanjiService, _kanjiBuilder, word, notFoundDocuments);
                    }
                    //search in javi dictionary
                    else if (StringUtil.IsJapanese(word))
                    {
                        SearchAndView<JaviModel>(_javiService, _javiBuilder, word, notFoundDocuments);
                    }
                    //search in vija dictionary
                    else if (word.Split(' ').All(StringUtil.IsVietnameseWord))
                    {
                        SearchAndView<VijaModel>(_vijaService, _vijaBuilder, word, notFoundDocuments);
                    }
                    else
                    {
                        ItemDocuments = notFoundDocuments;
                        IsLoading = false;
                    }
                    break;

                case DictionaryType.JaEn:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SearchAndView<TModel>(ISearchService<TModel> searcher, IBuilder<TModel> builder, string word, List<FlowDocument> notFoundDocuments) where TModel : new()
        {
            Task.Run(() =>
            {
                var models = searcher.SearchExact(word);
                if (models != null && models.Count > 0)
                {
                    foreach (var model in models)
                    {
                        searcher.LoadFull(model);
                    }
                }
                return models;
            }).ContinueWith(previous =>
            {
                var models = previous.Result;
                var documents = new List<FlowDocument>();
                if (models != null && models.Count > 0)
                {
                    foreach (var model in models)
                    {
                        documents.Add(builder.BuildLite(model));
                    }
                    ItemDocuments = documents;
                }
                else
                {
                    ItemDocuments = notFoundDocuments;
                }
                IsLoading = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        #endregion ----- Process Event Messages -----
    }
}