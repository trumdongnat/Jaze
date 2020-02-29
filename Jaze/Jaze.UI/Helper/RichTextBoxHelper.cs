using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Jaze.UI.Helper
{
    public static class RichTextBoxHelper
    {
        public static FlowDocument GetDocument(DependencyObject obj)
        {
            return (FlowDocument)obj.GetValue(DocumentProperty);
        }

        public static void SetDocument(DependencyObject obj, FlowDocument value)
        {
            obj.SetValue(DocumentProperty, value);
        }

        // Using a DependencyProperty as the backing store for Document.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DocumentProperty =
            DependencyProperty.RegisterAttached("Document", typeof(FlowDocument), typeof(RichTextBoxHelper), new PropertyMetadata(null, OnDocumentChange));

        private static void OnDocumentChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var flowDocument = e.NewValue as FlowDocument;
            switch (d)
            {
                case RichTextBox richTextBox:
                    richTextBox.Document = flowDocument;
                    break;

                case FlowDocumentReader viewer:
                    viewer.Document = flowDocument;
                    break;

                case FlowDocumentScrollViewer scrollViewer:
                    scrollViewer.Document = flowDocument;
                    break;

                case FlowDocumentPageViewer pageViewer:
                    pageViewer.Document = flowDocument;
                    break;
            }
        }
    }
}