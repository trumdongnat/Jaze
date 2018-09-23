using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using Jaze.Domain.Definitions;
using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using Jaze.UI.Models;
using Jaze.UI.Repository;
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

        private readonly IDictionaryRepository _dictionaryRepository;

        #endregion ----- Services -----

        #region ----- Properties -----

        private List<FlowDocument> _itemDocuments = new List<FlowDocument>();

        public List<FlowDocument> ItemDocuments
        {
            get => _itemDocuments;
            set => SetProperty(ref _itemDocuments, value);
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion ----- Properties -----

        #region ----- Contructor -----

        public QuickViewViewModel(IEventAggregator eventAggregator, IDictionaryRepository dictionaryRepository)
        {
            _eventAggregator = eventAggregator;
            _dictionaryRepository = dictionaryRepository;

            //register message
            _eventAggregator.GetEvent<PubSubEvent<QuickViewMessage>>().Subscribe(ProcessQuickViewMessage);
        }

        #endregion ----- Contructor -----

        #region ----- Process Event Messages -----

        private async void ProcessQuickViewMessage(QuickViewMessage message)
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
                    ItemDocuments = await GetDocumentsAsync(DictionaryType.HanViet, word);
                    break;

                case DictionaryType.JaVi:
                case DictionaryType.Kanji:
                case DictionaryType.ViJa:
                case DictionaryType.Grammar:
                    //search in kanji dictionary
                    if (word.Length == 1 && StringUtil.IsKanji(word[0]))
                    {
                        ItemDocuments = await GetDocumentsAsync(DictionaryType.Kanji, word);
                    }
                    //search in javi dictionary
                    else if (StringUtil.IsJapanese(word))
                    {
                        ItemDocuments = await GetDocumentsAsync(DictionaryType.JaVi, word);
                    }
                    //search in vija dictionary
                    else if (word.Split(' ').All(StringUtil.IsVietnameseWord))
                    {
                        var documents = await GetDocumentsAsync(DictionaryType.Kanji, word);
                        documents.AddRange(await GetDocumentsAsync(DictionaryType.ViJa, word));
                        ItemDocuments = documents;
                    }
                    break;

                case DictionaryType.JaEn:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (ItemDocuments == null || ItemDocuments.Count == 0)
            {
                ItemDocuments = notFoundDocuments;
            }
            IsLoading = false;
        }

        private async Task<List<FlowDocument>> GetDocumentsAsync(DictionaryType dictionaryType, string word)
        {
            var items = await _dictionaryRepository.SearchAsync(new SearchArgs(word, SearchOption.Exact, dictionaryType));
            var loadFullTasks = items.Select(item => _dictionaryRepository.LoadFullAsync(item));
            await Task.WhenAll(loadFullTasks);
            return items.Select(item => _dictionaryRepository.GetQuickViewDocument(item)).ToList();
        }

        #endregion ----- Process Event Messages -----
    }
}