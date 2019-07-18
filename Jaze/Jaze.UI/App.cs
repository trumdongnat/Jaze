using System.Windows;
using Jaze.UI.Models;
using Jaze.UI.Repository;
using Jaze.UI.Services;
using Jaze.UI.Services.Documents;
using Jaze.UI.Services.URI;
using Jaze.UI.Services.UserData;
using Jaze.UI.ViewModel;
using Jaze.UI.Views;
using MahApps.Metro.Controls.Dialogs;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using Unity.Lifetime;

namespace Jaze.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            //services
            containerRegistry.Register<ISearchService<GrammarModel>, GrammarService>();
            containerRegistry.Register<ISearchService<HanVietModel>, HanVietService>();
            containerRegistry.Register<ISearchService<JaenModel>, JaenService>();
            containerRegistry.Register<ISearchService<JaviModel>, JaviService>();
            containerRegistry.Register<ISearchService<KanjiModel>, KanjiService>();
            containerRegistry.Register<ISearchService<VijaModel>, VijaService>();
            containerRegistry.Register<IKanjiPartService, KanjiPartService>();
            containerRegistry.Register<IUriService, UriService>();
            containerRegistry.Register<IHistoryService, HistoryService>();
            containerRegistry.Register<IGroupService, GroupService>();

            //document builder
            containerRegistry.Register<IBuilder<GrammarModel>, GrammarBuilder>();
            containerRegistry.Register<IBuilder<HanVietModel>, HanVietBuilder>();
            containerRegistry.Register<IBuilder<JaenModel>, JaenBuilder>();
            containerRegistry.Register<IBuilder<JaviModel>, JaviBuilder>();
            containerRegistry.Register<IBuilder<KanjiModel>, KanjiBuilder>();
            containerRegistry.Register<IBuilder<VijaModel>, VijaBuilder>();

            //repository
            containerRegistry.Register<IDictionaryRepository, DictionaryRepository>();
            containerRegistry.Register<IUserDataRepository, UserDataRepository>();
            containerRegistry.Register<IDialogCoordinator, DialogCoordinator>();

            //search bar view model singleton
            containerRegistry.RegisterSingleton<SearchBarViewModel>();

            //view for navigation
            containerRegistry.RegisterForNavigation<ItemDisplayView>();

            //dialog
            containerRegistry.RegisterDialogWindow<DialogWindow>();
            containerRegistry.RegisterDialog<AddGroupView>();
            containerRegistry.RegisterDialog<AddToGroupView>();
            containerRegistry.RegisterDialog<EditGroupView>();
            containerRegistry.RegisterDialog<KanjiPart>();
            containerRegistry.RegisterDialog<SelectGroupView>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            ViewModelLocationProvider.Register<Shell, ShellViewModel>();
            ViewModelLocationProvider.Register<ItemDisplayView, ItemDisplayViewModel>();
            ViewModelLocationProvider.Register<KanjiPart, KanjiPartViewModel>();
            ViewModelLocationProvider.Register<ListDictionary, SearchBarViewModel>();
            ViewModelLocationProvider.Register<QuickView, QuickViewViewModel>();
            ViewModelLocationProvider.Register<SearchBar, SearchBarViewModel>();
            ViewModelLocationProvider.Register<SearchResult, SearchResultViewModel>();
            ViewModelLocationProvider.Register<SearchPanel, SearchPanelViewModel>();
            ViewModelLocationProvider.Register<WordGroupPanel, WordGroupViewModel>();
            ViewModelLocationProvider.Register<SelectGroupView, SelectGroupViewModel>();
            ViewModelLocationProvider.Register<AddGroupView, AddGroupViewModel>();
            ViewModelLocationProvider.Register<EditGroupView, EditGroupViewModel>();
            ViewModelLocationProvider.Register<HistoryView, HistoryViewModel>();
            ViewModelLocationProvider.Register<AddToGroupView, AddToGroupViewModel>();
        }
    }
}