namespace Coreclock
{
    partial class AttendanceLogs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttendanceLogs));
            panel1 = new Panel();
            ProfilePanel = new Panel();
            AttendanceLogsBtn = new Button();
            ReportBtn = new Button();
            EmployeBtn = new Button();
            LogOutBtn = new Button();
            AdminDashboardBtn = new Button();
            pictureBox1 = new PictureBox();
            AdjustLbl = new Label();
            label1 = new Label();
            label3 = new Label();
            label2 = new Label();
            DateTimePicker = new DateTimePicker();
            CurrentTime = new Label();
            AttendanceLogsDataGridView = new DataGridView();
            pictureBox2 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)AttendanceLogsDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
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
            panel1.Size = new Size(220, 717);
            panel1.TabIndex = 2;
            panel1.Paint += panel1_Paint;
            // 
            // ProfilePanel
            // 
            ProfilePanel.BackColor = Color.FromArgb(20, 20, 20);
            ProfilePanel.Location = new Point(14, 423);
            ProfilePanel.Name = "ProfilePanel";
            ProfilePanel.Size = new Size(188, 223);
            ProfilePanel.TabIndex = 16;
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
            AdminDashboardBtn.Click += AdminDashboardBtn_Click;
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
            // AdjustLbl
            // 
            AdjustLbl.AutoSize = true;
            AdjustLbl.Font = new Font("Verdana", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AdjustLbl.ForeColor = Color.Goldenrod;
            AdjustLbl.Location = new Point(323, 9);
            AdjustLbl.Name = "AdjustLbl";
            AdjustLbl.Size = new Size(177, 38);
            AdjustLbl.TabIndex = 4;
            AdjustLbl.Text = "Morning!";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Verdana", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(223, 9);
            label1.Name = "label1";
            label1.Size = new Size(109, 38);
            label1.TabIndex = 5;
            label1.Text = "Good";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Gray;
            label3.Location = new Point(229, 47);
            label3.Name = "label3";
            label3.Size = new Size(118, 15);
            label3.TabIndex = 6;
            label3.Text = "Hi Admin, Find who?";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(229, 62);
            label2.Name = "label2";
            label2.Size = new Size(1267, 15);
            label2.TabIndex = 9;
            label2.Text = resources.GetString("label2.Text");
            // 
            // DateTimePicker
            // 
            DateTimePicker.CalendarForeColor = Color.IndianRed;
            DateTimePicker.CalendarMonthBackground = SystemColors.InactiveCaption;
            DateTimePicker.Location = new Point(1293, 47);
            DateTimePicker.Name = "DateTimePicker";
            DateTimePicker.Size = new Size(200, 23);
            DateTimePicker.TabIndex = 10;
            // 
            // CurrentTime
            // 
            CurrentTime.AutoSize = true;
            CurrentTime.Font = new Font("Verdana", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CurrentTime.ForeColor = Color.Goldenrod;
            CurrentTime.Location = new Point(1379, 22);
            CurrentTime.Name = "CurrentTime";
            CurrentTime.Size = new Size(96, 23);
            CurrentTime.TabIndex = 11;
            CurrentTime.Text = "0:00:00";
            // 
            // AttendanceLogsDataGridView
            // 
            AttendanceLogsDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedHeaders;
            AttendanceLogsDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            AttendanceLogsDataGridView.Location = new Point(229, 252);
            AttendanceLogsDataGridView.Name = "AttendanceLogsDataGridView";
            AttendanceLogsDataGridView.Size = new Size(1264, 453);
            AttendanceLogsDataGridView.TabIndex = 14;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(229, 80);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(642, 44);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 15;
            pictureBox2.TabStop = false;
            // 
            // AttendanceLogs
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(1505, 717);
            Controls.Add(pictureBox2);
            Controls.Add(AttendanceLogsDataGridView);
            Controls.Add(CurrentTime);
            Controls.Add(DateTimePicker);
            Controls.Add(label2);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(AdjustLbl);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AttendanceLogs";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AttendanceLogs";
            Load += AttendanceLogs_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)AttendanceLogsDataGridView).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button AttendanceLogsBtn;
        private Button ReportBtn;
        private Button EmployeBtn;
        private Button LogOutBtn;
        private Button AdminDashboardBtn;
        private PictureBox pictureBox1;
        private Label AdjustLbl;
        private Label label1;
        private Label label3;
        private Label label2;
        private DateTimePicker DateTimePicker;
        private Label CurrentTime;
        private DataGridView AttendanceLogsDataGridView;
        private PictureBox pictureBox2;
        private Panel ProfilePanel;
    }
}