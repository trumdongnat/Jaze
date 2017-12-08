using Jaze.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaze.UI.Services
{
    public interface IKanjiPartService
    {
        List<PartModel> GetListParts();

        List<KanjiModel> GetListKanji(params PartModel[] parts);

        List<PartModel> GetSelectablePart(List<KanjiModel> kanjis);
    }
}