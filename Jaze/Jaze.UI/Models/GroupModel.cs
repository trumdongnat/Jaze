using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze.Domain.Entities;
using Prism.Mvvm;

namespace Jaze.UI.Models
{
    public class GroupModel : BindableBase
    {
        public GroupModel()
        {
        }

        public GroupModel(Group @group)
        {
            Id = group.Id;
            _name = group.Name;
        }

        public int Id { get; set; }
        public bool IsLoadFull { get; set; }
        private string _name;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public ObservableCollection<GroupItemModel> Items { get; set; }
    }
}