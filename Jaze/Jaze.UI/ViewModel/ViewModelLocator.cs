/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:Jaze.UI.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Jaze.UI.Models;
using Jaze.UI.Services;
using GalaSoft.MvvmLight.Messaging;
using Jaze.UI.Services.Documents;

namespace Jaze.UI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //services
            SimpleIoc.Default.Register<IMessenger>(() => Messenger.Default);
            SimpleIoc.Default.Register<ISearchService<GrammarModel>, GrammarService>();
            SimpleIoc.Default.Register<ISearchService<HanVietModel>, HanVietService>();
            SimpleIoc.Default.Register<ISearchService<JaenModel>, JaenService>();
            SimpleIoc.Default.Register<ISearchService<JaviModel>, JaviService>();
            SimpleIoc.Default.Register<ISearchService<KanjiModel>, KanjiService>();
            SimpleIoc.Default.Register<ISearchService<VijaModel>, VijaService>();

            //document builder
            SimpleIoc.Default.Register<IBuilder<GrammarModel>, GrammarBuilder>();
            SimpleIoc.Default.Register<IBuilder<HanVietModel>, HanVietBuilder>();
            SimpleIoc.Default.Register<IBuilder<JaenModel>, JaenBuilder>();
            SimpleIoc.Default.Register<IBuilder<JaviModel>, JaviBuilder>();
            SimpleIoc.Default.Register<IBuilder<KanjiModel>, KanjiBuilder>();
            SimpleIoc.Default.Register<IBuilder<VijaModel>, VijaBuilder>();

            //view model
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SearchBarViewModel>();
            SimpleIoc.Default.Register<SearchResultViewModel>();
            SimpleIoc.Default.Register<ItemDisplayViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public SearchBarViewModel SearchBar => ServiceLocator.Current.GetInstance<SearchBarViewModel>();

        public SearchResultViewModel SearchResult => ServiceLocator.Current.GetInstance<SearchResultViewModel>();

        public ItemDisplayViewModel ItemDisplay => ServiceLocator.Current.GetInstance<ItemDisplayViewModel>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}