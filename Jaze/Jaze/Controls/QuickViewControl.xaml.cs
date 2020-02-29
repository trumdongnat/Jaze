using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace Jaze.Controls
{
    /// <summary>
    /// Interaction logic for QuickViewControl.xaml
    /// </summary>
    public partial class QuickViewControl : UserControl
    {
        public QuickViewControl()
        {
            InitializeComponent();
        }

        public void SetDocument(string word, string doc)
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
                TextBlockNotify.Text = "Không tìm thấy từ \"" + word + "\"";
                TextBlockNotify.Visibility = Visibility.Visible;
                DocumentReader.Visibility = Visibility.Collapsed;
            }
            else
            {
                DocumentReader.Document = document;
                DocumentReader.Visibility = Visibility.Visible;
                TextBlockNotify.Visibility = Visibility.Collapsed;
            }


            TextBlockTitle.Text = string.Format("Tra nhanh: {0}", word);
        }
    }
}