using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BaseInfo;
using System.Runtime.InteropServices;

namespace WindowsFormsApp1
{
    static class Program
    {
        static public int WatchDog = 3000; // Интервал проверки (мсек)
        static public List<string> appTitle = new List<string>(); // 
        static public List<string> exePath = new List<string>(); // 
        static public List<string> exeArgs = new List<string>(); // 
        static public List<bool> reStart = new List<bool>(); // 
        static public List<bool> RUN = new List<bool>(); // 
        static public List<bool> IsProc = new List<bool>(); //
        static public List<ushort> Attempt = new List<ushort>(); //
        static public List<string> Fault = new List<string>(); //
        static public int MaxApp = 0;


        [DllImport("user32.dll")]
        public static extern int FindWindow(
            string lpClassName, // class name 
            string lpWindowName // window name 
        );

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LogHelper.Log("===============================================");

            // Загрузка параметров
            try
            {
                bool SetWG = false;
                LogHelper.Log("Загрузка параметров:");
                foreach (string line in File.ReadLines("sett.txt")) // перебор файла по строкам
                {
                    LogHelper.Log("--- "+line);
                    string[] args = line.Split(';'); // получение параметров строки

                    if (args.Length < 2) // если параметров мало, то
                        continue;
                    if (String.IsNullOrWhiteSpace(args[0]) || String.IsNullOrWhiteSpace(args[1])) // если параметры пустые, то 
                        continue;

                    // Что будем искать, процесс или окно
                    var Proc = false;
                    if (args[0].Substring(0,1) == ":") // для процесса ставим ":" перед названием
                    {
                        Proc = true;
                        args[0] = args[0].Substring(1);
                    }
                    IsProc.Add(Proc); // Процесс (или окно)

                    // Название процесса или окна
                    var Title = args[0];
                    appTitle.Add(Title); // Название процесса или окна

                    // Путь к файлу приложения для запуска
                    var Path = (args[1].IndexOf(":") != 1 && args[1].IndexOf("\\\\") != 0) ? Application.StartupPath + "\\" + args[1] : args[1];
                    exePath.Add(Path); // Путь к файлу

                    // Аргументы приложения
                    var Args = (args.Length >= 3 && !String.IsNullOrWhiteSpace(args[2])) ? args[2] : "";
                    exeArgs.Add(Args); // Аргументы

                    // Интервал проверки запущено ли заданное приложение
                    // Для всех приложений интервал один
                    if (args.Length >= 4 && int.Parse(args[3]) >= 1000)
                    {
                        if (SetWG == false)
                        {
                            WatchDog = int.Parse(args[3]); // Интервал проверки
                            SetWG = true;
                        }
                        else
                        {
                            WatchDog = Math.Min(int.Parse(args[3]), WatchDog); // Интервал проверки
                        }
                    }

                    reStart.Add(false); // Значение для рестарта
                    Attempt.Add(0); // Попыток запуска
                    Fault.Add(null); // Текст ошибки
                    RUN.Add(false); // Приложение в работе

                    MaxApp++;
                    LogHelper.Log($"    {MaxApp}) Параметры: {Title} - {Path} {Args}", null, true);
                }
                LogHelper.Log($"Периодичность: {WatchDog}", null, true);
            }
            catch (Exception ex)
            {
                LogHelper.Log($"Ошибка параметров: ", ex, true);
                MessageBox.Show(ex.Message);
                return;
            }

            // Запуск формы
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            } catch (Exception ex)
            {
                LogHelper.Log($"Ошибка приложения: ", ex, true);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
