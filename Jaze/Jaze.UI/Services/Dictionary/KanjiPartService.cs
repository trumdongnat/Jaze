using System.Collections.Generic;
using System.Linq;
using Jaze.UI.Models;
using Jaze.Domain;

namespace Jaze.UI.Services
{
    public class KanjiPartService : IKanjiPartService
    {
        private static readonly PartModel[] _parts;
        private static readonly KanjiModel[] _kanjis;
        private static readonly bool[,] _kanjiParts;
        private static readonly int _maxPartId;
        private static readonly int _maxKanjiId;

        static KanjiPartService()
        {
            using (var db = new JazeDatabaseContext())
            {
                _maxPartId = db.Parts.Max(entity => entity.Id);
                _maxKanjiId = db.Kanjis.Max(entity => entity.Id);
                _parts = new PartModel[_maxPartId + 1];
                _kanjis = new KanjiModel[_maxKanjiId + 1];
                _kanjiParts = new bool[_maxKanjiId + 1, _maxPartId + 1];

                var parts = db.Parts.ToList().Select(PartModel.Create);

                foreach (var part in parts)
                {
                    _parts[part.Id] = part;
                }

                var kanjis = db.Kanjis.ToList().Select(KanjiModel.Create);
                foreach (var kanji in kanjis)
                {
                    _kanjis[kanji.Id] = kanji;
                }

                foreach (var kanjiPart in db.KanjiParts)
                {
                    _kanjiParts[kanjiPart.KanjiId, kanjiPart.PartId] = true;
                }
            }
        }

        public List<KanjiModel> GetListKanji(params PartModel[] parts)
        {
            if (parts.Length == 0)
            {
                return new List<KanjiModel>();
            }
            var filteredKanjis = new List<KanjiModel>();

            for (int kanjiId = 0; kanjiId <= _maxKanjiId; kanjiId++)
            {
                var flag = true;
                foreach (var part in parts)
                {
                    if (!_kanjiParts[kanjiId, part.Id])
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    filteredKanjis.Add(_kanjis[kanjiId]);
                }
            }
            return filteredKanjis;
        }

        public List<PartModel> GetListParts()
        {
            return _parts.Where(part => part != null).ToList();
        }

        public List<PartModel> GetSelectablePart(List<KanjiModel> kanjis)
        {
            if (kanjis.Count == 0)
            {
                return GetListParts();
            }
            var _selectablePartIds = new bool[_maxPartId + 1];
            foreach (var kanji in kanjis)
            {
                for (int partId = 0; partId <= _maxPartId; partId++)
                {
                    _selectablePartIds[partId] = _selectablePartIds[partId] || _kanjiParts[kanji.Id, partId];
                }
            }
            return _parts.Where(part => part != null && _selectablePartIds[part.Id]).ToList();
        }

        private class LiteKanjiModel
        {
            public int Id { get; set; }
            public string Word { get; set; }
            public bool[] Parts { get; set; }
        }
    }
}