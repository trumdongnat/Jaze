using System.Collections.Generic;

namespace Jaze.UI.Services
{
    public interface ISearchService<TModel> where TModel : new()
    {
        List<TModel> Search(SearchArgs searchArgs);

        List<TModel> SearchExact(string key);

        List<TModel> SearchStartWith(string key);

        List<TModel> SearchEndWith(string key);

        List<TModel> SearchContain(string key);

        List<TModel> GetAll();
    }
}