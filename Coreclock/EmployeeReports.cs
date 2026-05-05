using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Coreclock
{
    public partial class EmployeeReports : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
             int nLeftRect, int nTopRect,
             int nRightRect, int nBottomRect,
             int nWidthEllipse, int nHeightEllipse
         );

        private Panel dtpPanel;
        private Label dtpLabel;
        private Image _profilePhoto = null;


        public EmployeeReports()
        {
            InitializeComponent();
            ApplyRoundedCorners();
            StyleDateTimePicker();
            StyleProfilePanel();
        }

        // ─── FAKE DATE TIME PICKER ───────────────────────────────────────────
        private void StyleDateTimePicker()
        {
            DateTimePicker.Visible = false;

            dtpPanel = new Panel();
            dtpPanel.Size = new Size(DateTimePicker.Width, DateTimePicker.Height);
            dtpPanel.Location = DateTimePicker.Location;
            dtpPanel.BackColor = Color.FromArgb(40, 40, 40);
            dtpPanel.Cursor = Cursors.Hand;

            dtpPanel.HandleCreated += (object? s, EventArgs e) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, dtpPanel.Width, dtpPanel.Height, 20, 20);
                dtpPanel.Region = System.Drawing.Region.FromHrgn(hRgn);
            };

            dtpLabel = new Label();
            dtpLabel.AutoSize = false;
            dtpLabel.Dock = DockStyle.Fill;
            dtpLabel.TextAlign = ContentAlignment.MiddleCenter;
            dtpLabel.ForeColor = Color.White;
            dtpLabel.BackColor = Color.Transparent;
            dtpLabel.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
            dtpLabel.Text = DateTimePicker.Value.ToString("dddd, MMMM d, yyyy");

            dtpPanel.Click += (object? s, EventArgs e) =>
            {
                DateTimePicker.Location = dtpPanel.Location;
                DateTimePicker.Visible = true;
                DateTimePicker.Focus();
            };

            dtpLabel.Click += (object? s, EventArgs e) =>
            {
                DateTimePicker.Location = dtpPanel.Location;
                DateTimePicker.Visible = true;
                DateTimePicker.Focus();
            };

            DateTimePicker.ValueChanged += (object? s, EventArgs e) =>
            {
                dtpLabel.Text = DateTimePicker.Value.ToString("dddd, MMMM d, yyyy");
                DateTimePicker.Visible = false;
            };

            DateTimePicker.Leave += (object? s, EventArgs e) =>
            {
                DateTimePicker.Visible = false;
            };

            DateTimePicker.CalendarMonthBackground = Color.FromArgb(40, 40, 40);
            DateTimePicker.CalendarForeColor = Color.White;
            DateTimePicker.CalendarTitleBackColor = Color.FromArgb(20, 20, 20);
            DateTimePicker.CalendarTitleForeColor = Color.White;
            DateTimePicker.CalendarTrailingForeColor = Color.Gray;

            dtpPanel.Controls.Add(dtpLabel);
            DateTimePicker.Parent.Controls.Add(dtpPanel);
            dtpPanel.BringToFront();
        }

        // ─── ROUNDED HELPERS ─────────────────────────────────────────────────
        private void MakeButtonRounded(Button btn, int radius)
        {
            IntPtr hRgn = CreateRoundRectRgn(0, 0, btn.Width, btn.Height, radius, radius);
            btn.Region = System.Drawing.Region.FromHrgn(hRgn);
        }

        private void MakePanelRounded(Panel panel, int radius)
        {
            IntPtr hRgn = CreateRoundRectRgn(0, 0, panel.Width, panel.Height, radius, radius);
            panel.Region = System.Drawing.Region.FromHrgn(hRgn);
        }

        private void ApplyRoundedCorners()
        {
            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, Width, Height, 30, 30)
            );
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyRoundedCorners();
        }

        // ─── STUBS ───────────────────────────────────────────────────────────
        private void EmployeeReports_Load(object sender, EventArgs e)
        {
            MakeButtonRounded(LeaveBtn, 20);
            MakeButtonRounded(OvertimeBtn, 20);
            MakeButtonRounded(IncidentBtn, 20);
            MakeButtonRounded(EquipmentBtn, 20);
            MakeButtonRounded(ScheduleBtn, 20);
            MakeButtonRounded(SickBtn, 20);
            MakeButtonRounded(ImportantBtn, 20);
            MakeButtonRounded(OtherBtn, 20);
            MakeButtonRounded(ClearBtn, 20);
            MakeButtonRounded(SendBtn, 20);
            MakeButtonRounded(OtherBtn, 20);

            LeaveBtn.FlatStyle = FlatStyle.Flat;
            LeaveBtn.FlatAppearance.BorderSize = 0;
            LeaveBtn.BackColor = Color.FromArgb(20, 20, 20);
            LeaveBtn.ForeColor = Color.White;

            OvertimeBtn.FlatStyle = FlatStyle.Flat;
            OvertimeBtn.FlatAppearance.BorderSize = 0;
            OvertimeBtn.BackColor = Color.FromArgb(20, 20, 20);
            OvertimeBtn.ForeColor = Color.White;

            IncidentBtn.FlatStyle = FlatStyle.Flat;
            IncidentBtn.FlatAppearance.BorderSize = 0;
            IncidentBtn.BackColor = Color.FromArgb(20, 20, 20);
            IncidentBtn.ForeColor = Color.White;

            EquipmentBtn.FlatStyle = FlatStyle.Flat;
            EquipmentBtn.FlatAppearance.BorderSize = 0;
            EquipmentBtn.BackColor = Color.FromArgb(20, 20, 20);
            EquipmentBtn.ForeColor = Color.White;

            ScheduleBtn.FlatStyle = FlatStyle.Flat;
            ScheduleBtn.FlatAppearance.BorderSize = 0;
            ScheduleBtn.BackColor = Color.FromArgb(20, 20, 20);
            ScheduleBtn.ForeColor = Color.White;

            SickBtn.FlatStyle = FlatStyle.Flat;
            SickBtn.FlatAppearance.BorderSize = 0;
            SickBtn.BackColor = Color.FromArgb(20, 20, 20);
            SickBtn.ForeColor = Color.White;

            ImportantBtn.FlatStyle = FlatStyle.Flat;
            ImportantBtn.FlatAppearance.BorderSize = 0;
            ImportantBtn.BackColor = Color.FromArgb(20, 20, 20);
            ImportantBtn.ForeColor = Color.White;

            ClearBtn.FlatStyle = FlatStyle.Flat;
            ClearBtn.FlatAppearance.BorderSize = 0;
            ClearBtn.BackColor = Color.FromArgb(20, 20, 20);
            ClearBtn.ForeColor = Color.White;

            SendBtn.FlatStyle = FlatStyle.Flat;
            SendBtn.FlatAppearance.BorderSize = 0;
            SendBtn.BackColor = Color.FromArgb(20, 20, 20);
            SendBtn.ForeColor = Color.White;

            OtherBtn.FlatStyle = FlatStyle.Flat;
            OtherBtn.FlatAppearance.BorderSize = 0;
            OtherBtn.BackColor = Color.FromArgb(20, 20, 20);
            OtherBtn.ForeColor = Color.White;

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            MakeButtonRounded(ReportsBtn, 20);
            MakeButtonRounded(MyDashboardBtn, 20);
            MakeButtonRounded(LogOutBtn, 20);

            MyDashboardBtn.FlatStyle = FlatStyle.Flat;
            MyDashboardBtn.FlatAppearance.BorderSize = 0;
            MyDashboardBtn.BackColor = Color.FromArgb(20, 20, 20);
            MyDashboardBtn.ForeColor = Color.White;

            ReportsBtn.FlatStyle = FlatStyle.Flat;
            ReportsBtn.FlatAppearance.BorderSize = 0;
            ReportsBtn.BackColor = Color.FromArgb(20, 20, 20);
            ReportsBtn.ForeColor = Color.White;

            LogOutBtn.FlatStyle = FlatStyle.Flat;
            LogOutBtn.FlatAppearance.BorderSize = 0;
            LogOutBtn.BackColor = Color.FromArgb(20, 20, 20);
            LogOutBtn.ForeColor = Color.White;
        }

        // ─── PROFILE PANEL ────────────────────────────────────────────────────
        private void StyleProfilePanel()
        {
            ProfilePanel.Controls.Clear();
            ProfilePanel.BackColor = Color.FromArgb(30, 30, 30);

            // ── Avatar PictureBox ──
            PictureBox avatar = new PictureBox();
            avatar.Size = new Size(70, 70);
            avatar.Location = new Point((ProfilePanel.Width - 70) / 2, 6);
            avatar.SizeMode = PictureBoxSizeMode.Zoom;
            avatar.BackColor = Color.FromArgb(42, 42, 42);
            avatar.Cursor = Cursors.Hand;
            IntPtr avatarRgn = CreateRoundRectRgn(0, 0, 70, 70, 70, 70);
            avatar.Region = System.Drawing.Region.FromHrgn(avatarRgn);

            // Initials label shown when no photo uploaded
            Label lblInitials = new Label();
            lblInitials.Text = "JI"; // replace with real initials later
            lblInitials.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            lblInitials.ForeColor = Color.FromArgb(200, 168, 75);
            lblInitials.BackColor = Color.Transparent;
            lblInitials.AutoSize = false;
            lblInitials.Size = avatar.Size;
            lblInitials.Location = new Point(0, 0);
            lblInitials.TextAlign = ContentAlignment.MiddleCenter;
            avatar.Controls.Add(lblInitials);

            // Gold border ring painted on avatar
            avatar.Paint += (s, e) =>
            {
                using var pen = new Pen(Color.FromArgb(200, 168, 75), 2);
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                e.Graphics.DrawEllipse(pen, 1, 1, avatar.Width - 3, avatar.Height - 3);
            };

            // ── Upload button (pencil icon) ──
            Button btnUpload = new Button();
            btnUpload.Text = "✎";
            btnUpload.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            btnUpload.Size = new Size(22, 22);
            btnUpload.Location = new Point(avatar.Right - 4, avatar.Bottom - 22);
            btnUpload.FlatStyle = FlatStyle.Flat;
            btnUpload.FlatAppearance.BorderSize = 0;
            btnUpload.BackColor = Color.FromArgb(200, 168, 75);
            btnUpload.ForeColor = Color.FromArgb(20, 20, 20);
            btnUpload.Cursor = Cursors.Hand;
            IntPtr btnRgn = CreateRoundRectRgn(0, 0, 22, 22, 22, 22);
            btnUpload.Region = System.Drawing.Region.FromHrgn(btnRgn);
            btnUpload.Click += (s, e) =>
            {
                using OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _profilePhoto = Image.FromFile(ofd.FileName);
                    avatar.Image = _profilePhoto;
                    lblInitials.Visible = false;
                }
            };

            // ── Name label ──
            Label lblName = new Label();
            lblName.Text = "Jong Idol"; // replace with real data later
            lblName.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblName.ForeColor = Color.White;
            lblName.BackColor = Color.Transparent;
            lblName.AutoSize = false;
            lblName.Width = ProfilePanel.Width - 10;
            lblName.Height = 20;
            lblName.Location = new Point(5, avatar.Bottom + 6);
            lblName.TextAlign = ContentAlignment.MiddleCenter;

            // ── Employee ID label ──
            Label lblId = new Label();
            lblId.Text = "EMP-001"; // replace with real data later
            lblId.Font = new Font("Segoe UI", 8f);
            lblId.ForeColor = Color.FromArgb(140, 140, 140);
            lblId.BackColor = Color.Transparent;
            lblId.AutoSize = false;
            lblId.Width = ProfilePanel.Width - 10;
            lblId.Height = 16;
            lblId.Location = new Point(5, lblName.Bottom + 2);
            lblId.TextAlign = ContentAlignment.MiddleCenter;

            // ── Position badge ──
            Label lblPos = new Label();
            lblPos.Text = "Software Developer"; // replace with real data later
            lblPos.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            lblPos.ForeColor = Color.FromArgb(200, 168, 75);
            lblPos.BackColor = Color.FromArgb(42, 35, 10);
            lblPos.AutoSize = false;
            lblPos.Width = ProfilePanel.Width - 24;
            lblPos.Height = 22;
            lblPos.Location = new Point(12, lblId.Bottom + 4);
            lblPos.TextAlign = ContentAlignment.MiddleCenter;
            IntPtr posRgn = CreateRoundRectRgn(0, 0, lblPos.Width, lblPos.Height, 20, 20);
            lblPos.Region = System.Drawing.Region.FromHrgn(posRgn);

            // ── Divider ──
            Panel divider = new Panel();
            divider.BackColor = Color.FromArgb(45, 45, 45);
            divider.Size = new Size(ProfilePanel.Width - 24, 1);
            divider.Location = new Point(12, lblPos.Bottom + 6);

            // ── Status dot ──
            Panel statusDot = new Panel();
            statusDot.Size = new Size(9, 9);
            statusDot.BackColor = Color.FromArgb(80, 200, 120);
            statusDot.Location = new Point((ProfilePanel.Width / 2) - 30, divider.Bottom + 6);
            IntPtr dotRgn = CreateRoundRectRgn(0, 0, 9, 9, 9, 9);
            statusDot.Region = System.Drawing.Region.FromHrgn(dotRgn);

            // ── Status text ──
            Label lblStatus = new Label();
            lblStatus.Text = "Active";
            lblStatus.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(80, 200, 120);
            lblStatus.BackColor = Color.Transparent;
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(statusDot.Right + 5, divider.Bottom + 4);

            ProfilePanel.Controls.Add(avatar);
            ProfilePanel.Controls.Add(btnUpload);
            ProfilePanel.Controls.Add(lblName);
            ProfilePanel.Controls.Add(lblId);
            ProfilePanel.Controls.Add(lblPos);
            ProfilePanel.Controls.Add(divider);
            ProfilePanel.Controls.Add(statusDot);
            ProfilePanel.Controls.Add(lblStatus);

            btnUpload.BringToFront();
        }

        private void MyDashboardBtn_Click(object sender, EventArgs e)
        {
            EmployeeDashboard EmployeeD = new EmployeeDashboard();
            EmployeeD.Show();
            this.Hide();
        }

        private void LogOutBtn_Click(object sender, EventArgs e)
        {
            HomeForm Home = new HomeForm();
            Home.Show();
            this.Hide();
        }
    }
}