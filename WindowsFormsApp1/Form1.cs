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
            try
            {
                int NumRun = 0;
                for (int i = 0; i < Program.MaxApp; i++)
                {
                    var applicationExists = (Program.IsProc[i]) ? Process.GetProcesses().Any(p => p.ProcessName.Contains(Program.appTitle[i])) : Program.FindWindow(null, Program.appTitle[i]) > 0 ;
                    Program.RUN[i] = applicationExists; // Приложение в работе или нет
                    if (listBox1.SelectedIndex == i)
                        textBoxPath.BackColor = (applicationExists) ? Color.GreenYellow : Color.Orange;

                    if (!applicationExists) // Приложение не работает
                    {
                        if (!Program.reStart[i])
                            LogHelper.Log($" Process {Program.appTitle[i]} is not running, start...", null, true);

                        if (Program.Attempt[i] < 3) // Если попыток меньше трех
                        {
                            Program.Attempt[i]++; // Новая попытка

                            // Запуск приложения
                            Process foo = new Process();
                            foo.StartInfo.FileName = Program.exePath[i];
                            foo.StartInfo.Arguments = Program.exeArgs[i];
                            foo.Start();
                            Program.reStart[i] = true;
                        }
                    }
                    else // Приложение работает
                    {
                        NumRun++;
                        Program.Attempt[i] = 0;
                        if (Program.reStart[i])
                        {
                            LogHelper.Log($" Process {Program.appTitle[i]} is running!", null, true);
                            Program.reStart[i] = false;
                        }

                    }
                }
                progressBar1.Value = NumRun;
                
            } catch (Exception ex)
            {
                textBoxPath.Text = ex.HResult.ToString()+" \n"+ex.Message;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
            int index = listBox1.SelectedIndex;
            if (index >= 0)
            {
                textBoxPath.Text = "( "+ ((Program.IsProc[index]) ? "process" : "window") +" ) "+ Program.exePath[index] + " " + Program.exeArgs[index];
                textBoxPath.BackColor = (Program.RUN[index]) ? Color.GreenYellow : Color.Orange;
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index >= 0)
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
                } else // Есть название окна
                {
                    hWnd = (IntPtr)Program.FindWindow(null, Program.appTitle[index]);
                    if (hWnd != IntPtr.Zero)
                    {
                        Program.ShowWindow(hWnd, 4); //выносим окно на передний план
                    }
                }

                

            }
        }
    }
}
