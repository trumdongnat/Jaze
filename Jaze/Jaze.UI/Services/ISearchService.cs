using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.UI.Services
{
    public interface ISearchService<TModel> where TModel: new()
    {
        List<TModel> SearchExact(string key);
        List<TModel> SearchStartWith(string key);
        List<TModel> SearchEndWith(string key);
        List<TModel> SearchContain(string key);
        List<TModel> GetAll();
    }
}
