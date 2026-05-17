namespace Coreclock
{
    partial class AdminDashboard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdminDashboard));
            panel1 = new Panel();
            ProfilePanel = new Panel();
            AttendanceLogsBtn = new Button();
            ReportBtn = new Button();
            EmployeBtn = new Button();
            LogOutBtn = new Button();
            AdminDashboardBtn = new Button();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            AdjustLbl = new Label();
            label3 = new Label();
            label2 = new Label();
            DateTimePicker = new DateTimePicker();
            CurrentTime = new Label();
            TotalEmployeePanel = new Panel();
            label6 = new Label();
            label5 = new Label();
            PresentTodayPanel = new Panel();
            label4 = new Label();
            label7 = new Label();
            AbsentTodayPanel = new Panel();
            label8 = new Label();
            label9 = new Label();
            LateTodayPanel = new Panel();
            label10 = new Label();
            label11 = new Label();
            label13 = new Label();
            EmployeeDataGridView = new DataGridView();
            AttendanceLogsDataGridView = new DataGridView();
            label12 = new Label();
            ReportPanel = new Panel();
            label14 = new Label();
            label15 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            TotalEmployeePanel.SuspendLayout();
            PresentTodayPanel.SuspendLayout();
            AbsentTodayPanel.SuspendLayout();
            LateTodayPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)EmployeeDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AttendanceLogsDataGridView).BeginInit();
            ReportPanel.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(31, 31, 31);
            panel1.Controls.Add(ProfilePanel);
            panel1.Controls.Add(AttendanceLogsBtn);
            panel1.Controls.Add(ReportBtn);
            panel1.Controls.Add(EmployeBtn);
            panel1.Controls.Add(LogOutBtn);
            panel1.Controls.Add(AdminDashboardBtn);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(-3, -2);
            panel1.Name = "panel1";
            panel1.Size = new Size(217, 724);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            // 
            // ProfilePanel
            // 
            ProfilePanel.BackColor = Color.FromArgb(20, 20, 20);
            ProfilePanel.Location = new Point(14, 423);
            ProfilePanel.Name = "ProfilePanel";
            ProfilePanel.Size = new Size(188, 223);
            ProfilePanel.TabIndex = 15;
            // 
            // AttendanceLogsBtn
            // 
            AttendanceLogsBtn.FlatStyle = FlatStyle.Flat;
            AttendanceLogsBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AttendanceLogsBtn.ForeColor = SystemColors.ButtonFace;
            AttendanceLogsBtn.Location = new Point(15, 254);
            AttendanceLogsBtn.Name = "AttendanceLogsBtn";
            AttendanceLogsBtn.Size = new Size(188, 32);
            AttendanceLogsBtn.TabIndex = 14;
            AttendanceLogsBtn.Text = "Attendance Logs";
            AttendanceLogsBtn.UseVisualStyleBackColor = true;
            AttendanceLogsBtn.Click += AttendanceLogsBtn_Click;
            // 
            // ReportBtn
            // 
            ReportBtn.FlatStyle = FlatStyle.Flat;
            ReportBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ReportBtn.ForeColor = SystemColors.ButtonFace;
            ReportBtn.Location = new Point(15, 312);
            ReportBtn.Name = "ReportBtn";
            ReportBtn.Size = new Size(188, 32);
            ReportBtn.TabIndex = 13;
            ReportBtn.Text = "Reports";
            ReportBtn.UseVisualStyleBackColor = true;
            ReportBtn.Click += ReportBtn_Click;
            // 
            // EmployeBtn
            // 
            EmployeBtn.FlatStyle = FlatStyle.Flat;
            EmployeBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            EmployeBtn.ForeColor = SystemColors.ButtonFace;
            EmployeBtn.Location = new Point(15, 200);
            EmployeBtn.Name = "EmployeBtn";
            EmployeBtn.Size = new Size(188, 32);
            EmployeBtn.TabIndex = 11;
            EmployeBtn.Text = " Employees Schedule";
            EmployeBtn.UseVisualStyleBackColor = true;
            EmployeBtn.Click += EmployeBtn_Click;
            // 
            // LogOutBtn
            // 
            LogOutBtn.FlatStyle = FlatStyle.Flat;
            LogOutBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LogOutBtn.ForeColor = SystemColors.ButtonFace;
            LogOutBtn.Location = new Point(15, 675);
            LogOutBtn.Name = "LogOutBtn";
            LogOutBtn.Size = new Size(188, 32);
            LogOutBtn.TabIndex = 10;
            LogOutBtn.Text = "↩ Logout";
            LogOutBtn.UseVisualStyleBackColor = true;
            LogOutBtn.Click += LogOutBtn_Click;
            // 
            // AdminDashboardBtn
            // 
            AdminDashboardBtn.FlatStyle = FlatStyle.Flat;
            AdminDashboardBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AdminDashboardBtn.ForeColor = SystemColors.ButtonFace;
            AdminDashboardBtn.Location = new Point(15, 146);
            AdminDashboardBtn.Name = "AdminDashboardBtn";
            AdminDashboardBtn.Size = new Size(188, 32);
            AdminDashboardBtn.TabIndex = 8;
            AdminDashboardBtn.Text = "Dashboard";
            AdminDashboardBtn.UseVisualStyleBackColor = true;
            AdminDashboardBtn.Click += MyDashboardBtn_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(3, 17);
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
            label1.Location = new Point(220, 9);
            label1.Name = "label1";
            label1.Size = new Size(109, 38);
            label1.TabIndex = 2;
            label1.Text = "Good";
            // 
            // AdjustLbl
            // 
            AdjustLbl.AutoSize = true;
            AdjustLbl.Font = new Font("Verdana", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AdjustLbl.ForeColor = Color.Goldenrod;
            AdjustLbl.Location = new Point(315, 9);
            AdjustLbl.Name = "AdjustLbl";
            AdjustLbl.Size = new Size(177, 38);
            AdjustLbl.TabIndex = 3;
            AdjustLbl.Text = "Morning!";
            AdjustLbl.Click += AdjustLbl_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Gray;
            label3.Location = new Point(226, 47);
            label3.Name = "label3";
            label3.Size = new Size(127, 15);
            label3.TabIndex = 4;
            label3.Text = "Welcome back, Admin";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(220, 62);
            label2.Name = "label2";
            label2.Size = new Size(1277, 15);
            label2.TabIndex = 8;
            label2.Text = resources.GetString("label2.Text");
            // 
            // DateTimePicker
            // 
            DateTimePicker.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            DateTimePicker.CalendarForeColor = Color.IndianRed;
            DateTimePicker.CalendarMonthBackground = SystemColors.InactiveCaption;
            DateTimePicker.Location = new Point(1293, 47);
            DateTimePicker.Name = "DateTimePicker";
            DateTimePicker.Size = new Size(200, 23);
            DateTimePicker.TabIndex = 9;
            // 
            // CurrentTime
            // 
            CurrentTime.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CurrentTime.AutoSize = true;
            CurrentTime.Font = new Font("Verdana", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CurrentTime.ForeColor = Color.Goldenrod;
            CurrentTime.Location = new Point(1379, 22);
            CurrentTime.Name = "CurrentTime";
            CurrentTime.Size = new Size(96, 23);
            CurrentTime.TabIndex = 10;
            CurrentTime.Text = "0:00:00";
            // 
            // TotalEmployeePanel
            // 
            TotalEmployeePanel.BackColor = Color.FromArgb(31, 31, 31);
            TotalEmployeePanel.Controls.Add(label6);
            TotalEmployeePanel.Controls.Add(label5);
            TotalEmployeePanel.Location = new Point(236, 99);
            TotalEmployeePanel.Name = "TotalEmployeePanel";
            TotalEmployeePanel.Size = new Size(128, 60);
            TotalEmployeePanel.TabIndex = 11;
            TotalEmployeePanel.Paint += TimeInPanel_Paint;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Arial", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.Goldenrod;
            label6.Location = new Point(3, 4);
            label6.Name = "label6";
            label6.Size = new Size(34, 24);
            label6.TabIndex = 7;
            label6.Text = "24";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.ForeColor = Color.Gray;
            label5.Location = new Point(3, 35);
            label5.Name = "label5";
            label5.Size = new Size(93, 15);
            label5.TabIndex = 4;
            label5.Text = "Total Employees";
            // 
            // PresentTodayPanel
            // 
            PresentTodayPanel.BackColor = Color.FromArgb(31, 31, 31);
            PresentTodayPanel.Controls.Add(label4);
            PresentTodayPanel.Controls.Add(label7);
            PresentTodayPanel.Location = new Point(384, 99);
            PresentTodayPanel.Name = "PresentTodayPanel";
            PresentTodayPanel.Size = new Size(117, 60);
            PresentTodayPanel.TabIndex = 12;
            PresentTodayPanel.Paint += PresentTodayPanel_Paint;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Arial", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.Goldenrod;
            label4.Location = new Point(3, 4);
            label4.Name = "label4";
            label4.Size = new Size(34, 24);
            label4.TabIndex = 7;
            label4.Text = "18";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.ForeColor = Color.Gray;
            label7.Location = new Point(3, 35);
            label7.Name = "label7";
            label7.Size = new Size(81, 15);
            label7.TabIndex = 4;
            label7.Text = "Present Today";
            // 
            // AbsentTodayPanel
            // 
            AbsentTodayPanel.BackColor = Color.FromArgb(31, 31, 31);
            AbsentTodayPanel.Controls.Add(label8);
            AbsentTodayPanel.Controls.Add(label9);
            AbsentTodayPanel.Location = new Point(524, 99);
            AbsentTodayPanel.Name = "AbsentTodayPanel";
            AbsentTodayPanel.Size = new Size(128, 60);
            AbsentTodayPanel.TabIndex = 13;
            AbsentTodayPanel.Paint += AbsentTodayPanel_Paint;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.BackColor = Color.Transparent;
            label8.Font = new Font("Arial", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.Crimson;
            label8.Location = new Point(3, 4);
            label8.Name = "label8";
            label8.Size = new Size(22, 24);
            label8.TabIndex = 7;
            label8.Text = "4";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = Color.Transparent;
            label9.ForeColor = Color.Gray;
            label9.Location = new Point(3, 35);
            label9.Name = "label9";
            label9.Size = new Size(79, 15);
            label9.TabIndex = 4;
            label9.Text = "Absent Today";
            // 
            // LateTodayPanel
            // 
            LateTodayPanel.BackColor = Color.FromArgb(31, 31, 31);
            LateTodayPanel.Controls.Add(label10);
            LateTodayPanel.Controls.Add(label11);
            LateTodayPanel.Location = new Point(674, 99);
            LateTodayPanel.Name = "LateTodayPanel";
            LateTodayPanel.Size = new Size(136, 60);
            LateTodayPanel.TabIndex = 14;
            LateTodayPanel.Paint += LateTodayPanel_Paint;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = Color.Transparent;
            label10.Font = new Font("Arial", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.ForeColor = Color.Goldenrod;
            label10.Location = new Point(3, 4);
            label10.Name = "label10";
            label10.Size = new Size(22, 24);
            label10.TabIndex = 7;
            label10.Text = "2";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.BackColor = Color.Transparent;
            label11.ForeColor = Color.Gray;
            label11.Location = new Point(3, 35);
            label11.Name = "label11";
            label11.Size = new Size(64, 15);
            label11.TabIndex = 4;
            label11.Text = "Late Today";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label13.ForeColor = Color.Gray;
            label13.Location = new Point(236, 175);
            label13.Name = "label13";
            label13.Size = new Size(109, 21);
            label13.TabIndex = 15;
            label13.Text = "Employee List";
            // 
            // EmployeeDataGridView
            // 
            EmployeeDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            EmployeeDataGridView.BackgroundColor = Color.FromArgb(20, 20, 20);
            EmployeeDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            EmployeeDataGridView.Location = new Point(236, 198);
            EmployeeDataGridView.Name = "EmployeeDataGridView";
            EmployeeDataGridView.Size = new Size(1257, 196);
            EmployeeDataGridView.TabIndex = 16;
            // 
            // AttendanceLogsDataGridView
            // 
            AttendanceLogsDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            AttendanceLogsDataGridView.BackgroundColor = Color.FromArgb(20, 20, 20);
            AttendanceLogsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AttendanceLogsDataGridView.Location = new Point(236, 421);
            AttendanceLogsDataGridView.Name = "AttendanceLogsDataGridView";
            AttendanceLogsDataGridView.Size = new Size(1257, 284);
            AttendanceLogsDataGridView.TabIndex = 17;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label12.ForeColor = Color.Gray;
            label12.Location = new Point(239, 397);
            label12.Name = "label12";
            label12.Size = new Size(102, 21);
            label12.TabIndex = 18;
            label12.Text = "Today's Shift";
            // 
            // ReportPanel
            // 
            ReportPanel.BackColor = Color.FromArgb(31, 31, 31);
            ReportPanel.Controls.Add(label14);
            ReportPanel.Controls.Add(label15);
            ReportPanel.Location = new Point(832, 99);
            ReportPanel.Name = "ReportPanel";
            ReportPanel.Size = new Size(134, 60);
            ReportPanel.TabIndex = 19;
            ReportPanel.Paint += ReportPanel_Paint;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.BackColor = Color.Transparent;
            label14.Font = new Font("Arial", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.ForeColor = Color.Goldenrod;
            label14.Location = new Point(3, 4);
            label14.Name = "label14";
            label14.Size = new Size(22, 24);
            label14.TabIndex = 7;
            label14.Text = "2";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.BackColor = Color.Transparent;
            label15.ForeColor = Color.Gray;
            label15.Location = new Point(3, 35);
            label15.Name = "label15";
            label15.Size = new Size(82, 15);
            label15.TabIndex = 4;
            label15.Text = "Reports Today";
            // 
            // AdminDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1505, 717);
            Controls.Add(ReportPanel);
            Controls.Add(label12);
            Controls.Add(AttendanceLogsDataGridView);
            Controls.Add(EmployeeDataGridView);
            Controls.Add(label13);
            Controls.Add(LateTodayPanel);
            Controls.Add(AbsentTodayPanel);
            Controls.Add(PresentTodayPanel);
            Controls.Add(TotalEmployeePanel);
            Controls.Add(CurrentTime);
            Controls.Add(DateTimePicker);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(AdjustLbl);
            Controls.Add(label1);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AdminDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AdminDashboard";
            Load += AdminDashboard_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            TotalEmployeePanel.ResumeLayout(false);
            TotalEmployeePanel.PerformLayout();
            PresentTodayPanel.ResumeLayout(false);
            PresentTodayPanel.PerformLayout();
            AbsentTodayPanel.ResumeLayout(false);
            AbsentTodayPanel.PerformLayout();
            LateTodayPanel.ResumeLayout(false);
            LateTodayPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)EmployeeDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)AttendanceLogsDataGridView).EndInit();
            ReportPanel.ResumeLayout(false);
            ReportPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button LogOutBtn;
        private Button AdminDashboardBtn;
        private Label label12;
        private PictureBox pictureBox1;
        private Label label1;
        private Label AdjustLbl;
        private Label label3;
        private Label label2;
        private DateTimePicker DateTimePicker;
        private Label CurrentTime;
        private Button button3;
        private Button button2;
        private Button button1;
        private Button EmployeBtn;
        private Button ReportBtn;
        private Panel TotalEmployeePanel;
        private Label label6;
        private Label label5;
        private Panel PresentTodayPanel;
        private Label label4;
        private Label label7;
        private Panel AbsentTodayPanel;
        private Label label8;
        private Label label9;
        private Panel LateTodayPanel;
        private Label label10;
        private Label label11;
        private Label label13;
        private DataGridView EmployeeDataGridView;
        private DataGridView AttendanceLogsDataGridView;
        private Panel panel2;
        private Label label14;
        private Label label15;
        private Panel ReportPanel;
        private Button AttendanceLogsBtn;
        private Panel ProfilePanel;
    }
}