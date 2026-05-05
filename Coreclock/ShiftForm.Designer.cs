namespace Coreclock
{
    partial class ShiftForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShiftForm));
            panel1 = new Panel();
            AttendanceLogsBtn = new Button();
            ReportBtn = new Button();
            AttendanceBtn = new Button();
            EmployeBtn = new Button();
            LogOutBtn = new Button();
            AdminDashboardBtn = new Button();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            AdjustLbl = new Label();
            label1 = new Label();
            label3 = new Label();
            DateTimePicker = new DateTimePicker();
            CurrentTime = new Label();
            ShiftDataGridView = new DataGridView();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ShiftDataGridView).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(31, 31, 31);
            panel1.Controls.Add(AttendanceLogsBtn);
            panel1.Controls.Add(ReportBtn);
            panel1.Controls.Add(AttendanceBtn);
            panel1.Controls.Add(EmployeBtn);
            panel1.Controls.Add(LogOutBtn);
            panel1.Controls.Add(AdminDashboardBtn);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(-3, -2);
            panel1.Name = "panel1";
            panel1.Size = new Size(220, 655);
            panel1.TabIndex = 3;
            panel1.Paint += panel1_Paint;
            // 
            // AttendanceLogsBtn
            // 
            AttendanceLogsBtn.FlatStyle = FlatStyle.Flat;
            AttendanceLogsBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AttendanceLogsBtn.ForeColor = SystemColors.ButtonFace;
            AttendanceLogsBtn.Location = new Point(15, 308);
            AttendanceLogsBtn.Name = "AttendanceLogsBtn";
            AttendanceLogsBtn.Size = new Size(188, 32);
            AttendanceLogsBtn.TabIndex = 14;
            AttendanceLogsBtn.Text = "Attendance Logs";
            AttendanceLogsBtn.UseVisualStyleBackColor = true;
            AttendanceLogsBtn.Click += button1_Click;
            // 
            // ReportBtn
            // 
            ReportBtn.FlatStyle = FlatStyle.Flat;
            ReportBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ReportBtn.ForeColor = SystemColors.ButtonFace;
            ReportBtn.Location = new Point(15, 365);
            ReportBtn.Name = "ReportBtn";
            ReportBtn.Size = new Size(188, 32);
            ReportBtn.TabIndex = 13;
            ReportBtn.Text = "Reports";
            ReportBtn.UseVisualStyleBackColor = true;
            // 
            // AttendanceBtn
            // 
            AttendanceBtn.FlatStyle = FlatStyle.Flat;
            AttendanceBtn.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AttendanceBtn.ForeColor = SystemColors.ButtonFace;
            AttendanceBtn.Location = new Point(15, 256);
            AttendanceBtn.Name = "AttendanceBtn";
            AttendanceBtn.Size = new Size(188, 32);
            AttendanceBtn.TabIndex = 12;
            AttendanceBtn.Text = "Works Schedule Today";
            AttendanceBtn.UseVisualStyleBackColor = true;
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
            LogOutBtn.Location = new Point(35, 556);
            LogOutBtn.Name = "LogOutBtn";
            LogOutBtn.Size = new Size(139, 32);
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
            pictureBox1.Location = new Point(3, 17);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(220, 112);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(220, 62);
            label2.Name = "label2";
            label2.Size = new Size(752, 15);
            label2.TabIndex = 13;
            label2.Text = "_____________________________________________________________________________________________________________________________________________________";
            // 
            // AdjustLbl
            // 
            AdjustLbl.AutoSize = true;
            AdjustLbl.Font = new Font("Verdana", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AdjustLbl.ForeColor = Color.Goldenrod;
            AdjustLbl.Location = new Point(335, 9);
            AdjustLbl.Name = "AdjustLbl";
            AdjustLbl.Size = new Size(177, 38);
            AdjustLbl.TabIndex = 14;
            AdjustLbl.Text = "Morning!";
            AdjustLbl.Click += AdjustLbl_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Verdana", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(236, 9);
            label1.Name = "label1";
            label1.Size = new Size(109, 38);
            label1.TabIndex = 15;
            label1.Text = "Good";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Gray;
            label3.Location = new Point(239, 47);
            label3.Name = "label3";
            label3.Size = new Size(127, 15);
            label3.TabIndex = 16;
            label3.Text = "Welcome back, Admin";
            // 
            // DateTimePicker
            // 
            DateTimePicker.CalendarForeColor = Color.IndianRed;
            DateTimePicker.CalendarMonthBackground = SystemColors.InactiveCaption;
            DateTimePicker.Location = new Point(766, 41);
            DateTimePicker.Name = "DateTimePicker";
            DateTimePicker.Size = new Size(200, 23);
            DateTimePicker.TabIndex = 17;
            // 
            // CurrentTime
            // 
            CurrentTime.AutoSize = true;
            CurrentTime.Font = new Font("Verdana", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CurrentTime.ForeColor = Color.Goldenrod;
            CurrentTime.Location = new Point(854, 15);
            CurrentTime.Name = "CurrentTime";
            CurrentTime.Size = new Size(96, 23);
            CurrentTime.TabIndex = 18;
            CurrentTime.Text = "0:00:00";
            // 
            // ShiftDataGridView
            // 
            ShiftDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ShiftDataGridView.Location = new Point(223, 121);
            ShiftDataGridView.Name = "ShiftDataGridView";
            ShiftDataGridView.Size = new Size(749, 522);
            ShiftDataGridView.TabIndex = 19;
            // 
            // ShiftForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(979, 649);
            Controls.Add(ShiftDataGridView);
            Controls.Add(CurrentTime);
            Controls.Add(DateTimePicker);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(AdjustLbl);
            Controls.Add(label2);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ShiftForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "ShiftForm";
            Load += ShiftForm_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)ShiftDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button ReportBtn;
        private Button AttendanceBtn;
        private Button EmployeBtn;
        private Button LogOutBtn;
        private Button AdminDashboardBtn;
        private PictureBox pictureBox1;
        private Label label2;
        private Label AdjustLbl;
        private Label label1;
        private Label label3;
        private DateTimePicker DateTimePicker;
        private Label CurrentTime;
        private DataGridView ShiftGridDataView;
        private DataGridView ShiftDataGridView;
        private Button AttendanceLogsBtn;
    }
}