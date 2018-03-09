using System;
using System.IO;

namespace Vocabulary
{
    public static class Logger
    {
        private static string FileLog = "Vocabulary.log";
        public static void Info(string msgTxt)
        {
            Log("Info", msgTxt);
        }

        public static void  Error(string msgTxt)
        {
            Log("Error", msgTxt);
        }
        private static void Log(string typeMsg, string msgTxt)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FileLog, append: true))
                {
                    writer.WriteLine(string.Format("{0}: {1}-{2}", DateTime.Now, typeMsg, msgTxt));
                    if (typeMsg == "Info")
                        Console.WriteLine(msgTxt);
                }
            }
            catch
            {
                Console.WriteLine("\nError writing to file " + FileLog);
            }
        }
    }
}
