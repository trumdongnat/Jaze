using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain.Definitions;
using Jaze.UI.Models;

namespace Jaze.UI.Services.UserData
{
    public interface IHistoryService
    {
        void Add(DictionaryType type, int id);

        void Add(DictionaryType type, int id, DateTime time);

        void Remove(DictionaryType type, int id);

        List<HistoryModel> GetListHistory();

        List<HistoryModel> GetListHistory(DateTime from);
    }
}