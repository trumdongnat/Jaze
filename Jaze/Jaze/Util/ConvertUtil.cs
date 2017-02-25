using System;
using System.Collections.Generic;
using System.Linq;

namespace Jaze.Util
{
    public static class ConvertStringUtil
    {
        //romaji - hiragana - katakana
        private static readonly List<Tuple<string, string, string>> ListTuple = new List<Tuple<string, string, string>>();
        private static readonly char[] Vowels;
        private static readonly char[] Consonants;

        static ConvertStringUtil()
        {
            Tuple<string, string, string> t;
            t = new Tuple<string, string, string>("kya", "きゃ", "キャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("kyu", "きゅ", "キュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("kyo", "きょ", "キョ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("sha", "しゃ", "シャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("shu", "しゅ", "シュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("syu", "しゅ", "シュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("sho", "しょ", "ショ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("cha", "ちゃ", "チャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("chu", "ちゅ", "チュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("cyu", "ちゅ", "チュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("cho", "ちょ", "チョ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("nya", "にゃ", "ニャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("nyu", "にゅ", "ニュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("nyo", "にょ", "ニョ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("hya", "ひゃ", "ヒャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("hyu", "ひゅ", "ヒュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("hyo", "ひょ", "ヒョ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("mya", "みゃ", "ミャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("myu", "みゅ", "ミュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("myo", "みょ", "ミョ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("rya", "りゃ", "リャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ryu", "りゅ", "リュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ryo", "りょ", "リョ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("gya", "ぎゃ", "ギャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("gyu", "ぎゅ", "ギュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("gyo", "ぎょ", "ギョ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ja", "じゃ", "ジャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ju", "じゅ", "ジュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("jyu", "じゅ", "ジュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("jo", "じょ", "ジョ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("dya", "ぢゃ", "ヂャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("dyu", "ぢゅ", "ヂュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("dyo", "ぢょ", "ヂョ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("bya", "びゃ", "ビャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("byu", "びゅ", "ビュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("byo", "びょ", "ビョ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("pya", "ぴゃ", "ピャ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("pyu", "ぴゅ", "ピュ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("pyo", "ぴょ", "ピョ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ka", "か", "カ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ki", "き", "キ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ku", "く", "ク");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ke", "け", "ケ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ko", "こ", "コ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ta", "た", "タ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("chi", "ち", "チ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ti", "ち", "チ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("tsu", "つ", "ツ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("tu", "つ", "ツ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("te", "て", "テ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("to", "と", "ト");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("sa", "さ", "サ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("shi", "し", "シ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("si", "し", "シ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ci", "し", "シ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("su", "す", "ス");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("se", "せ", "セ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("so", "そ", "ソ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("na", "な", "ナ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ni", "に", "ニ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("nu", "ぬ", "ヌ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ne", "ね", "ネ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("no", "の", "ノ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ha", "は", "ハ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("hi", "ひ", "ヒ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("hu", "ふ", "フ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("fu", "ふ", "フ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("he", "へ", "ヘ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ho", "ほ", "ホ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ma", "ま", "マ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("mi", "み", "ミ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("mu", "む", "ム");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("me", "め", "メ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("mo", "も", "モ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ya", "や", "ヤ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("yu", "ゆ", "ユ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ye", "いぇ", "イェ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("yo", "よ", "ヨ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ra", "ら", "ラ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ri", "り", "リ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ru", "る", "ル");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("re", "れ", "レ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ro", "ろ", "ロ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("wa", "わ", "ワ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("wi", "うぃ", "ウィ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("we", "うぇ", "ウェ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("wo", "を", "ヲ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ga", "が", "ガ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("gi", "ぎ", "ギ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("gu", "ぐ", "グ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ge", "げ", "ゲ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("go", "ご", "ゴ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("za", "ざ", "ザ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("zi", "じ", "ジ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ji", "じ", "ジ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("zu", "ず", "ズ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ze", "ぜ", "ゼ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("zo", "ぞ", "ゾ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("da", "だ", "ダ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("di", "ぢ", "ヂ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("du", "づ", "ヅ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("de", "で", "デ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("do", "ど", "ド");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("ba", "ば", "バ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("bi", "び", "ビ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("bu", "ぶ", "ブ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("be", "べ", "ベ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("bo", "ぼ", "ボ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("pa", "ぱ", "パ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("pi", "ぴ", "ピ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("pu", "ぷ", "プ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("pe", "ぺ", "ペ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("po", "ぽ", "ポ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("la", "ぁ", "ァ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("li", "ぃ", "ィ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("lu", "ぅ", "ゥ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("le", "ぇ", "ェ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("lo", "ぉ", "ォ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("a", "あ", "ア");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("i", "い", "イ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("u", "う", "ウ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("e", "え", "エ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("o", "お", "オ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("n", "ん", "ン");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("k", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("t", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("c", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("s", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("h", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("f", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("m", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("y", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("r", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("w", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("g", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("z", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("d", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("b", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("p", "っ", "ッ");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("-", "-", "ー");
            ListTuple.Add(t);
            t = new Tuple<string, string, string>("'", "", "");
            ListTuple.Add(t);
            //
            Vowels = "aăâeêioôơuưyàằầèềìòồờùừỳảẳẩẻểỉỏổởủửỷãẵẫẽễĩõỗỡũữỹáắấéếíóốớúứýạặậẹệịọộợụựỵ".ToCharArray();
            Consonants = "bcdđghklmnpqrstvx".ToCharArray();
        }

        public static List<char> FilterCharsInString(string text, CharSet charSet)
        {
            int min = 0, max = 0;
            switch (charSet)
            {
                case CharSet.Romaji:
                    min = 0x0020;
                    max = 0x007E;
                    break;

                case CharSet.Hiragana:
                    min = 0x3040;
                    max = 0x309F;
                    break;

                case CharSet.Katakana:
                    min = 0x30A0;
                    max = 0x30FF;
                    break;

                case CharSet.Kanji:
                    min = 0x4E00;
                    max = 0x9FBF;
                    break;
            }
            return text.Where(e => e >= min && e <= max).ToList();
        }
       
        public static string ConvertString(string source, CharSet fromType, CharSet toType)
        {
            string s = source;
            foreach (var tuple in ListTuple)
            {
                string f = "", t = "";
                switch (fromType)
                {
                    case CharSet.Romaji:
                        f = tuple.Item1;
                        break;

                    case CharSet.Hiragana:
                        f = tuple.Item2;
                        break;

                    case CharSet.Katakana:
                        f = tuple.Item3;
                        break;
                }
                switch (toType)
                {
                    case CharSet.Romaji:
                        t = tuple.Item1;
                        break;

                    case CharSet.Hiragana:
                        t = tuple.Item2;
                        break;

                    case CharSet.Katakana:
                        t = tuple.Item3;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("toType");
                }
                s = s.Replace(f, t);
            }
            return s;
        }

        public static string ConvertRomaji2Hiragana(string source)
        {
            return ConvertString(source, CharSet.Romaji, CharSet.Hiragana);
        }

        public static string ConvertRomaji2Katakana(string source)
        {
            return ConvertString(source, CharSet.Romaji, CharSet.Katakana);
        }

        public static bool IsKanji(char c)
        {
            int min = 0x4E00;
            int max = 0x9FBF;
            return c >= min && c <= max;
        }

        public static bool IsHiragana(char c)
        {
            int min = 0x3040;
            int max = 0x309F;
            return c >= min && c <= max;
        }

        public static bool IsKatakana(char c)
        {
            int min = 0x30A0;
            int max = 0x30FF;
            return c >= min && c <= max;
        }

        public static bool IsContainJapaneseCharacter(string sentence)
        {
            return sentence!=null && sentence.Any(c => c >= 0x3040);
        }

        public static bool IsJapanese(string s)
        {
            return s.ToCharArray().All(c =>IsHiragana(c) || IsKatakana(c) || IsKanji(c));
        }

        public static bool IsVietnamese(string s)
        {
            s = s.ToLower();
            while (s.Length > 0 && Consonants.Contains(s[0]))
            {
                s = s.Substring(1);
            }
            while (s.Length > 0 && Consonants.Contains(s[s.Length-1]))
            {
                s = s.Substring(0,s.Length-1);
            }
            return s.Length > 0 && s.ToCharArray().All(c => Vowels.Contains(c));
        }
    }

    public enum CharSet
    {
        Romaji,
        Hiragana,
        Katakana,
        Kanji
    }
}
