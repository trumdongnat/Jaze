using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Jaze.DAO;
using Jaze.Model;

namespace Jaze.Control
{
    /// <summary>
    /// Interaction logic for KanjiPartControl.xaml
    /// </summary>
    public partial class KanjiPartControl : UserControl
    {
        private const int MaxPartStrock = 17;
        private List<Part> _checkedParts;
        private HashSet<Kanji> _kanjis;
        private List<ToggleButton> _buttons;
        public KanjiPartControl()
        {
            _checkedParts = new List<Part>();
            _kanjis = new HashSet<Kanji>();
            _buttons = new List<ToggleButton>();
            InitializeComponent();
            InitializeUI();
        }

        public KanjiPartControl(Kanji kanji)
        {
            _checkedParts = new List<Part>(kanji.Parts);
            _kanjis = new HashSet<Kanji>();
            _buttons = new List<ToggleButton>();
            InitializeComponent();
            InitializeUI();
            Filter();
            CheckButton();
        }

        private void CheckButton()
        {
            foreach (var button in _buttons)
            {
                if (_checkedParts.Contains(button.Tag))
                {
                    button.IsChecked = true;
                }
            }
        }

        // ReSharper disable once InconsistentNaming
        private void InitializeUI()
        {
            var parts = DatabaseContext.Context.Parts;
            for (int i = 1; i <= MaxPartStrock; i++)
            {
                var arr = parts.Where(p => p.Stroke == i).ToArray();
                if (arr.Length > 0)
                {
                    var panel = new WrapPanel()
                    {
                        Orientation = Orientation.Horizontal
                    };
                    panel.Children.Add(new TextBlock()
                    {
                        Text = i.ToString(),
                    });
                    foreach (var part in arr)
                    {
                        var button = (new ToggleButton()
                        {
                            Content = part.Word,
                            Tag = part
                        });
                        panel.Children.Add(button);
                        _buttons.Add(button);
                    }
                    mainPanel.Children.Add(panel);
                }
            }
        }

        private void ButtonCopy_OnClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(textBox.Text);
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            if (button != null)
            {
                var part = button.Tag as Part;
                if (!_checkedParts.Contains(part))
                {
                    _checkedParts.Add(part);
                    if (_checkedParts.Count == 1)
                    {
                        Filter();
                    }
                    else
                    {
                        ContinueFilter(part);
                    }
                }
            }
        }

        private void ToggleButton_OnUnChecked(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            if (button != null)
            {
                var part = button.Tag as Part;
                _checkedParts.Remove(part);
                Filter();
            }
        }

        private void Filter()
        {
            if (_checkedParts.Count == 0)
            {
                _kanjis.Clear();
            }
            else
            {
                _kanjis = new HashSet<Kanji>(_checkedParts[0].Kanjis);
                foreach (var part in _checkedParts)
                {
                    _kanjis.IntersectWith(part.Kanjis);
                }
                textBox.Text = string.Concat(_kanjis.Select(k => k.Word));
            }
            DetectCheckableButton();
        }

        private void ContinueFilter(Part part)
        {
            _kanjis.IntersectWith(part.Kanjis);
            textBox.Text = string.Concat(_kanjis.Select(k => k.Word));
            DetectCheckableButton();
        }

        private void DetectCheckableButton()
        {
            HashSet<Part> checkableParts;
            if (_checkedParts.Count == 0)
            {
                checkableParts = new HashSet<Part>(DatabaseContext.Context.Parts);
            }
            else
            {
                checkableParts = new HashSet<Part>();
                foreach (var kanji in _kanjis)
                {
                    checkableParts.UnionWith(kanji.Parts);
                }
            }

            //
            foreach (var button in _buttons)
            {
                button.IsEnabled = checkableParts.Contains(button.Tag);
            }
        }
    }
}
