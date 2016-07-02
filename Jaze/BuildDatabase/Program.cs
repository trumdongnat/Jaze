using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jaze;
using Jaze.DAO.Model;
using Jaze.DAO;
using SQLite;
using static System.Data.SqlClient.SqlBulkCopy;

namespace BuildDatabase
{
    class Program
    {
        const int Batch = 1000;
        static void Main(string[] args)
        {
            UpdateFull();
            Console.WriteLine("finish");
            Console.ReadKey();
        }

        private static void Test()
        {
            using (var container = new DatabaseContext())
            {
                var part = container.Radicals.ToArray()[5];

                foreach (var kanji in part.Kanjis)
                {
                    System.IO.File.AppendAllText(@"D:\result.txt", kanji.Word);
                }
            }
        }

        private static void UpdateFull()
        {
            UpdateGradeTable();
            UpdateRadicalTable();
            UpdateLevelTable();
            UpdatePartTable();
            UpdateKanjiTable();
            UpdateHanVietTable();
            UpdateGrammarTable();
            UpdateJaEnExamTable();
            UpdateJaEnTable();
            UpdateJaViTable();
            UpdateJaViExamTable();
            UpdateViJaTable();
        }

        private static void UpdatePartTable()
        {
            using (var container = new DatabaseContext())
            {
                var arr = System.IO.File.ReadAllLines(@"D:\part.txt");
                foreach (var s in arr)
                {
                    var t = s.Split('\t');
                    container.Parts.Add(new Part()
                    {
                        Value = t[1],
                        Word = t[2],
                        Stroke = int.Parse(t[3])
                    });
                }
                container.SaveChanges();
            }
        }

        private static void UpdateRadicalTable()
        {
            using (var container = new DatabaseContext())
            {
                var arr = System.IO.File.ReadAllLines(@"D:\radical.txt");
                foreach (var s in arr)
                {
                    var t = s.Split('\t');
                    container.Radicals.Add(new Radical()
                    {
                        Word = t[0],
                        HanViet = t[1],
                        Meaning = t[2],
                        Stroke = int.Parse(t[3])
                    });
                }
                container.SaveChanges();
            }
        }

        private static void UpdateLevelTable()
        {
            using (var container = new DatabaseContext())
            {
                for (int i = 1; i <= 5; i++)
                {
                    container.Levels.Add(new Level()
                    {
                        Name = "N" + i
                    });
                }
                container.Levels.Add(new Level()
                {
                    Name = "Basic"
                });
                container.SaveChanges();
            }
        }

        private static void UpdateGradeTable()
        {
            using (var container = new DatabaseContext())
            {
                for (int i = 1; i <= 6; i++)
                {
                    container.Grades.Add(new Grade()
                    {
                        Name = "Grade " + i
                    });
                }
                container.SaveChanges();
            }
        }

        private static void UpdateKanjiTable()
        {
            using (var container = new DatabaseContext())
            {
                container.Configuration.AutoDetectChangesEnabled = false;
                //load radical
                var radicals = container.Radicals.ToDictionary(r => r.Word, r => r);
                var parts = container.Parts.ToDictionary(p => p.Value, p => p);
                var levels = container.Levels.ToDictionary(l => l.Id, l => l);
                var grades = container.Grades.ToDictionary(g => g.Id, g => g);
                foreach (var s in System.IO.File.ReadAllLines(@"D:\kanji.txt"))
                {
                    var arr = s.Split('\t');
                    Console.WriteLine(arr[0]);
                    var kanji = new Kanji()
                    {
                        Word = arr[1],
                        HanViet = arr[3],
                        Stroke = int.Parse(arr[4]),
                        Onyomi = arr[7],
                        Kunyomi = arr[8],
                        VieMeaning = arr[9],
                        EngMeaning = arr[10]
                    };

                    //radical
                    string t = arr[2];
                    kanji.Radical = radicals[t];
                    radicals[t].Kanjis.Add(kanji);

                    //grade
                    t = arr[5];
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        int i = int.Parse(t);
                        kanji.Grade = grades[i];
                        grades[i].Kanjis.Add(kanji);
                    }

                    //level
                    t = arr[6];
                    if (!string.IsNullOrWhiteSpace(t))
                    {
                        int i = int.Parse(t);
                        kanji.Level = levels[i];
                        levels[i].Kanjis.Add(kanji);
                    }

                    //parts
                    foreach (var p  in arr[11].Split(','))
                    {
                        kanji.Parts.Add(parts[p]);
                        parts[p].Kanjis.Add(kanji);
                    }

                    kanji.Frequence = !string.IsNullOrWhiteSpace(arr[12]) ? int.Parse(arr[12]) : int.MaxValue;
                    kanji.StrokeOrder = string.Empty;
                    container.Kanjis.Add(kanji);
                }
                Console.WriteLine("write to db");

                container.SaveChanges();
            }
        }

        private static void UpdateHanVietTable()
        {
            var container = new DatabaseContext();
            var arr = System.IO.File.ReadAllLines(@"D:\hanviet.csv");
            container.Configuration.AutoDetectChangesEnabled = false;
            container.Configuration.ValidateOnSaveEnabled = false;
            int count = 0;
            foreach (var s in arr)
            {
                var t = s.Split('\t');
                Console.WriteLine(t[0]);
                container.HanViets.Add(new HanViet()
                {
                    Word = t[1],
                    Reading = t[2],
                    Content = t[3]
                });
                count ++;
                if (count%Batch == 0)
                {
                    Console.WriteLine("saving");
                    container.SaveChanges();
                    container.Dispose();
                    container = new DatabaseContext();
                    container.Configuration.AutoDetectChangesEnabled = false;
                    container.Configuration.ValidateOnSaveEnabled = false;
                }
            }
            container.SaveChanges();
        }

