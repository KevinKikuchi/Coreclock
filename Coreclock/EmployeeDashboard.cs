using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace Coreclock
{
    public partial class EmployeeDashboard : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        private System.Windows.Forms.Timer clockTimer;
        private Panel dtpPanel;
        private Label dtpLabel;
        private Image _profilePhoto = null;

        public EmployeeDashboard()
        {
            InitializeComponent();
            ApplyRoundedCorners();
            StartClock();
            StyleDateTimePicker();
            StyleDataGridView();
            WrapGridWithRoundedPanel(LogsDataGridView, 20);
            StyleProfilePanel(); // ← profile panel
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

        // ─── DATA GRID VIEW ──────────────────────────────────────────────────
        private void StyleDataGridView()
        {
            LogsDataGridView.Columns.Clear();

            LogsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Date", HeaderText = "Date", Width = 120 });
            LogsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "TimeIn", HeaderText = "Time In", Width = 100 });
            LogsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "TimeOut", HeaderText = "Time Out", Width = 100 });
            LogsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "TotalHours", HeaderText = "Total Hours", Width = 100 });
            LogsDataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", Width = 100 });

            LogsDataGridView.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            LogsDataGridView.BackgroundColor = Color.FromArgb(40, 40, 40);
            LogsDataGridView.GridColor = Color.FromArgb(60, 60, 60);
            LogsDataGridView.BorderStyle = BorderStyle.None;
            LogsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            LogsDataGridView.RowHeadersVisible = false;
            LogsDataGridView.AllowUserToAddRows = false;
            LogsDataGridView.AllowUserToResizeRows = false;
            LogsDataGridView.ReadOnly = true;
            LogsDataGridView.SelectionMode = DataGridViewSelectionMode.CellSelect;
            LogsDataGridView.MultiSelect = false;
            LogsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(40, 40, 40);
            LogsDataGridView.DefaultCellStyle.SelectionForeColor = Color.White;
            LogsDataGridView.Enabled = false;

            LogsDataGridView.EnableHeadersVisualStyles = false;
            LogsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
            LogsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(200, 200, 200);
            LogsDataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
            LogsDataGridView.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            LogsDataGridView.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 5, 0, 5);
            LogsDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            LogsDataGridView.ColumnHeadersHeight = 35;

            LogsDataGridView.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            LogsDataGridView.DefaultCellStyle.ForeColor = Color.White;
            LogsDataGridView.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            LogsDataGridView.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            LogsDataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
            LogsDataGridView.DefaultCellStyle.SelectionForeColor = Color.White;
            LogsDataGridView.RowTemplate.Height = 35;

            LogsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            LogsDataGridView.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            LogsDataGridView.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(65, 65, 65);

            LogsDataGridView.CellPainting += LogsDataGridView_CellPainting;

            // Sample data — remove when loading real data
            LogsDataGridView.Rows.Add("05/02/2026", "08:00 AM", "05:00 PM", "9h 00m", "Present");
            LogsDataGridView.Rows.Add("05/01/2026", "08:15 AM", "05:10 PM", "8h 55m", "Present");
            LogsDataGridView.Rows.Add("04/30/2026", "—", "—", "—", "Absent");
        }

        private void LogsDataGridView_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == LogsDataGridView.Columns["Status"].Index && e.RowIndex >= 0)
            {
                string? status = e.Value?.ToString();
                Color textColor = status switch
                {
                    "Present" => Color.FromArgb(80, 200, 80),
                    "Absent" => Color.FromArgb(220, 60, 60),
                    "Late" => Color.FromArgb(255, 180, 0),
                    _ => Color.White
                };

                e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

                TextRenderer.DrawText(
                    e.Graphics,
                    status,
                    new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                    e.CellBounds,
                    textColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );

                e.Handled = true;
            }
        }

        // ─── ROUNDED WRAPPER FOR DATAGRIDVIEW ────────────────────────────────
        private void WrapGridWithRoundedPanel(DataGridView dgv, int radius)
        {
            Panel wrapper = new Panel();
            wrapper.Size = dgv.Size;
            wrapper.Location = dgv.Location;
            wrapper.Parent = dgv.Parent;
            wrapper.BackColor = Color.FromArgb(40, 40, 40);

            wrapper.HandleCreated += (object? s, EventArgs e) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, wrapper.Width, wrapper.Height, radius, radius);
                wrapper.Region = System.Drawing.Region.FromHrgn(hRgn);
            };

            wrapper.Resize += (object? s, EventArgs e) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, wrapper.Width, wrapper.Height, radius, radius);
                wrapper.Region = System.Drawing.Region.FromHrgn(hRgn);
            };

            dgv.Parent.Controls.Remove(dgv);
            wrapper.Controls.Add(dgv);
            dgv.Location = new Point(0, 0);
            dgv.Size = wrapper.Size;

            wrapper.BringToFront();
        }

        // ─── CLOCK ───────────────────────────────────────────────────────────
        private void StartClock()
        {
            CurrentTime.Text = DateTime.Now.ToString("HH:mm:ss");
            UpdateGreeting();
            clockTimer = new System.Windows.Forms.Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();
        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            CurrentTime.Text = DateTime.Now.ToString("HH:mm:ss");
            UpdateGreeting();
        }

        private void UpdateGreeting()
        {
            int hour = DateTime.Now.Hour;
            if (hour >= 5 && hour < 12)
                AdjustLbl.Text = "Morning!";
            else if (hour >= 12 && hour < 18)
                AdjustLbl.Text = "Afternoon!";
            else
                AdjustLbl.Text = "Evening!";
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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (clockTimer != null)
            {
                clockTimer.Stop();
                clockTimer.Dispose();
            }
        }

        // ─── EMPTY STUBS ─────────────────────────────────────────────────────
        private void EmployeeDashboard_Load(object sender, EventArgs e) { }
        private void EmployeeDashboard_Load_1(object sender, EventArgs e) { }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) { }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            MakeButtonRounded(ReportsBtn, 20);
            MakePanelRounded(ProfilePanel, 20);
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

        private void button1_Click(object sender, EventArgs e) { }
        private void DateTimePicker_Paint(object? sender, PaintEventArgs e) { }

        // ─── PANEL PAINTS ────────────────────────────────────────────────────
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            MakePanelRounded(TimePanel, 20);
            TimePanel.BackColor = Color.FromArgb(40, 40, 40);

            MakeButtonRounded(TimeInBtn, 20);
            MakeButtonRounded(TimeOutBtn, 20);

            TimeInBtn.FlatStyle = FlatStyle.Flat;
            TimeInBtn.FlatAppearance.BorderSize = 0;
            TimeInBtn.BackColor = Color.FromArgb(0, 78, 0);
            TimeInBtn.ForeColor = Color.White;

            TimeOutBtn.FlatStyle = FlatStyle.Flat;
            TimeOutBtn.FlatAppearance.BorderSize = 0;
            TimeOutBtn.BackColor = Color.FromArgb(98, 0, 0);
            TimeOutBtn.ForeColor = Color.White;
        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {
            MakePanelRounded(TimeInPanel, 20);
            TimeInPanel.BackColor = Color.FromArgb(40, 40, 40);
        }

        private void TimeOutPanel_Paint(object sender, PaintEventArgs e)
        {
            MakePanelRounded(TimeOutPanel, 20);
            TimeOutPanel.BackColor = Color.FromArgb(40, 40, 40);
        }

        private void HoursPanel_Paint(object sender, PaintEventArgs e)
        {
            MakePanelRounded(HoursPanel, 20);
            HoursPanel.BackColor = Color.FromArgb(40, 40, 40);
        }

        private void StatusPanel_Paint(object sender, PaintEventArgs e)
        {
            MakePanelRounded(StatusPanel, 20);
            StatusPanel.BackColor = Color.FromArgb(40, 40, 40);
        }

        private void LogsPanel_Paint(object sender, PaintEventArgs e) { }
        private void MyDashboardBtn_Click(object sender, EventArgs e) { }
        private void panel2_Paint_2(object sender, PaintEventArgs e) { }

        private void LogOutBtn_Click(object sender, EventArgs e)
        {
            HomeForm Home = new HomeForm();
            Home.Show();
            this.Hide();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            EmployeeReports EmployeeR = new EmployeeReports();
            EmployeeR.Show();
            this.Hide();
        }
    }
}