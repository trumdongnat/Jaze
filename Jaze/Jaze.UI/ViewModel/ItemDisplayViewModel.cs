﻿using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Documents;
using Jaze.UI.Definitions;
using Jaze.UI.Messages;
using System.Windows;
using Jaze.UI.Services.URI;
using System.Linq;
using Jaze.UI.Repository;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;

namespace Jaze.UI.ViewModel
{
    public class ItemDisplayViewModel : ViewModelBase, INavigationAware
    {
        #region ----- Services -----

        private readonly IEventAggregator _messenger;

        private readonly IDictionaryRepository _dictionaryRepository;

        private readonly IUriService _uriService;

        #endregion ----- Services -----

        #region ----- Properties -----

        private DictionaryType _dictionaryType;

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
                            _messenger.GetEvent<PubSubEvent<ShowPartsMessage>>().Publish(new ShowPartsMessage(parts));
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

        private void ExecuteCopyTextCommand(string parameter)
        {
            Clipboard.SetText(parameter);
        }

        private bool CanExecuteCopyTextCommand(string parameter)
        {
            return true;
        }

        #endregion ----- Commands -----

        #region ----- Contructor -----

        public ItemDisplayViewModel(IEventAggregator messenger, IUriService uriService, IDictionaryRepository dictionaryRepository)
        {
            _messenger = messenger;
            _uriService = uriService;
            _dictionaryRepository = dictionaryRepository;
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
            _dictionaryType = _dictionaryRepository.GetType(item);
            ItemDocument = await LoadDocumentAsync(item);
            IsLoading = false;
        }

        #endregion ----- Show Item -----

        #region Navigation

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var item = navigationContext.Parameters[ParamNames.Item];
            ShowItem(item);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion Navigation
    }
}