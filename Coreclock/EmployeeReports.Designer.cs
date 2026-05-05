namespace Coreclock
{
    partial class EmployeeReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeReports));
            panel1 = new Panel();
            ReportsBtn = new Button();
            LogOutBtn = new Button();
            MyDashboardBtn = new Button();
            label12 = new Label();
            pictureBox1 = new PictureBox();
            AdjustLbl = new Label();
            label1 = new Label();
            label3 = new Label();
            DateTimePicker = new DateTimePicker();
            label2 = new Label();
            pictureBox2 = new PictureBox();
            reportlbl = new Label();
            label4 = new Label();
            label5 = new Label();
            LeaveBtn = new Button();
            OvertimeBtn = new Button();
            IncidentBtn = new Button();
            EquipmentBtn = new Button();
            ScheduleBtn = new Button();
            SickBtn = new Button();
            ImportantBtn = new Button();
            OtherBtn = new Button();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            richTextBox1 = new RichTextBox();
            label10 = new Label();
            ClearBtn = new Button();
            SendBtn = new Button();
            ProfilePanel = new Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(31, 31, 31);
            panel1.Controls.Add(ProfilePanel);
            panel1.Controls.Add(ReportsBtn);
            panel1.Controls.Add(LogOutBtn);
            panel1.Controls.Add(MyDashboardBtn);
            panel1.Controls.Add(label12);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(-3, -2);
            panel1.Name = "panel1";
            panel1.Size = new Size(220, 601);
            panel1.TabIndex = 1;
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
            // AdjustLbl
            // 
            AdjustLbl.AutoSize = true;
            AdjustLbl.Font = new Font("Verdana", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            AdjustLbl.ForeColor = Color.Goldenrod;
            AdjustLbl.Location = new Point(311, 21);
            AdjustLbl.Name = "AdjustLbl";
            AdjustLbl.Size = new Size(177, 38);
            AdjustLbl.TabIndex = 3;
            AdjustLbl.Text = "Morning!";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Verdana", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(218, 21);
            label1.Name = "label1";
            label1.Size = new Size(109, 38);
            label1.TabIndex = 4;
            label1.Text = "Good";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Gray;
            label3.Location = new Point(227, 76);
            label3.Name = "label3";
            label3.Size = new Size(111, 15);
            label3.TabIndex = 5;
            label3.Text = "Report incoming.....";
            // 
            // DateTimePicker
            // 
            DateTimePicker.CalendarForeColor = Color.IndianRed;
            DateTimePicker.CalendarMonthBackground = SystemColors.InactiveCaption;
            DateTimePicker.Location = new Point(223, 271);
            DateTimePicker.Name = "DateTimePicker";
            DateTimePicker.Size = new Size(200, 23);
            DateTimePicker.TabIndex = 6;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.ForeColor = Color.Gray;
            label2.Location = new Point(223, 91);
            label2.Name = "label2";
            label2.Size = new Size(547, 15);
            label2.TabIndex = 7;
            label2.Text = "____________________________________________________________________________________________________________";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(223, 110);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(41, 39);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 8;
            pictureBox2.TabStop = false;
            // 
            // reportlbl
            // 
            reportlbl.AutoSize = true;
            reportlbl.Font = new Font("Verdana", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            reportlbl.ForeColor = Color.Goldenrod;
            reportlbl.Location = new Point(270, 118);
            reportlbl.Name = "reportlbl";
            reportlbl.Size = new Size(120, 16);
            reportlbl.TabIndex = 9;
            reportlbl.Text = "Submit a report";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Gray;
            label4.Location = new Point(270, 134);
            label4.Name = "label4";
            label4.Size = new Size(218, 15);
            label4.TabIndex = 10;
            label4.Text = "Send a message or request to the admin";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Verdana", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.Silver;
            label5.Location = new Point(223, 169);
            label5.Name = "label5";
            label5.Size = new Size(94, 16);
            label5.TabIndex = 11;
            label5.Text = "Report Type";
            // 
            // LeaveBtn
            // 
            LeaveBtn.BackColor = Color.FromArgb(31, 31, 31);
            LeaveBtn.FlatStyle = FlatStyle.Flat;
            LeaveBtn.ForeColor = Color.White;
            LeaveBtn.Location = new Point(223, 188);
            LeaveBtn.Name = "LeaveBtn";
            LeaveBtn.Size = new Size(94, 23);
            LeaveBtn.TabIndex = 12;
            LeaveBtn.Text = "Leave Request";
            LeaveBtn.UseVisualStyleBackColor = false;
            // 
            // OvertimeBtn
            // 
            OvertimeBtn.BackColor = Color.FromArgb(31, 31, 31);
            OvertimeBtn.FlatStyle = FlatStyle.Flat;
            OvertimeBtn.ForeColor = Color.White;
            OvertimeBtn.Location = new Point(323, 188);
            OvertimeBtn.Name = "OvertimeBtn";
            OvertimeBtn.Size = new Size(94, 23);
            OvertimeBtn.TabIndex = 13;
            OvertimeBtn.Text = "Overtime Request";
            OvertimeBtn.UseVisualStyleBackColor = false;
            // 
            // IncidentBtn
            // 
            IncidentBtn.BackColor = Color.FromArgb(31, 31, 31);
            IncidentBtn.FlatStyle = FlatStyle.Flat;
            IncidentBtn.ForeColor = Color.White;
            IncidentBtn.Location = new Point(423, 188);
            IncidentBtn.Name = "IncidentBtn";
            IncidentBtn.Size = new Size(94, 23);
            IncidentBtn.TabIndex = 14;
            IncidentBtn.Text = "Incident Report";
            IncidentBtn.UseVisualStyleBackColor = false;
            // 
            // EquipmentBtn
            // 
            EquipmentBtn.BackColor = Color.FromArgb(31, 31, 31);
            EquipmentBtn.FlatStyle = FlatStyle.Flat;
            EquipmentBtn.ForeColor = Color.White;
            EquipmentBtn.Location = new Point(523, 188);
            EquipmentBtn.Name = "EquipmentBtn";
            EquipmentBtn.Size = new Size(94, 23);
            EquipmentBtn.TabIndex = 15;
            EquipmentBtn.Text = "Equipment Change";
            EquipmentBtn.TextAlign = ContentAlignment.BottomCenter;
            EquipmentBtn.UseVisualStyleBackColor = false;
            // 
            // ScheduleBtn
            // 
            ScheduleBtn.BackColor = Color.FromArgb(31, 31, 31);
            ScheduleBtn.FlatStyle = FlatStyle.Flat;
            ScheduleBtn.ForeColor = Color.White;
            ScheduleBtn.Location = new Point(623, 188);
            ScheduleBtn.Name = "ScheduleBtn";
            ScheduleBtn.Size = new Size(94, 23);
            ScheduleBtn.TabIndex = 16;
            ScheduleBtn.Text = "Schedule Change";
            ScheduleBtn.TextAlign = ContentAlignment.BottomCenter;
            ScheduleBtn.UseVisualStyleBackColor = false;
            // 
            // SickBtn
            // 
            SickBtn.BackColor = Color.FromArgb(31, 31, 31);
            SickBtn.FlatStyle = FlatStyle.Flat;
            SickBtn.ForeColor = Color.White;
            SickBtn.Location = new Point(223, 217);
            SickBtn.Name = "SickBtn";
            SickBtn.Size = new Size(94, 23);
            SickBtn.TabIndex = 17;
            SickBtn.Text = "Sick Leave Notice";
            SickBtn.TextAlign = ContentAlignment.BottomCenter;
            SickBtn.UseVisualStyleBackColor = false;
            // 
            // ImportantBtn
            // 
            ImportantBtn.BackColor = Color.FromArgb(31, 31, 31);
            ImportantBtn.FlatStyle = FlatStyle.Flat;
            ImportantBtn.ForeColor = Color.White;
            ImportantBtn.Location = new Point(323, 217);
            ImportantBtn.Name = "ImportantBtn";
            ImportantBtn.Size = new Size(94, 23);
            ImportantBtn.TabIndex = 18;
            ImportantBtn.Text = "Important";
            ImportantBtn.TextAlign = ContentAlignment.BottomCenter;
            ImportantBtn.UseVisualStyleBackColor = false;
            // 
            // OtherBtn
            // 
            OtherBtn.BackColor = Color.FromArgb(31, 31, 31);
            OtherBtn.FlatStyle = FlatStyle.Flat;
            OtherBtn.ForeColor = Color.White;
            OtherBtn.Location = new Point(423, 217);
            OtherBtn.Name = "OtherBtn";
            OtherBtn.Size = new Size(94, 23);
            OtherBtn.TabIndex = 19;
            OtherBtn.Text = "Other";
            OtherBtn.TextAlign = ContentAlignment.BottomCenter;
            OtherBtn.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Verdana", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.FromArgb(224, 224, 224);
            label6.Location = new Point(223, 252);
            label6.Name = "label6";
            label6.Size = new Size(115, 16);
            label6.TabIndex = 20;
            label6.Text = "DATE / PERIOD";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.ForeColor = Color.Gray;
            label7.Location = new Point(223, 297);
            label7.Name = "label7";
            label7.Size = new Size(547, 15);
            label7.TabIndex = 22;
            label7.Text = "____________________________________________________________________________________________________________";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Verdana", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.ForeColor = Color.FromArgb(224, 224, 224);
            label8.Location = new Point(223, 327);
            label8.Name = "label8";
            label8.Size = new Size(75, 16);
            label8.TabIndex = 23;
            label8.Text = "MESSAGE";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Verdana", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.Goldenrod;
            label9.Location = new Point(293, 330);
            label9.Name = "label9";
            label9.Size = new Size(15, 13);
            label9.TabIndex = 24;
            label9.Text = "*";
            // 
            // richTextBox1
            // 
            richTextBox1.BackColor = Color.FromArgb(31, 31, 31);
            richTextBox1.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox1.ForeColor = Color.White;
            richTextBox1.Location = new Point(223, 356);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(540, 187);
            richTextBox1.TabIndex = 25;
            richTextBox1.Text = "";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            label10.ForeColor = Color.Gray;
            label10.Location = new Point(223, 546);
            label10.Name = "label10";
            label10.Size = new Size(104, 13);
            label10.TabIndex = 26;
            label10.Text = "Be clear and specific.";
            // 
            // ClearBtn
            // 
            ClearBtn.BackColor = Color.FromArgb(31, 31, 31);
            ClearBtn.FlatStyle = FlatStyle.Flat;
            ClearBtn.ForeColor = Color.White;
            ClearBtn.Location = new Point(223, 563);
            ClearBtn.Name = "ClearBtn";
            ClearBtn.Size = new Size(94, 23);
            ClearBtn.TabIndex = 27;
            ClearBtn.Text = "Clear";
            ClearBtn.TextAlign = ContentAlignment.BottomCenter;
            ClearBtn.UseVisualStyleBackColor = false;
            // 
            // SendBtn
            // 
            SendBtn.BackColor = Color.FromArgb(31, 31, 31);
            SendBtn.FlatStyle = FlatStyle.Flat;
            SendBtn.ForeColor = Color.White;
            SendBtn.Location = new Point(669, 563);
            SendBtn.Name = "SendBtn";
            SendBtn.Size = new Size(94, 23);
            SendBtn.TabIndex = 28;
            SendBtn.Text = "✉ Send";
            SendBtn.TextAlign = ContentAlignment.BottomCenter;
            SendBtn.UseVisualStyleBackColor = false;
            // 
            // ProfilePanel
            // 
            ProfilePanel.BackColor = Color.FromArgb(20, 20, 20);
            ProfilePanel.Location = new Point(15, 359);
            ProfilePanel.Name = "ProfilePanel";
            ProfilePanel.Size = new Size(188, 200);
            ProfilePanel.TabIndex = 12;
            // 
            // EmployeeReports
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(775, 598);
            Controls.Add(SendBtn);
            Controls.Add(ClearBtn);
            Controls.Add(label10);
            Controls.Add(richTextBox1);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(OtherBtn);
            Controls.Add(ImportantBtn);
            Controls.Add(SickBtn);
            Controls.Add(ScheduleBtn);
            Controls.Add(EquipmentBtn);
            Controls.Add(IncidentBtn);
            Controls.Add(OvertimeBtn);
            Controls.Add(LeaveBtn);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(reportlbl);
            Controls.Add(pictureBox2);
            Controls.Add(label2);
            Controls.Add(DateTimePicker);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(AdjustLbl);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "EmployeeReports";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EmployeeReports";
            Load += EmployeeReports_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button ReportsBtn;
        private Button LogOutBtn;
        private Button MyDashboardBtn;
        private Label label12;
        private PictureBox pictureBox1;
        private Label AdjustLbl;
        private Label label1;
        private Label label3;
        private DateTimePicker DateTimePicker;
        private Label label2;
        private PictureBox pictureBox2;
        private Label reportlbl;
        private Label label4;
        private Label label5;
        private Button LeaveBtn;
        private Button OvertimeBtn;
        private Button IncidentBtn;
        private Button EquipmentBtn;
        private Button ScheduleBtn;
        private Button SickBtn;
        private Button ImportantBtn;
        private Button OtherBtn;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private RichTextBox richTextBox1;
        private Label label10;
        private Button ClearBtn;
        private Button SendBtn;
        private Panel ProfilePanel;
    }
}