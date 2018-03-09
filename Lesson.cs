/****************************************************************
*                  Vocabulary Trainer 2018                      *
*                                                               *
*   Auth:    JESUS SALAZAR, jsalazar@saint.com.co               *
*   Date:    3/09/2018                                          *
*   Update:  3/09/2018                                          *
****************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Vocabulary
{
    class LessonVocabulary
    {
        enum Languaje
        {
            German,
            English
        };

        private Languaje ToLanguaje { get; set; } = Languaje.German;
        private Languaje FromLanguaje { get; set; } = Languaje.English;

        private  const  int NrTimes= 4;
        private string Answer { get; set; }
        private   int TotalCorrect { get;  set; }
        private   int TotalWrong { get;  set; }
        public static int TotalTimes { get; private set; }
        private int NrRounds { get; set; }
        private string FileCSV { get; set; }
        public void StartLesson(string fileLesson)
        {

            bool lessonOk = false;                 
            TotalCorrect = 0;
            TotalWrong = 0;
            TotalTimes = 0;
            NrRounds = 0;
            FileCSV = fileLesson;
            List<LessonRow> LessonVocabulary = File.ReadAllLines(FileCSV)
                                               .Skip(1)  //Skip header line of vocabulary lesson
                                               .Select(v => LessonRow.ParseLine(v ) )
                                              .ToList();
            lessonOk = LessonCompleted(LessonVocabulary);
            if (!lessonOk)
                lessonOk = DoQuestons(LessonVocabulary);
            if (lessonOk)
                ShowCongratulations();
        }//StartLesson

        private bool DoQuestons(List<LessonRow> lessonVocabulary)
        {
            Console.WriteLine("Vocabulary training started");
            Console.WriteLine(string.Format("Please enter the {0} translations, (Zero(0) to finish)\n", ToLanguaje.ToString()));
            do
            {
               NrRounds++;
                foreach (LessonRow words in lessonVocabulary)
                {
                    if (words.Count != NrTimes)
                    {
                        switch (ToLanguaje)
                        {
                            case Languaje.German:
                                {
                                    Console.WriteLine(string.Format("{0}: {1}", FromLanguaje, words.English));
                                    Console.Write(string.Format("{0}: ", ToLanguaje));
                                    break;
                                }
                            case Languaje.English:
                                {
                                    Console.WriteLine(string.Format("{0}: {1}", FromLanguaje, words.German));
                                    Console.Write(string.Format("{0}: ", ToLanguaje));
                                    break;
                                }
                        }
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Answer = Console.ReadLine().Trim().ToUpper();
                        Console.ResetColor();
                        if (Answer != "0")
                        {
                            if ((Answer == words.German.ToUpper() && ToLanguaje == Languaje.German) ||
                                (Answer == words.English.ToUpper() && ToLanguaje == Languaje.English))
                            {
                                Console.WriteLine("Correct");
                                words.Count += words.Count < NrTimes ? (1 + (words.Count == -1 ? 1 : 0)) : 0;
                                TotalCorrect += 1;
                                TotalTimes += 1;
                            }
                            else
                            {
                                Console.WriteLine(string.Format("Wrong. Correct is: {0}", words.German));
                                words.Count = (words.Count == 0 ? -1 : words.Count);
                                TotalWrong += 1;
                            }
                        }
                        else break;
                    }
                }
          } while (!LessonCompleted(lessonVocabulary) && Answer != "0");
          Console.WriteLine(string.Format("Total: {0}, Correct: {1}, Wrong: {2}", NrRounds, TotalCorrect, TotalWrong));
          SaveLesson(lessonVocabulary, FileCSV);
          return LessonCompleted(lessonVocabulary);
        }

        private bool LessonCompleted(List<LessonRow> lessonVocabulary)
        {
                return (TotalTimes == lessonVocabulary.Count * NrTimes);
        }

        private void ShowCongratulations()
        {
           Console.WriteLine("Congratulations.You successfully finished the lesson.");
        }

        private void SaveLesson(List<LessonRow> lessonVocabulary, string fileLesson)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileLesson, append: false))
                {
                    writer.WriteLine("German;English;Count");
                    foreach (LessonRow line in lessonVocabulary)
                    {
                        writer.WriteLine(string.Format("{0};{1};{2}", line.German,line.English,line.Count));
                    }
                    writer.Close();
                }
            }
            catch
            {
                Logger.Error("Writing to file " + fileLesson);                
            }
        }//SaveLesson

        class LessonRow
        {
            public string German;
            public string English;
            public int Count;
            public static LessonRow ParseLine(string txtLine )
            {
                string[] columns = txtLine.Split(';');
                LessonRow lRow = new LessonRow()
                {
                    German = columns[0].ToString().Trim(),
                    English = columns[1].ToString().Trim(),
                    Count = StrToInt(columns[2])
                };
                TotalTimes += ((lRow.Count > 0) ? lRow.Count : 0);
                return lRow;
            }
            private static int StrToInt(string stValue)
            {
                if (!Int32.TryParse(stValue, out int result))
                {
                    result = 0;
                }
                return result;
            }
        } //LessonRow
    }//LessonVocabulary
}
