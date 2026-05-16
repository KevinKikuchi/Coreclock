namespace Coreclock
{
    partial class EmployeeDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeDashboard));
            panel1 = new Panel();
            ReportsBtn = new Button();
            LogOutBtn = new Button();
            ProfilePanel = new Panel();
            MyDashboardBtn = new Button();
            label12 = new Label();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            AdjustLbl = new Label();
            label3 = new Label();
            DateTimePicker = new DateTimePicker();
            TimePanel = new Panel();
            ShiftOutBtn = new Label();
            ShiftInBtn = new Label();
            ShiftScheduleLbl = new Label();
            label2 = new Label();
            label4 = new Label();
            TimeOutBtn = new Button();
            TimeInBtn = new Button();
            CurrentTime = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            TimeInPanel = new Panel();
            label6 = new Label();
            label5 = new Label();
            TimeOutPanel = new Panel();
            label7 = new Label();
            label8 = new Label();
            HoursPanel = new Panel();
            HoursTodayLbl = new Label();
            label10 = new Label();
            StatusPanel = new Panel();
            StatusLbl = new Label();
            label11 = new Label();
            LogsDataGridView = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            TimePanel.SuspendLayout();
            TimeInPanel.SuspendLayout();
            TimeOutPanel.SuspendLayout();
            HoursPanel.SuspendLayout();
            StatusPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LogsDataGridView).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(31, 31, 31);
            panel1.Controls.Add(ReportsBtn);
            panel1.Controls.Add(LogOutBtn);
            panel1.Controls.Add(ProfilePanel);
            panel1.Controls.Add(MyDashboardBtn);
            panel1.Controls.Add(label12);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(-3, -2);
            panel1.Name = "panel1";
            panel1.Size = new Size(220, 601);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // ReportsBtn
            // 
            ReportsBtn.FlatStyle = FlatStyle.Flat;
            ReportsBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ReportsBtn.ForeColor = Color.Black;
            ReportsBtn.Location = new Point(15, 197);
            ReportsBtn.Name = "ReportsBtn";
            ReportsBtn.Size = new Size(188, 32);
            ReportsBtn.TabIndex = 11;
            ReportsBtn.Text = "Reports";
            ReportsBtn.UseVisualStyleBackColor = true;
            ReportsBtn.Click += button1_Click_1;
            // 
            // LogOutBtn
            // 
            LogOutBtn.FlatStyle = FlatStyle.Flat;
            LogOutBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LogOutBtn.ForeColor = SystemColors.ButtonFace;
            LogOutBtn.Location = new Point(35, 556);
            LogOutBtn.Name = "LogOutBtn";
            LogOutBtn.Size = new Size(139, 32);
            LogOutBtn.TabIndex = 10;
            LogOutBtn.Text = "↩ Logout";
            LogOutBtn.UseVisualStyleBackColor = true;
            LogOutBtn.Click += LogOutBtn_Click;
            // 
            // ProfilePanel
            // 
            ProfilePanel.BackColor = Color.FromArgb(20, 20, 20);
            ProfilePanel.Location = new Point(15, 343);
            ProfilePanel.Name = "ProfilePanel";
            ProfilePanel.Size = new Size(188, 207);
            ProfilePanel.TabIndex = 9;
            ProfilePanel.Paint += panel2_Paint_2;
            // 
            // MyDashboardBtn
            // 
            MyDashboardBtn.FlatStyle = FlatStyle.Flat;
            MyDashboardBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            MyDashboardBtn.ForeColor = Color.Black;
            MyDashboardBtn.Location = new Point(15, 146);
            MyDashboardBtn.Name = "MyDashboardBtn";
            MyDashboardBtn.Size = new Size(188, 32);
            MyDashboardBtn.TabIndex = 8;
            MyDashboardBtn.Text = "My Dashboard\n";
            MyDashboardBtn.UseVisualStyleBackColor = true;
            MyDashboardBtn.Click += MyDashboardBtn_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.BackColor = Color.Transparent;
            label12.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label12.ForeColor = Color.Gray;
            label12.Location = new Point(-5, 112);
            label12.Name = "label12";
            label12.Size = new Size(237, 15);
            label12.TabIndex = 7;
            label12.Text = "______________________________________________";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(-3, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(220, 112);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Verdana", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(236, 9);
            label1.Name = "label1";
            label1.Size = new Size(109, 38);
            label1.TabIndex = 1;
            label1.Text = "Good";
            // 
            // AdjustLbl
            // 
            AdjustLbl.AutoSize = true;
            AdjustLbl.Font = new Font("Verdana", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AdjustLbl.ForeColor = Color.Goldenrod;
            AdjustLbl.Location = new Point(335, 9);
            AdjustLbl.Name = "AdjustLbl";
            AdjustLbl.Size = new Size(177, 38);
            AdjustLbl.TabIndex = 2;
            AdjustLbl.Text = "Morning!";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Gray;
            label3.Location = new Point(239, 47);
            label3.Name = "label3";
            label3.Size = new Size(186, 15);
            label3.TabIndex = 3;
            label3.Text = "Welcome back to your dashboard";
            // 
            // DateTimePicker
            // 
            DateTimePicker.CalendarForeColor = Color.IndianRed;
            DateTimePicker.CalendarMonthBackground = SystemColors.InactiveCaption;
            DateTimePicker.Location = new Point(239, 64);
            DateTimePicker.Name = "DateTimePicker";
            DateTimePicker.Size = new Size(200, 23);
            DateTimePicker.TabIndex = 4;
            DateTimePicker.ValueChanged += dateTimePicker1_ValueChanged;
            // 
            // TimePanel
            // 
            TimePanel.BackColor = Color.FromArgb(31, 31, 31);
            TimePanel.Controls.Add(ShiftOutBtn);
            TimePanel.Controls.Add(ShiftInBtn);
            TimePanel.Controls.Add(ShiftScheduleLbl);
            TimePanel.Controls.Add(label2);
            TimePanel.Controls.Add(label4);
            TimePanel.Controls.Add(TimeOutBtn);
            TimePanel.Controls.Add(TimeInBtn);
            TimePanel.Controls.Add(CurrentTime);
            TimePanel.Location = new Point(236, 93);
            TimePanel.Name = "TimePanel";
            TimePanel.Size = new Size(520, 124);
            TimePanel.TabIndex = 5;
            TimePanel.Paint += panel2_Paint;
            // 
            // ShiftOutBtn
            // 
            ShiftOutBtn.AutoSize = true;
            ShiftOutBtn.ForeColor = Color.Red;
            ShiftOutBtn.Location = new Point(411, 51);
            ShiftOutBtn.Name = "ShiftOutBtn";
            ShiftOutBtn.Size = new Size(109, 15);
            ShiftOutBtn.TabIndex = 10;
            ShiftOutBtn.Text = "Time Out: 4:00 AM";
            // 
            // ShiftInBtn
            // 
            ShiftInBtn.AutoSize = true;
            ShiftInBtn.ForeColor = Color.GreenYellow;
            ShiftInBtn.Location = new Point(411, 36);
            ShiftInBtn.Name = "ShiftInBtn";
            ShiftInBtn.Size = new Size(102, 15);
            ShiftInBtn.TabIndex = 9;
            ShiftInBtn.Text = " Time In: 8:00 PM";
            // 
            // ShiftScheduleLbl
            // 
            ShiftScheduleLbl.AutoSize = true;
            ShiftScheduleLbl.ForeColor = Color.Gray;
            ShiftScheduleLbl.Location = new Point(418, 17);
            ShiftScheduleLbl.Name = "ShiftScheduleLbl";
            ShiftScheduleLbl.Size = new Size(94, 15);
            ShiftScheduleLbl.TabIndex = 8;
            ShiftScheduleLbl.Text = "Monday - Friday";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(418, 2);
            label2.Name = "label2";
            label2.Size = new Size(59, 15);
            label2.TabIndex = 7;
            label2.Text = "Your Shift";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Gray;
            label4.Location = new Point(9, 17);
            label4.Name = "label4";
            label4.Size = new Size(78, 15);
            label4.TabIndex = 6;
            label4.Text = "TIME PUNCH";
            // 
            // TimeOutBtn
            // 
            TimeOutBtn.FlatStyle = FlatStyle.Flat;
            TimeOutBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TimeOutBtn.ForeColor = SystemColors.ButtonFace;
            TimeOutBtn.Location = new Point(270, 73);
            TimeOutBtn.Name = "TimeOutBtn";
            TimeOutBtn.Size = new Size(242, 32);
            TimeOutBtn.TabIndex = 4;
            TimeOutBtn.Text = "Time Out";
            TimeOutBtn.UseVisualStyleBackColor = true;
            TimeOutBtn.Click += TimeOutBtn_Click;
            // 
            // TimeInBtn
            // 
            TimeInBtn.FlatStyle = FlatStyle.Flat;
            TimeInBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            TimeInBtn.ForeColor = SystemColors.ButtonFace;
            TimeInBtn.Location = new Point(9, 73);
            TimeInBtn.Name = "TimeInBtn";
            TimeInBtn.Size = new Size(242, 32);
            TimeInBtn.TabIndex = 3;
            TimeInBtn.Text = "Time In";
            TimeInBtn.UseVisualStyleBackColor = true;
            TimeInBtn.Click += TimeInBtn_Click;
            // 
            // CurrentTime
            // 
            CurrentTime.AutoSize = true;
            CurrentTime.Font = new Font("Verdana", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CurrentTime.ForeColor = Color.Goldenrod;
            CurrentTime.Location = new Point(3, 32);
            CurrentTime.Name = "CurrentTime";
            CurrentTime.Size = new Size(158, 38);
            CurrentTime.TabIndex = 2;
            CurrentTime.Text = "0:00:00";
            // 
            // TimeInPanel
            // 
            TimeInPanel.BackColor = Color.FromArgb(31, 31, 31);
            TimeInPanel.Controls.Add(label6);
            TimeInPanel.Controls.Add(label5);
            TimeInPanel.Location = new Point(236, 238);
            TimeInPanel.Name = "TimeInPanel";
            TimeInPanel.Size = new Size(117, 56);
            TimeInPanel.TabIndex = 6;
            TimeInPanel.Paint += panel2_Paint_1;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Arial", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.GreenYellow;
            label6.Location = new Point(3, 26);
            label6.Name = "label6";
            label6.Size = new Size(84, 24);
            label6.TabIndex = 7;
            label6.Text = "0:00:00";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.ForeColor = Color.Gray;
            label5.Location = new Point(3, 11);
            label5.Name = "label5";
            label5.Size = new Size(48, 15);
            label5.TabIndex = 4;
            label5.Text = "Time In";
            // 
            // TimeOutPanel
            // 
            TimeOutPanel.BackColor = Color.FromArgb(31, 31, 31);
            TimeOutPanel.Controls.Add(label7);
            TimeOutPanel.Controls.Add(label8);
            TimeOutPanel.Location = new Point(370, 238);
            TimeOutPanel.Name = "TimeOutPanel";
            TimeOutPanel.Size = new Size(117, 56);
            TimeOutPanel.TabIndex = 7;
            TimeOutPanel.Paint += TimeOutPanel_Paint;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Arial", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.Red;
            label7.Location = new Point(3, 26);
            label7.Name = "label7";
            label7.Size = new Size(81, 24);
            label7.TabIndex = 7;
            label7.Text = "Not Yet";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.ForeColor = Color.Gray;
            label8.Location = new Point(3, 11);
            label8.Name = "label8";
            label8.Size = new Size(57, 15);
            label8.TabIndex = 4;
            label8.Text = "Time Out";
            // 
            // HoursPanel
            // 
            HoursPanel.BackColor = Color.FromArgb(31, 31, 31);
            HoursPanel.Controls.Add(HoursTodayLbl);
            HoursPanel.Controls.Add(label10);
            HoursPanel.Location = new Point(503, 238);
            HoursPanel.Name = "HoursPanel";
            HoursPanel.Size = new Size(117, 56);
            HoursPanel.TabIndex = 8;
            HoursPanel.Paint += HoursPanel_Paint;
            // 
            // HoursTodayLbl
            // 
            HoursTodayLbl.AutoSize = true;
            HoursTodayLbl.BackColor = Color.Transparent;
            HoursTodayLbl.Font = new Font("Arial", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            HoursTodayLbl.ForeColor = Color.Goldenrod;
            HoursTodayLbl.Location = new Point(3, 26);
            HoursTodayLbl.Name = "HoursTodayLbl";
            HoursTodayLbl.Size = new Size(84, 24);
            HoursTodayLbl.TabIndex = 7;
            HoursTodayLbl.Text = "6h 23m";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Transparent;
            label10.ForeColor = Color.Gray;
            label10.Location = new Point(3, 11);
            label10.Name = "label10";
            label10.Size = new Size(74, 15);
            label10.TabIndex = 4;
            label10.Text = "Hours Today";
            // 
            // StatusPanel
            // 
            StatusPanel.BackColor = Color.FromArgb(31, 31, 31);
            StatusPanel.Controls.Add(StatusLbl);
            StatusPanel.Controls.Add(label11);
            StatusPanel.Location = new Point(639, 238);
            StatusPanel.Name = "StatusPanel";
            StatusPanel.Size = new Size(117, 56);
            StatusPanel.TabIndex = 9;
            StatusPanel.Paint += StatusPanel_Paint;
            // 
            // StatusLbl
            // 
            StatusLbl.AutoSize = true;
            StatusLbl.BackColor = Color.Transparent;
            StatusLbl.Font = new Font("Arial", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            StatusLbl.ForeColor = Color.GreenYellow;
            StatusLbl.Location = new Point(3, 26);
            StatusLbl.Name = "StatusLbl";
            StatusLbl.Size = new Size(88, 24);
            StatusLbl.TabIndex = 7;
            StatusLbl.Text = "Present";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = Color.Transparent;
            label11.ForeColor = Color.Gray;
            label11.Location = new Point(3, 11);
            label11.Name = "label11";
            label11.Size = new Size(40, 15);
            label11.TabIndex = 4;
            label11.Text = "Status";
            // 
            // LogsDataGridView
            // 
            LogsDataGridView.BackgroundColor = Color.FromArgb(31, 31, 31);
            LogsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            LogsDataGridView.Location = new Point(236, 310);
            LogsDataGridView.Name = "LogsDataGridView";
            LogsDataGridView.Size = new Size(520, 276);
            LogsDataGridView.TabIndex = 10;
            // 
            // EmployeeDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(775, 598);
            Controls.Add(LogsDataGridView);
            Controls.Add(StatusPanel);
            Controls.Add(HoursPanel);
            Controls.Add(TimeOutPanel);
            Controls.Add(TimeInPanel);
            Controls.Add(TimePanel);
            Controls.Add(DateTimePicker);
            Controls.Add(label3);
            Controls.Add(AdjustLbl);
            Controls.Add(label1);
            Controls.Add(panel1);
            Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EmployeeDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EmployeeDashboard";
            Load += EmployeeDashboard_Load_1;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            TimePanel.ResumeLayout(false);
            TimePanel.PerformLayout();
            TimeInPanel.ResumeLayout(false);
            TimeInPanel.PerformLayout();
            TimeOutPanel.ResumeLayout(false);
            TimeOutPanel.PerformLayout();
            HoursPanel.ResumeLayout(false);
            HoursPanel.PerformLayout();
            StatusPanel.ResumeLayout(false);
            StatusPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)LogsDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private PictureBox pictureBox1;
        private Label label1;
        private Label AdjustLbl;
        private Label label3;
        private DateTimePicker DateTimePicker;
        private Panel TimePanel;
        private Label CurrentTime;
        private Button TimeOutBtn;
        private Button TimeInBtn;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Label label4;
        private Panel TimeInPanel;
        private Label label5;
        private Label label6;
        private Panel StatusPanel;
        private Label label7;
        private Label label8;
        private Panel TimeOutPanel;
        private Panel HoursPanel;
        private Label HoursTodayLbl;
        private Label label10;
        private Label StatusLbl;
        private Label label11;
        private DataGridView LogsDataGridView;
        private Label label12;
        private Button MyDashboardBtn;
        private Panel ProfilePanel;
        private Button LogOutBtn;
        private Label label2;
        private Label ShiftScheduleLbl;
        private Label ShiftInBtn;
        private Label ShiftOutBtn;
        private Button ReportsBtn;
    }
}