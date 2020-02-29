using System;
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
using Prism.Interactivity.InteractionRequest;
using Prism.Services.Dialogs;

namespace Jaze.UI.ViewModel
{
    public class KanjiPartViewModel : DialogViewModelBase
    {
        #region ----- Services -----

        private readonly ISearchService<KanjiModel> _kanjiService;
        private readonly IBuilder<KanjiModel> _kanjiBuilder;
        private readonly IKanjiPartService _kanjiPartService;

        #endregion ----- Services -----

        #region ----- Properties -----

        private List<SelectablePartModel> _parts = new List<SelectablePartModel>();

        public List<SelectablePartModel> Parts
        {
            get => _parts;
            set => SetProperty(ref _parts, value);
        }

        private FlowDocument _itemDocument = null;

        public FlowDocument ItemDocument
        {
            get => _itemDocument;
            set => SetProperty(ref _itemDocument, value);
        }

        private bool _isLoadingDocument = false;

        public bool IsLoadingDocument
        {
            get => _isLoadingDocument;
            set => SetProperty(ref _isLoadingDocument, value);
        }

        private bool _isFiltering;

        public bool IsFiltering
        {
            get => _isFiltering;
            set => SetProperty(ref _isFiltering, value);
        }

        #endregion ----- Properties -----

        #region -----  Commands -----

        private DelegateCommand _updateFilterCommand;

        public DelegateCommand UpdateFilterCommand => _updateFilterCommand ?? (_updateFilterCommand = new DelegateCommand(
                                                          ExecuteUpdateFilterCommand,
                                                          CanExecuteUpdateFilterCommand));

        private async void ExecuteUpdateFilterCommand()
        {
            IsFiltering = true;
            var selectedPart = Parts.Where(part => part.IsSelected).ToArray();
            FilteredKanjis = await Task.Run(() => _kanjiPartService.GetListKanji(selectedPart));
            var selectableParts = await Task.Run(() => _kanjiPartService.GetSelectablePart(FilteredKanjis));
            foreach (var part in Parts)
            {
                part.IsEnabled = false;
            }
            foreach (var selectablePart in selectableParts)
            {
                var part = Parts.FirstOrDefault(p => p.Id == selectablePart.Id);
                if (part != null)
                {
                    part.IsEnabled = true;
                }
            }

            IsFiltering = false;
        }

        private bool CanExecuteUpdateFilterCommand()
        {
            return true;
        }

        private List<KanjiModel> _filteredKanjis = new List<KanjiModel>();

        public List<KanjiModel> FilteredKanjis
        {
            get => _filteredKanjis;
            set => SetProperty(ref _filteredKanjis, value);
        }

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

        private DelegateCommand _copyKanjiCommand;

        public DelegateCommand CopyKanjiCommand => _copyKanjiCommand
                                                   ?? (_copyKanjiCommand = new DelegateCommand(ExecuteCopyKanjiCommand));

        private void ExecuteCopyKanjiCommand()
        {
            var words = string.Join("", FilteredKanjis.Select(kanji => kanji.Word).ToArray());
            Clipboard.SetText(words);
        }

        #endregion -----  Commands -----

        #region ----- Contructor -----

        public KanjiPartViewModel(ISearchService<KanjiModel> kanjiService, IBuilder<KanjiModel> kanjiBuilder, IKanjiPartService kanjiPartService)
        {
            _kanjiService = kanjiService;
            _kanjiBuilder = kanjiBuilder;
            _kanjiPartService = kanjiPartService;

            Parts = _kanjiPartService.GetListParts()
                .Select(SelectablePartModel.Create)
                .ToList();
            Title = "Kanji Part";
        }

        private void ShowParts(List<string> parts)
        {
            Parts.ForEach(part => { part.IsSelected = false; part.IsEnabled = true; });
            foreach (var partStr in parts)
            {
                var part = Parts.FirstOrDefault(obj => obj.Word == partStr);
                if (part != null)
                {
                    part.IsSelected = true;
                }
            }
            ExecuteUpdateFilterCommand();
        }

        #endregion ----- Contructor -----

        #region Dialog

        public override event Action<IDialogResult> RequestClose;

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            var parts = parameters.GetValue<List<string>>("Parts");
            ShowParts(parts);
        }

        #endregion Dialog
    }
}