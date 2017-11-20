using System;
using System.Collections.Generic;
using Jaze.UI.Definitions;
using Jaze.UI.Models;

namespace Jaze.UI.Services
{
    public abstract class ServiceBase<TModel> : ISearchService<TModel> where TModel : new()
    {
        public virtual List<TModel> Search(SearchArgs searchArgs)
        {
            if (string.IsNullOrWhiteSpace(searchArgs.SearchKey))
            {
                return GetAll();
            }
            else
            {
                switch (searchArgs.Option)
                {
                    case SearchOption.Exact:
                        return SearchExact(searchArgs.SearchKey);

                    case SearchOption.StartWith:
                        return SearchStartWith(searchArgs.SearchKey);

                    case SearchOption.EndWith:
                        return SearchEndWith(searchArgs.SearchKey);

                    case SearchOption.Contain:
                        return SearchContain(searchArgs.SearchKey);

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public abstract List<TModel> SearchExact(string key);

        public abstract List<TModel> SearchStartWith(string key);

        public abstract List<TModel> SearchEndWith(string key);

        public abstract List<TModel> SearchContain(string key);

        public abstract List<TModel> GetAll();

        public abstract void LoadFull(TModel model);
    }
}