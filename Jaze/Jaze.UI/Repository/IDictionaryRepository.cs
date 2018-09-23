using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using Jaze.Domain.Definitions;
using Jaze.UI.Models;

namespace Jaze.UI.Repository
{
    public interface IDictionaryRepository
    {
        List<Dictionary> GetDictionarys();

        Task<object> GetAsync(DictionaryType type, int id);

        Task<List<object>> SearchAsync(SearchArgs args);

        Task LoadFullAsync(object item);

        FlowDocument GetDocument(object item);

        FlowDocument GetQuickViewDocument(object item);

        DictionaryType GetType(object item);
    }
}