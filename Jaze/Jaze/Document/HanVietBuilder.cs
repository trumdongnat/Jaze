using System.Windows.Documents;
using System.Windows.Markup;
using HTMLConverter;

namespace Jaze.Document
{
    static class HanVietBuilder
    {
        internal static FlowDocument BuildQuickView(Model.HanViet hanViet)
        {
            return Build(hanViet);
        }

        internal static FlowDocument Build(Model.HanViet hanViet)
        {
            if (hanViet == null)
            {
                return null;
            }
            
            var docStr = HtmlToXamlConverter.ConvertHtmlToXaml(hanViet.Content.Replace("<hr>", ""), true);
            return XamlReader.Parse(docStr) as FlowDocument;
            
        }
    }
}
