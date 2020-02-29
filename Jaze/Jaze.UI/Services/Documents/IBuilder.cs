using System.Windows.Documents;

namespace Jaze.UI.Services.Documents
{
    public interface IBuilder<TModel>
    {
        FlowDocument Build(TModel model);

        FlowDocument BuildLite(TModel model);
    }
}