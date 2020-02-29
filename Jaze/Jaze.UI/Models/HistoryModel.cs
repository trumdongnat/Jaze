using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain.Definitions;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class HistoryModel : ModelBase
    {
        public HistoryModel()
        {
        }

        public HistoryModel(History history)
        {
            Type = history.Type;
            Id = history.Id;
            LastTime = history.LastTime;
        }

        public DictionaryType Type { get; set; }
        public int Id { get; set; }
        public DateTime LastTime { get; set; }
        public object Item { get; set; }
    }
}