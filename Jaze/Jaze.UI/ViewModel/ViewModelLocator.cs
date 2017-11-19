/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:Jaze.UI.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>

  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Jaze.UI.Model;
using Jaze.UI.Models;
using Jaze.UI.Services;
using GalaSoft.MvvmLight.Messaging;

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
            SimpleIoc.Default.Register<IDataService, DataService>();
            SimpleIoc.Default.Register<ISearchService<GrammarModel>, GrammarService>();
            SimpleIoc.Default.Register<ISearchService<HanVietModel>, HanVietService>();
            SimpleIoc.Default.Register<ISearchService<JaenModel>, JaenService>();
            SimpleIoc.Default.Register<ISearchService<JaviModel>, JaviService>();
            SimpleIoc.Default.Register<ISearchService<KanjiModel>, KanjiService>();
            SimpleIoc.Default.Register<ISearchService<VijaModel>, VijaService>();

            //view model
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SearchBarViewModel>();
        }

        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        public SearchBarViewModel SearchBar => ServiceLocator.Current.GetInstance<SearchBarViewModel>();

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}