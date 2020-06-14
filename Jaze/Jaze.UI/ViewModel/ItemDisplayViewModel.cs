using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using System.Windows;
using Jaze.UI.Services.URI;
using System.Linq;
using Jaze.Domain.Definitions;
using Jaze.UI.Models;
using Jaze.UI.Repository;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using Prism.Services.Dialogs;
using Jaze.UI.Views;
using System;
using System.Collections.Generic;
using Jaze.UI.Services;

namespace Jaze.UI.ViewModel
{
    public class ItemDisplayViewModel : ViewModelBase, INavigationAware
    {
        #region ----- Services -----

        private readonly IEventAggregator _messenger;
        private readonly IDictionaryRepository _dictionaryRepository;
        private readonly IUriService _uriService;
        private readonly IUserDataRepository _userDataRepository;
        private readonly IDialogService _dialogService;

        #endregion ----- Services -----

        #region ----- Properties -----

        private DictionaryType _dictionaryType;
        private object _item;

        private FlowDocument _itemDocument = null;

        public FlowDocument ItemDocument
        {
            get => _itemDocument;
            set => SetProperty(ref _itemDocument, value);
        }

        private bool _isLoading = false;

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        #endregion ----- Properties -----

        #region ----- Commands -----

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
                            _dialogService.ShowKanjiPartDialog(parts, null);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

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

        private DelegateCommand<string> _copyTextCommand;

        public DelegateCommand<string> CopyTextCommand => _copyTextCommand ?? (_copyTextCommand = new DelegateCommand<string>(
                                                              ExecuteCopyTextCommand,
                                                              CanExecuteCopyTextCommand));

        private void ExecuteCopyTextCommand(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                switch (_item)
                {
                    case KanjiModel kanji:
                        text = kanji.Word;
                        break;

                    case GrammarModel grammar:
                        text = grammar.Struct;
                        break;

                    case HanVietModel hanviet:
                        text = hanviet.Word;
                        break;

                    case JaenModel jaen:
                        text = jaen.Word;
                        break;

                    case JaviModel javi:
                        text = javi.Word;
                        break;

                    case VijaModel vija:
                        text = vija.Word;
                        break;
                }
            }
          
            Clipboard.SetText(text ?? string.Empty);
        }

        private bool CanExecuteCopyTextCommand(string parameter)
        {
            return true;
        }

        private DelegateCommand _addToGroupCommand;

        public DelegateCommand AddToGroupCommand =>
            _addToGroupCommand ?? (_addToGroupCommand = new DelegateCommand(ExecuteAddToGroupCommand));

        private void ExecuteAddToGroupCommand()
        {
            if (_item != null)
            {
                var type = _dictionaryRepository.GetType(_item);
                object id = _item.GetType().GetProperty("Id")?.GetValue(_item);
                if (id is int idInt)
                {
                    var item = new GroupItemModel() { Type = type, WordId = idInt };
                    _dialogService.ShowSelectGroupDialog(item, null);
                }
            }
        }

        #endregion ----- Commands -----

        #region ----- Contructor -----

        public ItemDisplayViewModel(IEventAggregator messenger, IUriService uriService, IDictionaryRepository dictionaryRepository, IUserDataRepository userDataRepository, IDialogService dialogService)
        {
            _messenger = messenger;
            _uriService = uriService;
            _dictionaryRepository = dictionaryRepository;
            _userDataRepository = userDataRepository;
            _dialogService = dialogService;
        }

        #endregion ----- Contructor -----

        #region ----- Show Item -----

        private async Task<FlowDocument> LoadDocumentAsync(object item)
        {
            await _dictionaryRepository.LoadFullAsync(item);
            return _dictionaryRepository.GetDocument(item);
        }

        private async void ShowItem(object item)
        {
            IsLoading = true;
            ItemDocument = await LoadDocumentAsync(item);
            IsLoading = false;
        }

        #endregion ----- Show Item -----

        #region Navigation

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var item = navigationContext.Parameters[ParamNames.Item];
            _item = item;
            _dictionaryType = _dictionaryRepository.GetType(item);
            ShowItem(item);
            SaveHistory(item);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion Navigation

        private async void SaveHistory(object item)
        {
            if (item == null)
            {
                return;
            }
            var type = _dictionaryRepository.GetType(item);
            if (type == DictionaryType.Unknown)
            {
                return;
            }

            object id = _item.GetType().GetProperty("Id")?.GetValue(_item);
            if (id is int idInt)
            {
                await _userDataRepository.AddHistory(type, idInt);
            }
        }
    }
}