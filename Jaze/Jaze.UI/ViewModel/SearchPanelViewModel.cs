using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.UI.Views;
using Prism.Regions;

namespace Jaze.UI.ViewModel
{
    public class SearchPanelViewModel : ViewModelBase
    {
        public SearchPanelViewModel(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion("SearchBar", typeof(SearchBar));
            regionManager.RegisterViewWithRegion("SearchResult", typeof(SearchResult));
            regionManager.RegisterViewWithRegion("DictionaryList", typeof(ListDictionary));
            regionManager.RegisterViewWithRegion("ItemDisplay", typeof(ItemDisplayView));
        }
    }
}