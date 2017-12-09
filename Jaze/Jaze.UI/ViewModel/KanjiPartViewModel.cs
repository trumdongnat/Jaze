using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
using System;

namespace Jaze.UI.ViewModel
{
    public class KanjiPartViewModel : ViewModelBase
    {
        #region ----- Services -----

        private readonly IMessenger _messenger;
        private readonly ISearchService<KanjiModel> _kanjiService;
        private readonly IBuilder<KanjiModel> _kanjiBuilder;
        private readonly IKanjiPartService _kanjiPartService;

        #endregion ----- Services -----

        #region ----- Properties -----

        private DictionaryType _dictionaryType;

        #endregion ----- Properties -----

        #region ----- Parts -----

        /// <summary>
        /// The <see cref="Parts" /> property's name.
        /// </summary>
        public const string PartsPropertyName = "Parts";

        private List<SelectablePartModel> _parts = new List<SelectablePartModel>();

        /// <summary>
        /// Sets and gets the Parts property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public List<SelectablePartModel> Parts
        {
            get
            {
                return _parts;
            }
            set
            {
                Set(PartsPropertyName, ref _parts, value);
            }
        }

        #endregion ----- Parts -----

        #region ----- Update Filter Command -----

        private RelayCommand _updateFilterCommand;

        /// <summary>
        /// Gets the UpdateFilterCommand.
        /// </summary>
        public RelayCommand UpdateFilterCommand
        {
            get
            {
                return _updateFilterCommand ?? (_updateFilterCommand = new RelayCommand(
                    ExecuteUpdateFilterCommand,
                    CanExecuteUpdateFilterCommand));
            }
        }

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

        /// <summary>
        /// The <see cref="FilteredKanjis" /> property's name.
        /// </summary>
        public const string FilteredKanjisPropertyName = "FilteredKanjis";

        private List<KanjiModel> _filteredKanjis = new List<KanjiModel>();

        /// <summary>
        /// Sets and gets the FilteredKanjis property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public List<KanjiModel> FilteredKanjis
        {
            get
            {
                return _filteredKanjis;
            }
            set
            {
                Set(FilteredKanjisPropertyName, ref _filteredKanjis, value);
            }
        }

        #endregion ----- Filtered Kanjis -----

        #region ----- Show Kanji Command -----

        private RelayCommand<KanjiModel> _showKanjiCommand;

        /// <summary>
        /// Gets the ShowKanjiCommand.
        /// </summary>
        public RelayCommand<KanjiModel> ShowKanjiCommand
        {
            get
            {
                return _showKanjiCommand ?? (_showKanjiCommand = new RelayCommand<KanjiModel>(
                    ExecuteShowKanjiCommand,
                    CanExecuteShowKanjiCommand));
            }
        }

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

        #region ----- Is Loading Document-----

        /// <summary>
        /// The <see cref="IsLoadingDocument" /> property's name.
        /// </summary>
        public const string IsLoadingPropertyName = "IsLoadingDocument";

        private bool _isLoadingDocument = false;

        /// <summary>
        /// Sets and gets the IsLoading property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public bool IsLoadingDocument
        {
            get
            {
                return _isLoadingDocument;
            }
            set
            {
                Set(IsLoadingPropertyName, ref _isLoadingDocument, value);
            }
        }

        #endregion ----- Is Loading Document-----

        #region ----- Copy Text -----

        private RelayCommand _copyKanjiCommand;

        public RelayCommand CopyKanjiCommand
        {
            get
            {
                return _copyKanjiCommand
                    ?? (_copyKanjiCommand = new RelayCommand(ExecuteCopyKanjiCommand));
            }
        }

        private void ExecuteCopyKanjiCommand()
        {
            var words = string.Join("", FilteredKanjis.Select(kanji => kanji.Word).ToArray());
            Clipboard.SetText(words);
        }

        #endregion ----- Copy Text -----

        #region ----- Contructor -----

        public KanjiPartViewModel(IMessenger messenger, ISearchService<KanjiModel> kanjiService, IBuilder<KanjiModel> kanjiBuilder, IKanjiPartService kanjiPartService)
        {
            _messenger = messenger;
            _kanjiService = kanjiService;
            _kanjiBuilder = kanjiBuilder;
            _kanjiPartService = kanjiPartService;

            Parts = _kanjiPartService.GetListParts()
                .Select(SelectablePartModel.Create)
                .ToList();

            messenger.Register<ShowPartsMessage>(this, ProcessShowPartsMessage);
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