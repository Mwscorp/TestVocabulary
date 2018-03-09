/****************************************************************
*                  Vocabulary Trainer 2018                      *
*                                                               *
*   Auth:    JESUS SALAZAR, jsalazar@saint.com.co               *
*   Date:    3/09/2018                                          *
*   Update:  3/09/2018                                          *
****************************************************************/

using System;
using System.IO;

namespace Vocabulary
{
    class Program
    {
        static void Main(string[] args)
        {
            var fileLesson = args.Length > 0 ? args[0].ToString().ToUpper() : "";
            if (!fileLesson.Contains(".CSV"))
            {
                Console.WriteLine("Please indicate a valid lesson file name.\nExample:  Vocabulary Lesson1.CSV");
            }
            else
            {
                if (File.Exists(fileLesson))
                {
                    LessonVocabulary Lesson = new LessonVocabulary();
                    Lesson.StartLesson(fileLesson);
                }
                else
                {
                    Logger.Info(string.Format("Lesson file {0} does not exist !", fileLesson));
                }
            }
        }
    }
}
