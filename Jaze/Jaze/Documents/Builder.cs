using System.Windows.Documents;
using Jaze.Domain.Entities;

namespace Jaze.Documents
{
    public static class Builder
    {
        #region Build full document
        public static FlowDocument Build(object model)
        {
            if (model == null)
            {
                return null;
            }

            if (model is Grammar)
            {
                return BuildGrammar((Grammar) model);
            }

            if (model is HanViet)
            {
                return BuildHanViet((HanViet) model);
            }

            if (model is Kanji)
            {
                return BuildKanji((Kanji) model);
            }

            if (model is JaVi)
            {
                return BuildJavi((JaVi) model);
            }

            if (model is ViJa)
            {
                return BuildVija(model as ViJa);
            }

            if (model is JaEn)
            {
                return BuildJaEn(model as JaEn);
            }
            return null;
        }

        private static FlowDocument BuildJaEn(JaEn jaEn)
        {
            return JaEnBuilder.Build(jaEn);
        }

        private static FlowDocument BuildVija(ViJa viJa)
        {
            return ViJaBuilder.Build(viJa);
        }

        private static FlowDocument BuildKanji(Kanji kanji)
        {
            return KanjiBuilder.Build(kanji);
        }

        private static FlowDocument BuildHanViet(HanViet hanViet)
        {
            return HanVietBuilder.Build(hanViet);
        }

        private static FlowDocument BuildGrammar(Grammar grammar)
        {
            return GrammarBuilder.Build(grammar);
        }

        private static FlowDocument BuildJavi(JaVi javi)
        {
            return JaViBuilder.Build(javi);
        }
        #endregion

        //        #region Build quickview document
        //        public static string BuildQuickViewKanji(string word)
        //        {
        //            return KanjiBuilder.BuildQuickView(DatabaseManager.GetKanji(word));
        //        }

        //        public static string BuildQuickViewHanViet(string word)
        //        {
        //            return HanVietBuilder.BuildQuickView(DatabaseManager.GetHanViet(word));
        //        }

        //        public static string BuildQuickViewJaVocab(string word,string kana)
        //        {
        //            return NewJapaneseBuilder.BuildQuickView(DatabaseManager.GetJaVocab(word, kana));
        //        }
        //        #endregion

    }


}