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
        static public List<bool> IsProc = new List<bool>();
        static public List<ushort> Attempt = new List<ushort>();
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
            // Загрузка параметров
            try
            {
                bool SetWG = false;
                foreach (string line in File.ReadLines("sett.txt"))
                {
                    string[] args = line.Split(';');
                    if (args.Length < 2)
                        continue;
                    if (String.IsNullOrWhiteSpace(args[0]) || String.IsNullOrWhiteSpace(args[1]))
                        continue;

                    var Proc = false;
                    if (args[0].Substring(0,1) == ":")
                    {
                        Proc = true;
                        args[0] = args[0].Substring(1);
                    }
                    IsProc.Add(Proc); // Процесс (или окно)

                    var Title = args[0];
                    appTitle.Add(Title); // Название процесса или окна

                    var Path = (args[1].IndexOf(":") != 1 && args[1].IndexOf("\\\\") != 0) ? Application.StartupPath + "\\" + args[1] : args[1];
                    exePath.Add(Path); // Путь к файлу

                    var Args = (args.Length >= 3 && !String.IsNullOrWhiteSpace(args[2])) ? args[2] : "";
                    exeArgs.Add(Args); // Аргументы

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
                    RUN.Add(false); // Приложение в работе

                    MaxApp++;
                    LogHelper.Log($"--- {MaxApp}) Params: {Title} - {Path} {Args}", null, true);
                }
                LogHelper.Log($"WatchDog: {WatchDog}", null, true);
            }
            catch (Exception ex)
            {
                LogHelper.Log($"ERROR Params", ex, true);
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
                LogHelper.Log($"ERROR Application", ex, true);
                MessageBox.Show(ex.Message);
            }
        }
    }
}
