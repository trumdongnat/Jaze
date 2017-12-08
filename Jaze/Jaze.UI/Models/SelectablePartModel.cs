using Jaze.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.UI.Models
{
    public class SelectablePartModel : PartModel
    {
        public const string IsSelectedPropertyName = "IsSelected";
        private bool _isSelected = false;

        public bool IsSelected
        {
            get => _isSelected;
            set => Set(IsSelectedPropertyName, ref _isSelected, value);
        }

        public const string IsEnabledPropertyName = "IsEnabled";

        private bool _isEnabled = true;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => Set(IsEnabledPropertyName, ref _isEnabled, value);
        }

        public static SelectablePartModel Create(PartModel model)
        {
            return new SelectablePartModel
            {
                Id = model.Id,
                Value = model.Value,
                Word = model.Word,
                Stroke = model.Stroke,
            };
        }
    }
}