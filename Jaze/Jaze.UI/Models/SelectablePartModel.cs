namespace Jaze.UI.Models
{
    public class SelectablePartModel : PartModel
    {
        public const string IsSelectedPropertyName = "IsSelected";
        private bool _isSelected = false;

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        public const string IsEnabledPropertyName = "IsEnabled";

        private bool _isEnabled = true;

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
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