        private static void UpdateGrammarTable()
        {
            var container = new DatabaseContext();
            var arr = System.IO.File.ReadAllLines(@"D:\grammar.csv");
            var levels = container.Levels.ToDictionary(l => l.Name, l => l);
            foreach (var s in arr)
            {
                var t = s.Split('\t');
                Console.WriteLine(t[0]);
                var grammar = new Grammar()
                {
                    Struct = t[1],
                    Detail = t[2],

                    Meaning = t[4]
                };
                grammar.Level = levels[t[3]];
                levels[t[3]].Grammars.Add(grammar);
                container.Grammars.Add(grammar);
            }
            container.SaveChanges();
        }

        private static void UpdateJaEnExamTable()
        {
            var container = new DatabaseContext();
            container.Configuration.AutoDetectChangesEnabled = false;
            container.Configuration.ValidateOnSaveEnabled = false;

            var sqliteDb = new SQLiteConnection(@"C:\Users\CuHo\Desktop\Mazii\jaen.db");
            var list = sqliteDb.Table<exam>().ToList();
            
            int count = 0;
            foreach (var e in list)
            {
               
                Console.WriteLine(e.id);
                container.JaEnExamples.Add(new JaEnExample()
                {
                    English = e.eng,
                    Japanese = e.jpn
                    
                });
                count++;
                if (count % Batch == 0)
                {
                    Console.WriteLine("saving");
                    container.SaveChanges();
                    container.Dispose();
                    container = new DatabaseContext();
                    container.Configuration.AutoDetectChangesEnabled = false;
                    container.Configuration.ValidateOnSaveEnabled = false;
                }
            }
            container.SaveChanges();
        }

        private static void UpdateJaEnTable()
        {
            var container = new DatabaseContext();
            container.Configuration.AutoDetectChangesEnabled = false;
            container.Configuration.ValidateOnSaveEnabled = false;

            var sqliteDb = new SQLiteConnection(@"C:\Users\CuHo\Desktop\Mazii\jaen.db");
            var list = sqliteDb.Table<jaen>().ToList();

            int count = 0;
            foreach (var w in list)
            {

                Console.WriteLine(w.id);
                container.JaEns.Add(new JaEn()
                {
                    Word = w.word,
                    Kana = w.phonetic,
                    Mean = w.mean

                });
                count++;
                if (count % Batch == 0)
                {
                    Console.WriteLine("saving");
                    container.SaveChanges();
                    container.Dispose();
                    container = new DatabaseContext();
                    container.Configuration.AutoDetectChangesEnabled = false;
                    container.Configuration.ValidateOnSaveEnabled = false;
                }
            }
            container.SaveChanges();
        }

        private static void UpdateJaViTable()
        {
            var container = new DatabaseContext();
            container.Configuration.AutoDetectChangesEnabled = false;
            container.Configuration.ValidateOnSaveEnabled = false;

            var sqliteDb = new SQLiteConnection(@"C:\Users\CuHo\Desktop\Mazii\Mazii VN\javn3.db");
            var list = sqliteDb.Table<javinew>().ToList();

            int count = 0;
            foreach (var w in list)
            {

                Console.WriteLine(w.id);
                container.JaVis.Add(new JaVi()
                {
                    Word = w.word,
                    Kana = w.kana,
                    Mean = w.mean

                });
                count++;
                if (count % Batch == 0)
                {
                    Console.WriteLine("saving");
                    container.SaveChanges();
                    container.Dispose();
                    container = new DatabaseContext();
                    container.Configuration.AutoDetectChangesEnabled = false;
                    container.Configuration.ValidateOnSaveEnabled = false;
                }
            }
            container.SaveChanges();
        }

        private static void UpdateJaViExamTable()
        {
            var container = new DatabaseContext();
            container.Configuration.AutoDetectChangesEnabled = false;
            container.Configuration.ValidateOnSaveEnabled = false;

            var sqliteDb = new SQLiteConnection(@"C:\Users\CuHo\Desktop\Mazii\Mazii VN\javn3.db");
            var list = sqliteDb.Table<examplenew>().ToList();

            int count = 0;
            foreach (var e in list)
            {

                Console.WriteLine(e.id);
                container.JaViExamples.Add(new JaViExample()
                {
                    Id = e.id,
                    Japanese = e.content,
                    VietNamese = e.mean
                });
                count++;
                if (count % Batch == 0)
                {
                    Console.WriteLine("saving");
                    container.SaveChanges();
                    container.Dispose();
                    container = new DatabaseContext();
                    container.Configuration.AutoDetectChangesEnabled = false;
                    container.Configuration.ValidateOnSaveEnabled = false;
                }
            }
            container.SaveChanges();
        }

        private static void UpdateViJaTable()
        {
            var container = new DatabaseContext();
            container.Configuration.AutoDetectChangesEnabled = false;
            container.Configuration.ValidateOnSaveEnabled = false;

            var sqliteDb = new SQLiteConnection(@"C:\Users\CuHo\Desktop\Mazii\Mazii VN\javn3.db");
            var list = sqliteDb.Table<vijanew>().ToList();

            int count = 0;
            foreach (var w in list)
            {

                Console.WriteLine(w.id);
                container.ViJas.Add(new ViJa()
                {
                    Word = w.word,
                    Mean = w.mean

                });
                count++;
                if (count % Batch == 0)
                {
                    Console.WriteLine("saving");
                    container.SaveChanges();
                    container.Dispose();
                    container = new DatabaseContext();
                    container.Configuration.AutoDetectChangesEnabled = false;
                    container.Configuration.ValidateOnSaveEnabled = false;
                }
            }
            container.SaveChanges();
        }
    }
}