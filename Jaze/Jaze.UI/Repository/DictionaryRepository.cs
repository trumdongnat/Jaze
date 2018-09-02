using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using Jaze.UI.Definitions;
using Jaze.UI.Models;
using Jaze.UI.Services;
using Jaze.UI.Services.Documents;

namespace Jaze.UI.Repository
{
    public class DictionaryRepository : IDictionaryRepository
    {
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

        public DictionaryRepository(ISearchService<GrammarModel> grammarService, ISearchService<HanVietModel> hanvietService, ISearchService<JaenModel> jaenService, ISearchService<JaviModel> javiService, ISearchService<KanjiModel> kanjiService, ISearchService<VijaModel> vijaService, IBuilder<GrammarModel> grammarBuilder, IBuilder<HanVietModel> hanvietBuilder, IBuilder<JaenModel> jaenBuilder, IBuilder<JaviModel> javiBuilder, IBuilder<KanjiModel> kanjiBuilder, IBuilder<VijaModel> vijaBuilder)
        {
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
        }

        public List<Dictionary> GetDictionarys()
        {
            return Dictionary.GetDictionarys();
        }

        public Task<List<object>> SearchAsync(SearchArgs args)
        {
            return Task.Run(() => Search(args));
        }

        private List<object> Search(SearchArgs args)
        {
            var dictionaryType = args.Dictionary;
            var key = args.SearchKey;
            var searchOption = args.Option;
            var result = new List<object>();
            switch (dictionaryType)
            {
                case DictionaryType.JaVi:
                    result.AddRange(_javiService.Search(new SearchArgs(key, searchOption)));
                    break;

                case DictionaryType.HanViet:
                    result.AddRange(_hanvietService.Search(new SearchArgs(key, searchOption)));
                    break;

                case DictionaryType.Kanji:
                    result.AddRange(_kanjiService.Search(new SearchArgs(key, searchOption)));
                    break;

                case DictionaryType.ViJa:
                    result.AddRange(_vijaService.Search(new SearchArgs(key, searchOption)));
                    break;

                case DictionaryType.Grammar:
                    result.AddRange(_grammarService.Search(new SearchArgs(key, searchOption)));
                    break;

                case DictionaryType.JaEn:
                    result.AddRange(_jaenService.Search(new SearchArgs(key, searchOption)));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(dictionaryType), dictionaryType, null);
            }

            return result;
        }

        public Task LoadFullAsync(object item)
        {
            return Task.Run(() => LoadFull(item));
        }

        private void LoadFull(object item)
        {
            switch (item)
            {
                case KanjiModel kanji:
                    _kanjiService.LoadFull(kanji);
                    break;

                case GrammarModel grammar:
                    _grammarService.LoadFull(grammar);
                    break;

                case HanVietModel hanviet:
                    _hanvietService.LoadFull(hanviet);
                    break;

                case JaenModel jaen:
                    _jaenService.LoadFull(jaen);
                    break;

                case JaviModel javi:
                    _javiService.LoadFull(javi);
                    break;

                case VijaModel vija:
                    _vijaService.LoadFull(vija);
                    break;
            }
        }

        public FlowDocument GetDocument(object item)
        {
            switch (item)
            {
                case KanjiModel kanji:
                    return _kanjiBuilder.Build(kanji);

                case GrammarModel grammar:
                    return _grammarBuilder.Build(grammar);

                case HanVietModel hanviet:
                    return _hanvietBuilder.Build(hanviet);

                case JaenModel jaen:
                    return _jaenBuilder.Build(jaen);

                case JaviModel javi:
                    return _javiBuilder.Build(javi);

                case VijaModel vija:
                    return _vijaBuilder.Build(vija);

                default:
                    return null;
            }
        }

        public FlowDocument GetQuickViewDocument(object item)
        {
            switch (item)
            {
                case KanjiModel kanji:
                    return _kanjiBuilder.BuildLite(kanji);

                case GrammarModel grammar:
                    return _grammarBuilder.BuildLite(grammar);

                case HanVietModel hanviet:
                    return _hanvietBuilder.BuildLite(hanviet);

                case JaenModel jaen:
                    return _jaenBuilder.BuildLite(jaen);

                case JaviModel javi:
                    return _javiBuilder.BuildLite(javi);

                case VijaModel vija:
                    return _vijaBuilder.BuildLite(vija);

                default:
                    return null;
            }
        }

        public DictionaryType GetType(object item)
        {
            switch (item)
            {
                case KanjiModel kanji:
                    return DictionaryType.Kanji;

                case GrammarModel grammar:
                    return DictionaryType.Grammar;

                case HanVietModel hanviet:
                    return DictionaryType.HanViet;

                case JaenModel jaen:
                    return DictionaryType.JaEn;

                case JaviModel javi:
                    return DictionaryType.JaVi;

                case VijaModel vija:
                    return DictionaryType.ViJa;

                default:
                    return DictionaryType.Unknown;
            }
        }
    }
}