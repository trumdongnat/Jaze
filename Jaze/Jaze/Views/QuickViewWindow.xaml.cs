using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Jaze.Views
{
    /// <summary>
    /// Interaction logic for QuickViewWindow.xaml
    /// </summary>
    public partial class QuickViewWindow : Window
    {
        public QuickViewWindow()
        {
            InitializeComponent();
            
        }
        public void SetDocument(string title, string doc)
        {
            FlowDocument document;
            try
            {
                document = XamlReader.Parse(doc) as FlowDocument;
            }
            catch
            {
                document = null;
            }

            if (document == null)
            {
                var textblock = new TextBlock()
                {
                    FontSize = 18,
                    VerticalAlignment = System.Windows.VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                    Text = "Không tìm thấy"
                };
                this.Content = textblock;
            }
            else
            {
                document.PagePadding = new Thickness(0);
                FlowDocumentReader reader = new FlowDocumentReader()
                {
                    IsTwoPageViewEnabled = false,
                    Document = document,
                    Padding = new Thickness(5),
                    Zoom = 120,
                    IsFindEnabled = false
                };
                this.Content = reader;
            }

            this.Title = string.Format("Tra nhanh: {0}", title);
        }
    }
}
