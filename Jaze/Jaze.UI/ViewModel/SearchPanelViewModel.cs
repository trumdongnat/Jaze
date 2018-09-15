using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.UI.Definitions;
using Jaze.UI.Views;
using Prism.Regions;

namespace Jaze.UI.ViewModel
{
    public class SearchPanelViewModel : ViewModelBase
    {
        public SearchPanelViewModel(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(RegionNames.SearchBar, typeof(SearchBar));
            regionManager.RegisterViewWithRegion(RegionNames.SearchResult, typeof(SearchResult));
            regionManager.RegisterViewWithRegion(RegionNames.DictionaryList, typeof(ListDictionary));
            regionManager.RegisterViewWithRegion(RegionNames.ItemDisplay, typeof(ItemDisplayView));
        }
    }
}