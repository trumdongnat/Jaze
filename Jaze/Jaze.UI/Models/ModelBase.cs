using System;
using Prism.Mvvm;

namespace Jaze.UI.Models
{
    public class ModelBase : BindableBase
    {
        public bool IsLoadFull { get; set; }
        public bool IsBookmarked { get; set; }
        public DateTime? LastView { get; set; }
    }
}