using System.Windows.Documents;
using System.Windows.Markup;
using HTMLConverter;
using Jaze.UI.Models;

namespace Jaze.UI.Services.Documents
{
    public class HanVietBuilder : BuilderBase<HanVietModel>
    {
        public override FlowDocument Build(HanVietModel hanViet)
        {
            if (hanViet == null)
            {
                return null;
            }

            var docStr = HtmlToXamlConverter.ConvertHtmlToXaml(hanViet.Content.Replace("<hr>", ""), true);
            return XamlReader.Parse(docStr) as FlowDocument;
        }

        public override FlowDocument BuildLite(HanVietModel model)
        {
            return Build(model);
        }
    }
}