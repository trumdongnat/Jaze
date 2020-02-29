using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain.Definitions;
using Jaze.Domain.Entities;

namespace Jaze.UI.Models
{
    public class GroupItemModel
    {
        public GroupItemModel()
        {
        }

        public GroupItemModel(GroupItem item)
        {
            Id = item.Id;
            Type = item.Type;
            WordId = item.WordId;
        }

        public int Id { get; set; }
        public DictionaryType Type { get; set; }
        public int WordId { get; set; }
        public object Item { get; set; }
        public bool IsLoadFull { get; set; }
    }
}