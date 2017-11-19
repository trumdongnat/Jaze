using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Jaze.UI.ViewModel
{
    public class ItemDisplayViewModel : ViewModelBase
    {
        #region ----- Services -----

        private IMessenger _messenger;
        //private ISearchService<GrammarModel> _grammarService;
        //private ISearchService<HanVietModel> _hanvietService;
        //private ISearchService<JaenModel> _jaenService;
        //private ISearchService<JaviModel> _javiService;
        //private ISearchService<KanjiModel> _kanjiService;
        //private ISearchService<VijaModel> _vijaService;

        #endregion ----- Services -----

        #region ----- Properties -----

        private object _item;

        #endregion ----- Properties -----

        #region ----- Item Document -----

        /// <summary>
        /// The <see cref="ItemDocument" /> property's name.
        /// </summary>
        public const string ItemDocumentPropertyName = "ItemDocument";

        private FlowDocument _itemDocument = null;

        /// <summary>
        /// Sets and gets the ItemDocument property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
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
    }
}