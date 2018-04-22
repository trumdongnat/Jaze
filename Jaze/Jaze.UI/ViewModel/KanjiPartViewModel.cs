using Jaze.UI.Definitions;
using Jaze.UI.Models;
using Jaze.UI.Services;
using Jaze.UI.Services.Documents;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Linq;
using System.Threading.Tasks;
using Jaze.UI.Messages;
using Prism.Commands;
using Prism.Events;

namespace Jaze.UI.ViewModel
{
    public class KanjiPartViewModel : ViewModelBase
    {
        #region ----- Services -----

        private readonly IEventAggregator _messenger;
        private readonly ISearchService<KanjiModel> _kanjiService;
        private readonly IBuilder<KanjiModel> _kanjiBuilder;
        private readonly IKanjiPartService _kanjiPartService;

        #endregion ----- Services -----

        #region ----- Properties -----

        private DictionaryType _dictionaryType;

        #endregion ----- Properties -----

        #region ----- Parts -----

        private List<SelectablePartModel> _parts = new List<SelectablePartModel>();

        public List<SelectablePartModel> Parts
        {
            get => _parts;
            set => SetProperty(ref _parts, value);
        }

        #endregion ----- Parts -----

        #region ----- Update Filter Command -----

        private DelegateCommand _updateFilterCommand;

        public DelegateCommand UpdateFilterCommand => _updateFilterCommand ?? (_updateFilterCommand = new DelegateCommand(
                                                          ExecuteUpdateFilterCommand,
                                                          CanExecuteUpdateFilterCommand));

        private void ExecuteUpdateFilterCommand()
        {
            var selectedPart = Parts.Where(part => part.IsSelected).ToArray();
            FilteredKanjis = _kanjiPartService.GetListKanji(selectedPart);
            var selectableParts = _kanjiPartService.GetSelectablePart(FilteredKanjis);
            foreach (var part in Parts)
            {
                part.IsEnabled = false;
            }
            foreach (var selectablePart in selectableParts)
            {
                Parts.FirstOrDefault(part => part.Id == selectablePart.Id).IsEnabled = true;
            }
        }

        private bool CanExecuteUpdateFilterCommand()
        {
            return true;
        }

        #endregion ----- Update Filter Command -----

        #region ----- Filtered Kanjis -----

        public const string FilteredKanjisPropertyName = "FilteredKanjis";

        private List<KanjiModel> _filteredKanjis = new List<KanjiModel>();

        public List<KanjiModel> FilteredKanjis
        {
            get => _filteredKanjis;
            set => SetProperty(ref _filteredKanjis, value);
        }

        #endregion ----- Filtered Kanjis -----

        #region ----- Show Kanji Command -----

        private DelegateCommand<KanjiModel> _showKanjiCommand;

        public DelegateCommand<KanjiModel> ShowKanjiCommand => _showKanjiCommand ?? (_showKanjiCommand = new DelegateCommand<KanjiModel>(
                                                                   ExecuteShowKanjiCommand,
                                                                   CanExecuteShowKanjiCommand));

        private void ExecuteShowKanjiCommand(KanjiModel kanji)
        {
            ItemDocument = new FlowDocument();
            IsLoadingDocument = true;
            Task.Run(() =>
            {
                _kanjiService.LoadFull(kanji);
            }).ContinueWith(previous =>
            {
                ItemDocument = _kanjiBuilder.Build(kanji);
                IsLoadingDocument = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private bool CanExecuteShowKanjiCommand(KanjiModel kanji)
        {
            return true;
        }

        #endregion ----- Show Kanji Command -----

        #region ----- Item Document -----

        public const string ItemDocumentPropertyName = "ItemDocument";

        private FlowDocument _itemDocument = null;

        public FlowDocument ItemDocument
        {
            get => _itemDocument;
            set => SetProperty(ref _itemDocument, value);
        }

        #endregion ----- Item Document -----

        #region ----- Is Loading Document-----

        public const string IsLoadingPropertyName = "IsLoadingDocument";

        private bool _isLoadingDocument = false;

        public bool IsLoadingDocument
        {
            get => _isLoadingDocument;
            set => SetProperty(ref _isLoadingDocument, value);
        }

        #endregion ----- Is Loading Document-----

        #region ----- Copy Text -----

        private DelegateCommand _copyKanjiCommand;

        public DelegateCommand CopyKanjiCommand => _copyKanjiCommand
                                                   ?? (_copyKanjiCommand = new DelegateCommand(ExecuteCopyKanjiCommand));

        private void ExecuteCopyKanjiCommand()
        {
            var words = string.Join("", FilteredKanjis.Select(kanji => kanji.Word).ToArray());
            Clipboard.SetText(words);
        }

        #endregion ----- Copy Text -----

        #region ----- Contructor -----

        public KanjiPartViewModel(IEventAggregator messenger, ISearchService<KanjiModel> kanjiService, IBuilder<KanjiModel> kanjiBuilder, IKanjiPartService kanjiPartService)
        {
            _messenger = messenger;
            _kanjiService = kanjiService;
            _kanjiBuilder = kanjiBuilder;
            _kanjiPartService = kanjiPartService;

            Parts = _kanjiPartService.GetListParts()
                .Select(SelectablePartModel.Create)
                .ToList();

            messenger.GetEvent<PubSubEvent<ShowPartsMessage>>().Subscribe(ProcessShowPartsMessage);
        }

        private void ProcessShowPartsMessage(ShowPartsMessage message)
        {
            if (message?.Parts != null)
            {
                Parts.ForEach(part => { part.IsSelected = false; part.IsEnabled = true; });
                foreach (var partStr in message.Parts)
                {
                    var part = Parts.FirstOrDefault(obj => obj.Word == partStr);
                    if (part != null)
                    {
                        part.IsSelected = true;
                    }
                }
                ExecuteUpdateFilterCommand();
            }
        }

        #endregion ----- Contructor -----
    }
}