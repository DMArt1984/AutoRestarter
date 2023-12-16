using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseInfo;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            int indexList = listBox1.SelectedIndex;
 
            int NumRun = 0;

            for (int index = 0; index < Program.MaxApp; index++)
            {
                try
                {
                    var applicationExists = (Program.IsProc[index]) ? Process.GetProcesses().Any(p => p.ProcessName.Contains(Program.appTitle[index])) : Program.FindWindow(null, Program.appTitle[index]) > 0 ;

                    //var x = Process.GetProcesses(); // для тестов

                    Program.RUN[index] = applicationExists; // Приложение в работе или нет
                    //if (listBox1.SelectedIndex == i)
                    //    textBoxPath.BackColor = (applicationExists) ? Color.GreenYellow : Color.Orange;

                    if (!applicationExists) // Приложение не работает
                    {
                        if (!Program.reStart[index])
                            LogHelper.Log($" Process {Program.appTitle[index]} is not running, start...", null, true);

                        if (Program.Attempt[index] < 3) // Если попыток меньше трех
                        {
                            Program.Attempt[index]++; // Новая попытка

                            // Запуск приложения
                            Process foo = new Process();
                            foo.StartInfo.FileName = Program.exePath[index];
                            foo.StartInfo.Arguments = Program.exeArgs[index];
                            foo.Start();
                            Program.reStart[index] = true;
                        } else
                        {
                            // Ошибка
                            if (Program.Fault[index] == null)
                                Program.Fault[index] = $"Приложение {Program.appTitle[index]} найти не удалось c {Program.Attempt[index]} попыток";
                        }
                    }
                    else // Приложение работает
                    {
                        NumRun++;
                        Program.Attempt[index] = 0;
                        if (Program.reStart[index])
                        {
                            LogHelper.Log($" Process {Program.appTitle[index]} is running!", null, true);
                            Program.reStart[index] = false;
                        }

                        // Ошибок нет
                        Program.Fault[index] = null;

                    }

                }
                catch (Exception ex)
                {
                    // Ошибка
                    Program.Fault[index] = ex.HResult.ToString() + " \n" + ex.Message;
                }

                // Обновить информацию на форме
                if (indexList == index)
                    Draw(indexList);
            }
            
            // Прогрессбар
            progressBar1.Value = NumRun;

            // Иконка
            if (Program.MaxApp <= 0)
            {
                notifyIcon1.Icon = DM_AutoRestarter.Properties.Resources.restart_blue;
                notifyIcon1.Text = "Не заданы приложения для контроля";
            }
            else
            {
                if (NumRun == 0)
                {
                    if (Program.Fault.Any(x => x != null))
                    {
                        notifyIcon1.Icon = DM_AutoRestarter.Properties.Resources.restart_red;
                        notifyIcon1.Text = "Ошибка запуска приложения";
                    }
                    else
                    {
                        notifyIcon1.Icon = DM_AutoRestarter.Properties.Resources.restart_black;
                        notifyIcon1.Text = "Приложения не запущены";
                    }
                }
                else if (NumRun == Program.MaxApp)
                {
                    notifyIcon1.Icon = DM_AutoRestarter.Properties.Resources.restart_green;
                    notifyIcon1.Text = "Все приложения запущены";
                }
                else
                {
                    notifyIcon1.Icon = DM_AutoRestarter.Properties.Resources.restart_yellow;
                    notifyIcon1.Text = "Приложения запущены частично";
                }
            }
            
            this.Icon = notifyIcon1.Icon;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // AssemblyVersion

            #region StartControl
            string procName = Process.GetCurrentProcess().ProcessName;
            int c = 0;
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.ProcessName.Contains(procName))
                {
                    c++;
                    if (c > 1)
                    {
                        MessageBox.Show("Уже запущена одна копия программы!", " ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        this.Close();
                    }
                }
            }
            #endregion

            //Starts minimized
            this.WindowState = FormWindowState.Minimized;
            // oth
            progressBar1.Maximum = Program.MaxApp;
            foreach (string item in Program.appTitle)
            {
                listBox1.Items.Add(item);
            }
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;

            textBoxInterval.Text = Program.WatchDog.ToString();
            timer1.Interval = Program.WatchDog;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                notifyIcon1.Visible = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            LogHelper.Log($"Closed: {e.CloseReason}", null, true);
        }

        private void textBoxInterval_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int indexList = listBox1.SelectedIndex;
            Draw(indexList);
        }

        private void Draw(int index)
        {
            if (index >= 0 && index < Program.MaxApp)
            {
                textBoxPath.Text = "( " + ((Program.IsProc[index]) ? "process" : "window") + " ) " + Program.exePath[index] + " " + Program.exeArgs[index];
                textBoxPath.BackColor = (Program.RUN[index]) ? Color.GreenYellow : Color.Orange;

                textBoxFault.Visible = Program.Fault[index] != null;
                textBoxFault.Text = Program.Fault[index];
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index >= 0)
            {
                if (Program.RUN[index]) // Если приложение работает, то
                {

                    IntPtr hWnd = IntPtr.Zero;
                    if (Program.IsProc[index]) // Есть процесс
                    {
                        Process[] pc = Process.GetProcessesByName(Program.appTitle[index]);
                        foreach (Process item in pc)
                        {
                            hWnd = item.MainWindowHandle;
                            //Console.WriteLine(Program.appTitle[index]+ " "+hWnd.ToString()+" "+ item.ToString());
                            if (hWnd != IntPtr.Zero)
                            {
                                Program.ShowWindow(hWnd, 4); //выносим окно на передний план
                            }
                        }
                    }
                    else // Есть название окна
                    {
                        hWnd = (IntPtr)Program.FindWindow(null, Program.appTitle[index]);
                        if (hWnd != IntPtr.Zero)
                        {
                            Program.ShowWindow(hWnd, 4); //выносим окно на передний план
                        }
                    }

                } else // Если приложение не работает, то
                {
                    Program.Attempt[index] = 0; // сбрасываем счетчик попыток
                    Program.Fault[index] = null; // сбрасываем ошибку, если была
                }

            }
        }

        private void versionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),"Версия приложения");
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
            WindowState = FormWindowState.Normal;
        }
    }
}
