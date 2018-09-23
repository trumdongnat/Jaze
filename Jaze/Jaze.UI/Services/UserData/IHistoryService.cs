using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain.Definitions;

namespace Jaze.UI.Services.UserData
{
    public interface IHistoryService
    {
        void Add(DictionaryType type, int id);

        void Add(DictionaryType type, int id, DateTime time);

        void Remove(DictionaryType type, int id);
    }
}