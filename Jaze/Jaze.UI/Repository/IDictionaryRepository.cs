using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Jaze.UI.Definitions;
using Jaze.UI.Models;

namespace Jaze.UI.Repository
{
    public interface IDictionaryRepository
    {
        List<Dictionary> GetDictionarys();

        Task<List<object>> SearchAsync(SearchArgs args);

        Task LoadFullAsync(object item);

        FlowDocument GetDocument(object item);

        FlowDocument GetQuickViewDocument(object item);

        DictionaryType GetType(object item);
    }
}