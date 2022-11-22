using System;
using System.Text;
using System.IO;


namespace BaseInfo
{

    class LogHelper
    {
        private static object sync = new object();
        public static void Log(string Message="", Exception ex = null, bool writeConsole=false)
        {
            try
            {
                // Путь .\\Log
                string pathToLog = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log");
                if (!Directory.Exists(pathToLog))
                    Directory.CreateDirectory(pathToLog); // Создаем директорию, если нужно

                string filename = Path.Combine(pathToLog, string.Format("{0}_{1:dd.MM.yyy}.log", AppDomain.CurrentDomain.FriendlyName, DateTime.Now));
                string fullText = "";
                if (Message != "")
                {
                    fullText += string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] {1}\r\n", DateTime.Now, Message);
                }
                if (ex != null) {
                    fullText += string.Format("[{0:dd.MM.yyy HH:mm:ss.fff}] [{1}.{2}()] {3}\r\n", DateTime.Now, ex.TargetSite.DeclaringType, ex.TargetSite.Name, ex.Message);
                }

                if (writeConsole) // Пишем в консоль
                {
                    Console.WriteLine(Message);
                }

                lock (sync)
                {
                    File.AppendAllText(filename, fullText, Encoding.GetEncoding("Windows-1251"));
                }
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }

    }
}
