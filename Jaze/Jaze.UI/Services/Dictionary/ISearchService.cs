using System.Collections.Generic;
using Jaze.UI.Models;

namespace Jaze.UI.Services
{
    public interface ISearchService<TModel> where TModel : new()
    {
        TModel Get(int id);

        List<TModel> Search(SearchArgs searchArgs);

        List<TModel> SearchExact(string key);

        List<TModel> SearchStartWith(string key);

        List<TModel> SearchEndWith(string key);

        List<TModel> SearchContain(string key);

        List<TModel> GetAll();

        void LoadFull(TModel model);
    }
}