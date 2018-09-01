using Jaze.UI.Models;
using System.Collections.Generic;

namespace Jaze.UI.Services
{
    public interface IKanjiPartService
    {
        List<PartModel> GetListParts();

        List<KanjiModel> GetListKanji(params PartModel[] parts);

        List<PartModel> GetSelectablePart(List<KanjiModel> kanjis);
    }
}