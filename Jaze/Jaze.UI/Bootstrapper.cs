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
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Prism.Unity;
using ContainerControlledLifetimeManager = Microsoft.Practices.Unity.ContainerControlledLifetimeManager;

namespace Jaze.UI
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();
            //services
            Container.RegisterType<ISearchService<GrammarModel>, GrammarService>();
            Container.RegisterType<ISearchService<HanVietModel>, HanVietService>();
            Container.RegisterType<ISearchService<JaenModel>, JaenService>();
            Container.RegisterType<ISearchService<JaviModel>, JaviService>();
            Container.RegisterType<ISearchService<KanjiModel>, KanjiService>();
            Container.RegisterType<ISearchService<VijaModel>, VijaService>();
            Container.RegisterType<IKanjiPartService, KanjiPartService>();
            Container.RegisterType<IUriService, UriService>();
            Container.RegisterType<IHistoryService, HistoryService>();
            Container.RegisterType<IGroupService, GroupService>();

            //document builder
            Container.RegisterType<IBuilder<GrammarModel>, GrammarBuilder>();
            Container.RegisterType<IBuilder<HanVietModel>, HanVietBuilder>();
            Container.RegisterType<IBuilder<JaenModel>, JaenBuilder>();
            Container.RegisterType<IBuilder<JaviModel>, JaviBuilder>();
            Container.RegisterType<IBuilder<KanjiModel>, KanjiBuilder>();
            Container.RegisterType<IBuilder<VijaModel>, VijaBuilder>();
            //repository
            Container.RegisterType<IDictionaryRepository, DictionaryRepository>();
            Container.RegisterType<IUserDataRepository, UserDataRepository>();

            Container.RegisterType<IDialogCoordinator, DialogCoordinator>();

            //search bar view model singleton
            Container.RegisterType<SearchBarViewModel>(new ContainerControlledLifetimeManager());

            //view for navigation
            Container.RegisterTypeForNavigation<ItemDisplayView>();
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