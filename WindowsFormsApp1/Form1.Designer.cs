namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.textBoxInterval = new System.Windows.Forms.TextBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 3000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Location = new System.Drawing.Point(12, 122);
            this.textBoxPath.Multiline = true;
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.ReadOnly = true;
            this.textBoxPath.Size = new System.Drawing.Size(324, 57);
            this.textBoxPath.TabIndex = 1;
            this.textBoxPath.TabStop = false;
            this.textBoxPath.Text = "C:\\Proj\\ConsoleOPC_UA_88\\ConsoleOPC_UA\\bin\\Debug\\TagsConnector.exe";
            // 
            // textBoxInterval
            // 
            this.textBoxInterval.Location = new System.Drawing.Point(12, 192);
            this.textBoxInterval.Name = "textBoxInterval";
            this.textBoxInterval.ReadOnly = true;
            this.textBoxInterval.Size = new System.Drawing.Size(71, 20);
            this.textBoxInterval.TabIndex = 2;
            this.textBoxInterval.TabStop = false;
            this.textBoxInterval.TextChanged += new System.EventHandler(this.textBoxInterval_TextChanged);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Auto Restart process";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(12, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(324, 109);
            this.listBox1.TabIndex = 3;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.DoubleClick += new System.EventHandler(this.listBox1_DoubleClick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(89, 192);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(247, 20);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 220);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.textBoxInterval);
            this.Controls.Add(this.textBoxPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "AutoRestarter";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.TextBox textBoxInterval;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}

