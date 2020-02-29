using System.Windows.Documents;
using System.Windows.Markup;
using HTMLConverter;
using Jaze.Domain.Entities;

namespace Jaze.Documents
{
    static class HanVietBuilder
    {
        internal static FlowDocument BuildQuickView(HanViet hanViet)
        {
            return Build(hanViet);
        }

        internal static FlowDocument Build(HanViet hanViet)
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
