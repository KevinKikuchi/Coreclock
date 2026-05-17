using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Coreclock
{
    public partial class AttendanceLogs : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        // ─── FIELDS ──────────────────────────────────────────────────────────
        private System.Windows.Forms.Timer clockTimer;
        private Panel dtpPanel;
        private Label dtpLabel;
        private Button _activeBtn;
        private Image _profilePhoto = null;


        // Top controls
        private ComboBox cmbViewMode;
        private ComboBox cmbWeek;
        private TextBox txtSearch;
        private Button btnGenerate;

        // Stat cards
        private Panel pnlStats;
        private Label lblTotalEmpVal;
        private Label lblTotalHrsVal;
        private Label lblAvgHrsVal;
        private Label lblAbsentVal;
        private Label lblAvgProdVal;

        // Detail panel
        private Panel pnlDetail;
        private Panel pnlDetailScroll;
        private Label lblDetailTitle;

        // Weekly detail labels (7 days)
        private Label[] lblDetailDayLabel = new Label[7];
        private Label[] lblDetailDayHours = new Label[7];
        private Label[] lblDetailDayTime = new Label[7];
        private Label[] lblDetailDayBadge = new Label[7];

        // Semi-monthly detail labels (14 days)
        private Label[] lblSMDayLabel = new Label[14];
        private Label[] lblSMDayHours = new Label[14];
        private Label[] lblSMDayTime = new Label[14];
        private Label[] lblSMDayBadge = new Label[14];

        private Label lblSumTotalHrs, lblSumTotalHrsVal;
        private Label lblSumProd, lblSumProdVal;
        private Label lblSumStatus, lblSumStatusVal;

        // ─── VIEW MODE ───────────────────────────────────────────────────────
        private bool IsSemiMonthly => cmbViewMode?.SelectedIndex == 1;

        // ─── DATA ────────────────────────────────────────────────────────────
        private struct EmpRecord
        {
            public string ID, Name;
            public string Mon, Tue, Wed, Thu, Fri, Sat, Sun;
            public string TotalHrs, Prod, Status;
            public string MonTime, TueTime, WedTime, ThuTime, FriTime;
            public string MonStatus, TueStatus, WedStatus, ThuStatus, FriStatus;
            public string[] DayHrs, DayTime, DayStatus;
            public string SMTotalHrs, SMProd, SMStatus;
        }

        private EmpRecord[] _allRecords;
        private EmpRecord[] _filtered;
        private int _selectedRow = 0;

        // ─── CONSTRUCTOR ─────────────────────────────────────────────────────
        public AttendanceLogs()
        {
            InitializeComponent();
            var screen = Screen.FromControl(this).WorkingArea;
            if (this.Width > screen.Width) this.Width = screen.Width;
            if (this.Height > screen.Height) this.Height = screen.Height;
            ApplyRoundedCorners();
            StartClock();
            StyleDateTimePicker();
            SetActiveButton(AttendanceLogsBtn);
            BuildTopControls();
            BuildStatCards();
            SetupAttendanceGrid();
            BuildDetailPanel();
            StyleProfilePanel();

            this.Load += async (s, e) =>
            {
                await BuildDataFromSupabase();
                _filtered = _allRecords;
                RefreshAll();
            };
        }
        // ─── PROFILE PANEL ────────────────────────────────────────────────────
        private void StyleProfilePanel()
        {
            ProfilePanel.Controls.Clear();
            ProfilePanel.BackColor = Color.FromArgb(30, 30, 30);

            var profile = SupabaseHelper.Instance.CurrentUserProfile;
            var nameParts = (profile?.FullName ?? "??").Split(' ');
            var initials = nameParts.Length >= 2
                ? $"{nameParts[0][0]}{nameParts[1][0]}"
                : nameParts[0].Substring(0, Math.Min(2, nameParts[0].Length));
            initials = initials.ToUpper();
            string fullName = profile?.FullName ?? "Unknown";
            string employeeId = profile?.EmployeeId ?? "000000";
            string position = profile?.Role == "admin" ? "Admin" : (profile?.Position ?? "Agent");
            string contactNumber = profile?.ContactNumber ?? "";

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
            lblInitials.Text = initials;
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
            lblName.Text = fullName;
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
            lblId.Text = employeeId;
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
            lblPos.Text = position;
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

            Label lblContact = new Label();
            lblContact.Text = "📞 " + (string.IsNullOrEmpty(contactNumber) ? "N/A" : contactNumber);
            lblContact.Font = new Font("Segoe UI", 8f);
            lblContact.ForeColor = Color.FromArgb(170, 170, 170);
            lblContact.BackColor = Color.Transparent;
            lblContact.AutoSize = false;
            lblContact.Width = ProfilePanel.Width - 10;
            lblContact.Height = 16;
            lblContact.Location = new Point(5, lblPos.Bottom + 4);
            lblContact.TextAlign = ContentAlignment.MiddleCenter;

            Panel divider = new Panel();
            divider.BackColor = Color.FromArgb(45, 45, 45);
            divider.Size = new Size(ProfilePanel.Width - 24, 1);
            divider.Location = new Point(12, lblContact.Bottom + 6);

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
            ProfilePanel.Controls.Add(lblContact);
            ProfilePanel.Controls.Add(divider);
            ProfilePanel.Controls.Add(statusDot);
            ProfilePanel.Controls.Add(lblStatus);

            btnUpload.BringToFront();
        }

        // ─── DATA DEFINITION ─────────────────────────────────────────────────
        private bool IsWorkingToday(string workDays, string dayAbbr)
        {
            if (string.IsNullOrEmpty(workDays)) return true;
            var days = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            int todayIndex = Array.IndexOf(days, dayAbbr);
            if (todayIndex < 0) return false;
            if (workDays.Contains("-"))
            {
                var parts = workDays.Split('-');
                if (parts.Length == 2)
                {
                    int start = Array.IndexOf(days, parts[0].Trim());
                    int end   = Array.IndexOf(days, parts[1].Trim());
                    if (start >= 0 && end >= 0)
                    {
                        if (start <= end) return todayIndex >= start && todayIndex <= end;
                        else return todayIndex >= start || todayIndex <= end;
                    }
                }
            }
            return workDays.Split(',').Select(d => d.Trim())
                           .Contains(dayAbbr, StringComparer.OrdinalIgnoreCase);
        }

        private async Task BuildDataFromSupabase()
        {
            var employees = await SupabaseHelper.Instance.FetchAllEmployeesAsync();
            var records = new List<EmpRecord>();

            foreach (var emp in employees)
            {
                var logs = await SupabaseHelper.Instance.FetchMyLogsAsync(emp.Id);

                DateTime periodStart = IsSemiMonthly
                    ? GetSemiMonthlyPeriodStart()
                    : GetWeeklyPeriodStart();

                int dayCount = IsSemiMonthly ? 14 : 7;
                var dayHrs    = new string[dayCount];
                var dayTime   = new string[dayCount];
                var dayStatus = new string[dayCount];

                // Skip employee if registered after this period ends
                DateTime regDate = DateTime.Today;
                if (DateTimeOffset.TryParse(emp.CreatedAt, out var parsed))
                    regDate = parsed.ToLocalTime().Date;
                DateTime periodEnd = periodStart.AddDays(dayCount - 1);
                if (regDate > periodEnd) continue;

                double totalHours = 0;
                int absentCount = 0;
                int workingDays = 0;

                for (int i = 0; i < dayCount; i++)
                {
                    DateTime day = periodStart.AddDays(i);
                    string dateStr = day.ToString("yyyy-MM-dd");
                    string dayAbbr = day.ToString("ddd");

                    if (!IsWorkingToday(emp.WorkDays, dayAbbr))
                    {
                        dayHrs[i]    = "DO";
                        dayTime[i]   = "";
                        dayStatus[i] = "dayoff";
                        continue;
                    }

                    workingDays++;
                    var log = logs.FirstOrDefault(l => l.Date == dateStr);

                    if (log == null)
                    {
                        if (day.Date >= DateTime.Today || day.Date < regDate)
                        {
                            dayHrs[i]    = "—";
                            dayTime[i]   = "";
                            dayStatus[i] = "dayoff";
                        }
                        else
                        {
                            dayHrs[i]    = "Absent";
                            dayTime[i]   = "";
                            dayStatus[i] = "absent";
                            absentCount++;
                        }
                    }
                    else if (log.Status == "Absent")
                    {
                        dayHrs[i]    = "Absent";
                        dayTime[i]   = "";
                        dayStatus[i] = "absent";
                        absentCount++;
                    }
                    else
                    {
                        double hrs = 0;
                        if (DateTime.TryParse(log.TimeIn, out var tin) &&
                            DateTime.TryParse(log.TimeOut, out var tout))
                        {
                            hrs = (tout - tin).TotalHours;
                            totalHours += hrs;
                            dayTime[i] = $"{log.TimeIn} – {log.TimeOut}";
                        }
                        else
                        {
                            dayTime[i] = log.TimeIn ?? "";
                        }

                        string hrsStr = hrs > 0 ? $"{hrs:0.##} hrs" : "—";
                        dayHrs[i] = hrsStr;
                        dayStatus[i] = hrs >= 8 ? "ontime" : (hrs > 0 ? "late" : "ontime");
                    }
                }

                double possible = workingDays * 8.0;
                double prod = possible > 0 ? (totalHours / possible * 100) : 0;
                string prodStr = $"{prod:F1}%";

                bool hasAbsent = dayStatus.Any(s => s == "absent");
                bool hasLate   = dayStatus.Any(s => s == "late");
                string overallStatus = hasAbsent ? "absent" : hasLate ? "late" : "ontime";

                int th = (int)totalHours;
                int tm = (int)((totalHours - th) * 60);

                records.Add(new EmpRecord
                {
                    ID         = emp.EmployeeId,
                    Name       = emp.FullName,
                    TotalHrs   = $"{th}h {tm}m",
                    SMTotalHrs = $"{th}h {tm}m",
                    Prod       = prodStr,
                    SMProd     = prodStr,
                    Status     = overallStatus,
                    SMStatus   = overallStatus,
                    DayHrs     = dayHrs,
                    DayTime    = dayTime,
                    DayStatus  = dayStatus,
                    Mon = dayHrs.Length > 0 ? dayHrs[0] : "—",
                    Tue = dayHrs.Length > 1 ? dayHrs[1] : "—",
                    Wed = dayHrs.Length > 2 ? dayHrs[2] : "—",
                    Thu = dayHrs.Length > 3 ? dayHrs[3] : "—",
                    Fri = dayHrs.Length > 4 ? dayHrs[4] : "—",
                    Sat = dayHrs.Length > 5 ? dayHrs[5] : "DO",
                    Sun = dayHrs.Length > 6 ? dayHrs[6] : "DO",
                });
            }

            _allRecords = records.ToArray();
            _filtered   = _allRecords;
        }

        // ─── PERIOD BUILDERS ─────────────────────────────────────────────────
        private List<string> BuildWeeklyPeriods()
        {
            var periods = new List<string>();
            DateTime weekStart = new DateTime(2026, 1, 1);
            DateTime yearEnd = new DateTime(2026, 12, 31);
            while (weekStart.AddDays(6) <= yearEnd)
            {
                periods.Add($"{weekStart:MMM d} – {weekStart.AddDays(6):MMM d}, 2026");
                weekStart = weekStart.AddDays(7);
            }
            return periods;
        }

        private List<string> BuildSemiMonthlyPeriods()
        {
            var periods = new List<string>();
            DateTime periodStart = new DateTime(2026, 1, 1);
            DateTime yearEnd = new DateTime(2026, 12, 31);
            while (periodStart.AddDays(13) <= yearEnd)
            {
                periods.Add($"{periodStart:MMM d} – {periodStart.AddDays(13):MMM d}, 2026");
                periodStart = periodStart.AddDays(14);
            }
            return periods;
        }

        // ─── TOP CONTROLS ────────────────────────────────────────────────────
        private void BuildTopControls()
        {
            Control parent = AttendanceLogsDataGridView.Parent;
            int gx = AttendanceLogsDataGridView.Left;
            int gy = AttendanceLogsDataGridView.Top;

            parent.Controls.AddRange(new Control[] {
                MakeSmallLabel("View Mode",       gx,       gy - 50),
                MakeSmallLabel("Select Period",   gx + 165, gy - 50),
                MakeSmallLabel("Search Employee", gx + 345, gy - 50)
            });

            cmbViewMode = new ComboBox();
            cmbViewMode.Items.AddRange(new object[] { "Weekly", "Semi-Monthly" });
            cmbViewMode.SelectedIndex = 0;
            StyleCombo(cmbViewMode, gx, gy - 30, 150);
            cmbViewMode.SelectedIndexChanged += CmbViewMode_SelectedIndexChanged;
            parent.Controls.Add(cmbViewMode);

            cmbWeek = new ComboBox();
            foreach (var p in BuildWeeklyPeriods()) cmbWeek.Items.Add(p);
            cmbWeek.SelectedIndex = 0;
            StyleCombo(cmbWeek, gx + 165, gy - 30, 185);
            cmbWeek.SelectedIndexChanged += CmbWeek_SelectedIndexChanged;
            parent.Controls.Add(cmbWeek);

            txtSearch = new TextBox();
            txtSearch.PlaceholderText = "Search by name or ID...";
            txtSearch.BackColor = Color.FromArgb(35, 35, 35);
            txtSearch.ForeColor = Color.White;
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Segoe UI", 9f);
            txtSearch.Location = new Point(gx + 360, gy - 30);
            txtSearch.Size = new Size(200, 24);
            txtSearch.KeyDown += async (s, e) => { if (e.KeyCode == Keys.Enter) await RunGenerateAsync(); };
            parent.Controls.Add(txtSearch);

            btnGenerate = new Button();
            btnGenerate.Text = "⟳  Generate";
            btnGenerate.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btnGenerate.BackColor = Color.FromArgb(200, 168, 75);
            btnGenerate.ForeColor = Color.FromArgb(20, 20, 20);
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.Location = new Point(gx + 572, gy - 32);
            btnGenerate.Size = new Size(110, 28);
            btnGenerate.Cursor = Cursors.Hand;
            btnGenerate.Click += async (s, e) => await RunGenerateAsync();
            IntPtr hRgn = CreateRoundRectRgn(0, 0, btnGenerate.Width, btnGenerate.Height, 12, 12);
            btnGenerate.Region = System.Drawing.Region.FromHrgn(hRgn);
            parent.Controls.Add(btnGenerate);
        }

        private async void CmbViewMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbWeek.Items.Clear();
            foreach (var p in IsSemiMonthly ? BuildSemiMonthlyPeriods() : BuildWeeklyPeriods())
                cmbWeek.Items.Add(p);
            if (cmbWeek.Items.Count > 0) cmbWeek.SelectedIndex = 0;
            SetupAttendanceGrid();
            RebuildDetailPanelForMode();
            await BuildDataFromSupabase();
            _filtered = _allRecords;
            RefreshAll();
        }

        private async void CmbWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupAttendanceGrid();
            await BuildDataFromSupabase();
            _filtered = _allRecords;
            RefreshAll();
        }

        private Label MakeSmallLabel(string text, int x, int y)
        {
            return new Label { Text = text, Font = new Font("Segoe UI", 8f), ForeColor = Color.FromArgb(160, 160, 160), BackColor = Color.Transparent, AutoSize = true, Location = new Point(x, y) };
        }

        private void StyleCombo(ComboBox cmb, int x, int y, int width)
        {
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.BackColor = Color.FromArgb(35, 35, 35);
            cmb.ForeColor = Color.White;
            cmb.Font = new Font("Segoe UI", 9f);
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.Location = new Point(x, y);
            cmb.Size = new Size(width, 24);
        }

        // ─── STAT CARDS ──────────────────────────────────────────────────────
        private void BuildStatCards()
        {
            Control parent = AttendanceLogsDataGridView.Parent;
            int gx = AttendanceLogsDataGridView.Left;
            int panelY = AttendanceLogsDataGridView.Top - 110;

            pnlStats = new Panel { Location = new Point(gx, panelY), Size = new Size(AttendanceLogsDataGridView.Width, 52), BackColor = Color.Transparent };
            parent.Controls.Add(pnlStats);

            int cardW = (pnlStats.Width - 32) / 5;
            string[] titles = { "Total Employees", "Total Hours\n(This Period)", "Average Hours\nPer Employee", "Absent\n(This Period)", "Avg Productivity\nThis Period" };
            Color[] colors = { Color.FromArgb(76, 175, 146), Color.FromArgb(136, 136, 204), Color.FromArgb(200, 168, 75), Color.FromArgb(192, 57, 43), Color.FromArgb(76, 175, 146) };

            for (int i = 0; i < 5; i++)
            {
                Panel card = new Panel { Location = new Point(i * (cardW + 8), 0), Size = new Size(cardW, 52), BackColor = Color.FromArgb(28, 28, 28) };
                IntPtr r = CreateRoundRectRgn(0, 0, card.Width, card.Height, 12, 12);
                card.Region = System.Drawing.Region.FromHrgn(r);
                pnlStats.Controls.Add(card);

                card.Controls.Add(new Label { Text = titles[i], Font = new Font("Segoe UI", 7f), ForeColor = Color.FromArgb(140, 140, 140), BackColor = Color.Transparent, AutoSize = false, Size = new Size(cardW - 8, 28), Location = new Point(4, 2), TextAlign = ContentAlignment.BottomCenter });

                Label val = new Label { Font = new Font("Segoe UI", 11f, FontStyle.Bold), ForeColor = colors[i], BackColor = Color.Transparent, AutoSize = false, Size = new Size(cardW - 8, 22), Location = new Point(4, 28), TextAlign = ContentAlignment.TopCenter };
                card.Controls.Add(val);

                switch (i)
                {
                    case 0: lblTotalEmpVal = val; break;
                    case 1: lblTotalHrsVal = val; break;
                    case 2: lblAvgHrsVal = val; break;
                    case 3: lblAbsentVal = val; break;
                    case 4: lblAvgProdVal = val; break;
                }
            }
        }

        private void UpdateStatCards()
        {
            int absent = 0;
            double totalH = 0, totalP = 0;
            foreach (var r in _filtered)
            {
                string statusToCheck = IsSemiMonthly ? r.SMStatus : r.Status;
                string hrsStr = IsSemiMonthly ? r.SMTotalHrs : r.TotalHrs;
                string prodStr = IsSemiMonthly ? r.SMProd : r.Prod;
                if (statusToCheck == "absent") absent++;
                if (double.TryParse(hrsStr.Replace("hrs", "").Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double h)) totalH += h;
                if (double.TryParse(prodStr.Replace("%", "").Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double p)) totalP += p;
            }
            int count = _filtered.Length;
            double avgH = count > 0 ? totalH / count : 0;
            double avgP = count > 0 ? totalP / count : 0;
            int hh = (int)totalH, hm = (int)Math.Round((totalH - hh) * 60);
            int ah = (int)avgH, am = (int)Math.Round((avgH - ah) * 60);
            lblTotalEmpVal.Text = count.ToString();
            lblTotalHrsVal.Text = $"{hh}h {hm}m";
            lblAvgHrsVal.Text = $"{ah}h {am}m";
            lblAbsentVal.Text = absent.ToString();
            lblAvgProdVal.Text = $"{avgP:F1}%";
        }

        // ─── DETAIL PANEL ────────────────────────────────────────────────────
        private void BuildDetailPanel()
        {
            Control parent = AttendanceLogsDataGridView.Parent;
            int dgvBottom = AttendanceLogsDataGridView.Bottom + 10;
            int gx = AttendanceLogsDataGridView.Left;

            pnlDetail = new Panel { Location = new Point(gx, dgvBottom), Size = new Size(AttendanceLogsDataGridView.Width, 145), BackColor = Color.FromArgb(25, 25, 15) };
            IntPtr rr = CreateRoundRectRgn(0, 0, pnlDetail.Width, pnlDetail.Height, 14, 14);
            pnlDetail.Region = System.Drawing.Region.FromHrgn(rr);
            parent.Controls.Add(pnlDetail);

            lblDetailTitle = new Label { Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Color.FromArgb(200, 168, 75), BackColor = Color.Transparent, AutoSize = true, Location = new Point(10, 8) };
            pnlDetail.Controls.Add(lblDetailTitle);

            pnlDetailScroll = new Panel { Location = new Point(0, 26), Size = new Size(pnlDetail.Width, pnlDetail.Height - 26), BackColor = Color.Transparent, AutoScroll = false };
            pnlDetail.Controls.Add(pnlDetailScroll);

            BuildWeeklyCards();
            BuildSemiMonthlyCards();
            BuildSummaryCard();
            ShowWeeklyCards();
        }

        private void BuildWeeklyCards()
        {
            string[] dayNames = { "MON", "TUE", "WED", "THU", "FRI", "SAT", "SUN" };
            int cardW = 76, cardH = 105, startX = 8, startY = 4, gap = 5;
            for (int i = 0; i < 7; i++)
            {
                Panel dc = new Panel { Name = $"wkCard{i}", Location = new Point(startX + i * (cardW + gap), startY), Size = new Size(cardW, cardH), BackColor = Color.FromArgb(35, 35, 25) };
                IntPtr dr = CreateRoundRectRgn(0, 0, dc.Width, dc.Height, 10, 10);
                dc.Region = System.Drawing.Region.FromHrgn(dr);
                pnlDetailScroll.Controls.Add(dc);

                lblDetailDayLabel[i] = new Label { Text = dayNames[i], Font = new Font("Segoe UI", 7f), ForeColor = Color.FromArgb(140, 140, 140), BackColor = Color.Transparent, AutoSize = false, Size = new Size(cardW, 14), Location = new Point(0, 4), TextAlign = ContentAlignment.TopCenter };
                lblDetailDayHours[i] = new Label { Font = new Font("Segoe UI", 9f, FontStyle.Bold), BackColor = Color.Transparent, AutoSize = false, Size = new Size(cardW, 16), Location = new Point(0, 20), TextAlign = ContentAlignment.TopCenter };
                lblDetailDayTime[i] = new Label { Font = new Font("Segoe UI", 6f), ForeColor = Color.FromArgb(110, 110, 90), BackColor = Color.Transparent, AutoSize = false, Size = new Size(cardW - 4, 26), Location = new Point(2, 38), TextAlign = ContentAlignment.TopCenter };
                lblDetailDayBadge[i] = new Label { Font = new Font("Segoe UI", 7f, FontStyle.Bold), AutoSize = false, Size = new Size(cardW - 10, 18), Location = new Point(5, 76), TextAlign = ContentAlignment.MiddleCenter };
                dc.Controls.AddRange(new Control[] { lblDetailDayLabel[i], lblDetailDayHours[i], lblDetailDayTime[i], lblDetailDayBadge[i] });
            }
        }

        private void BuildSemiMonthlyCards()
        {
            int cardW = 64, cardH = 105, startX = 8, startY = 4, gap = 4;
            for (int i = 0; i < 14; i++)
            {
                Panel dc = new Panel { Name = $"smCard{i}", Location = new Point(startX + i * (cardW + gap), startY), Size = new Size(cardW, cardH), BackColor = Color.FromArgb(35, 35, 25), Visible = false };
                IntPtr dr = CreateRoundRectRgn(0, 0, dc.Width, dc.Height, 10, 10);
                dc.Region = System.Drawing.Region.FromHrgn(dr);
                pnlDetailScroll.Controls.Add(dc);

                lblSMDayLabel[i] = new Label { Font = new Font("Segoe UI", 6.5f), ForeColor = Color.FromArgb(140, 140, 140), BackColor = Color.Transparent, AutoSize = false, Size = new Size(cardW, 14), Location = new Point(0, 4), TextAlign = ContentAlignment.TopCenter };
                lblSMDayHours[i] = new Label { Font = new Font("Segoe UI", 8.5f, FontStyle.Bold), BackColor = Color.Transparent, AutoSize = false, Size = new Size(cardW, 16), Location = new Point(0, 20), TextAlign = ContentAlignment.TopCenter };
                lblSMDayTime[i] = new Label { Font = new Font("Segoe UI", 5.5f), ForeColor = Color.FromArgb(110, 110, 90), BackColor = Color.Transparent, AutoSize = false, Size = new Size(cardW - 2, 28), Location = new Point(1, 38), TextAlign = ContentAlignment.TopCenter };
                lblSMDayBadge[i] = new Label { Font = new Font("Segoe UI", 6.5f, FontStyle.Bold), AutoSize = false, Size = new Size(cardW - 8, 16), Location = new Point(4, 78), TextAlign = ContentAlignment.MiddleCenter };
                dc.Controls.AddRange(new Control[] { lblSMDayLabel[i], lblSMDayHours[i], lblSMDayTime[i], lblSMDayBadge[i] });
            }
        }

        private void BuildSummaryCard()
        {
            Panel sumCard = new Panel { Name = "sumCard", BackColor = Color.FromArgb(35, 35, 25), Size = new Size(120, 105) };
            IntPtr sr2 = CreateRoundRectRgn(0, 0, sumCard.Width, sumCard.Height, 10, 10);
            sumCard.Region = System.Drawing.Region.FromHrgn(sr2);
            pnlDetailScroll.Controls.Add(sumCard);
            AddSummaryRow(sumCard, 4, "Total Hours", out lblSumTotalHrs, out lblSumTotalHrsVal);
            AddSummaryRow(sumCard, 36, "Productivity", out lblSumProd, out lblSumProdVal);
            AddSummaryRow(sumCard, 68, "Status", out lblSumStatus, out lblSumStatusVal);
        }

        private void PositionSummaryCard(int afterX)
        {
            foreach (Control c in pnlDetailScroll.Controls)
                if (c.Name == "sumCard") { c.Location = new Point(afterX + 6, 4); break; }
        }

        private void ShowWeeklyCards()
        {
            for (int i = 0; i < 7; i++) { var c = pnlDetailScroll.Controls[$"wkCard{i}"]; if (c != null) c.Visible = true; }
            for (int i = 0; i < 14; i++) { var c = pnlDetailScroll.Controls[$"smCard{i}"]; if (c != null) c.Visible = false; }
            PositionSummaryCard(8 + 7 * (76 + 5));
        }

        private void ShowSemiMonthlyCards()
        {
            for (int i = 0; i < 7; i++) { var c = pnlDetailScroll.Controls[$"wkCard{i}"]; if (c != null) c.Visible = false; }
            for (int i = 0; i < 14; i++) { var c = pnlDetailScroll.Controls[$"smCard{i}"]; if (c != null) c.Visible = true; }
            PositionSummaryCard(8 + 14 * (64 + 4));
        }

        private void RebuildDetailPanelForMode()
        {
            if (IsSemiMonthly) ShowSemiMonthlyCards();
            else ShowWeeklyCards();
        }

        private void AddSummaryRow(Panel parent, int y, string label, out Label lbl, out Label val)
        {
            lbl = new Label { Text = label, Font = new Font("Segoe UI", 7.5f), ForeColor = Color.FromArgb(140, 140, 140), BackColor = Color.Transparent, AutoSize = false, Size = new Size(parent.Width / 2 - 2, 20), Location = new Point(4, y), TextAlign = ContentAlignment.MiddleLeft };
            val = new Label { Font = new Font("Segoe UI", 8f, FontStyle.Bold), BackColor = Color.Transparent, AutoSize = false, Size = new Size(parent.Width / 2, 20), Location = new Point(parent.Width / 2, y), TextAlign = ContentAlignment.MiddleRight };
            parent.Controls.AddRange(new Control[] { lbl, val });
        }

        private void UpdateDetailPanel()
        {
            if (_filtered == null || _filtered.Length == 0 || _selectedRow >= _filtered.Length) return;
            var r = _filtered[_selectedRow];

            lblDetailTitle.Text = IsSemiMonthly
                ? $"Semi-Monthly Details:  {r.Name}  ({r.ID})"
                : $"Weekly Details:  {r.Name}  ({r.ID})";

            if (IsSemiMonthly) UpdateSemiMonthlyDetail(r);
            else UpdateWeeklyDetail(r);

            string totalHrs = IsSemiMonthly ? r.SMTotalHrs : r.TotalHrs;
            string prod = IsSemiMonthly ? r.SMProd : r.Prod;
            string status = IsSemiMonthly ? r.SMStatus : r.Status;

            lblSumTotalHrsVal.Text = totalHrs;
            lblSumTotalHrsVal.ForeColor = Color.FromArgb(76, 175, 146);
            lblSumProdVal.Text = prod;
            lblSumProdVal.ForeColor = ProdColor(prod);
            StyleBadgeLabel(lblSumStatusVal, status);
        }

        private void UpdateWeeklyDetail(EmpRecord r)
        {
            DateTime periodStart = GetWeeklyPeriodStart();
            int dayCount = GetWeeklyDayCount(periodStart);
            for (int i = 0; i < 7; i++)
            {
                var card = pnlDetailScroll.Controls[$"wkCard{i}"];
                if (i < dayCount && i < r.DayHrs.Length)
                {
                    DateTime d = periodStart.AddDays(i);
                    lblDetailDayLabel[i].Text = $"{d:ddd}\n{d.Day}".ToUpper();
                    lblDetailDayHours[i].Text = r.DayHrs[i];
                    lblDetailDayHours[i].ForeColor = StatusColor(r.DayStatus[i]);
                    lblDetailDayTime[i].Text = r.DayTime[i];
                    StyleBadgeLabel(lblDetailDayBadge[i], r.DayStatus[i]);
                    if (card != null) card.Visible = true;
                }
                else { if (card != null) card.Visible = false; }
            }
        }

        private void UpdateSemiMonthlyDetail(EmpRecord r)
        {
            DateTime periodStart = GetSemiMonthlyPeriodStart();
            int dayCount = Math.Min(GetSemiMonthlyDayCount(periodStart), r.DayHrs.Length);
            for (int i = 0; i < 14; i++)
            {
                var card = pnlDetailScroll.Controls[$"smCard{i}"];
                if (i < dayCount)
                {
                    DateTime d = periodStart.AddDays(i);
                    lblSMDayLabel[i].Text = $"{d:ddd}\n{d.Day}".ToUpper();
                    lblSMDayHours[i].Text = r.DayHrs[i];
                    lblSMDayHours[i].ForeColor = StatusColor(r.DayStatus[i]);
                    lblSMDayTime[i].Text = r.DayTime[i];
                    StyleBadgeLabel(lblSMDayBadge[i], r.DayStatus[i]);
                    if (card != null) card.Visible = true;
                }
                else { if (card != null) card.Visible = false; }
            }
        }

        // ─── HELPERS ─────────────────────────────────────────────────────────
        private Color StatusColor(string s) => s switch
        {
            "ontime" => Color.FromArgb(76, 175, 146),
            "late" => Color.FromArgb(200, 168, 75),
            "absent" => Color.FromArgb(192, 57, 43),
            _ => Color.FromArgb(100, 100, 100)
        };

        private Color ProdColor(string prod)
        {
            if (double.TryParse(prod.Replace("%", "").Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double v))
            {
                if (v >= 99) return Color.FromArgb(76, 175, 146);
                if (v >= 90) return Color.FromArgb(200, 168, 75);
                return Color.FromArgb(192, 57, 43);
            }
            return Color.White;
        }

        private void StyleBadgeLabel(Label lbl, string status)
        {
            switch (status)
            {
                case "ontime": lbl.Text = "On Time"; lbl.BackColor = Color.FromArgb(20, 60, 40); lbl.ForeColor = Color.FromArgb(76, 175, 146); break;
                case "late": lbl.Text = "Late"; lbl.BackColor = Color.FromArgb(60, 50, 10); lbl.ForeColor = Color.FromArgb(200, 168, 75); break;
                case "absent": lbl.Text = "Absent"; lbl.BackColor = Color.FromArgb(55, 15, 15); lbl.ForeColor = Color.FromArgb(192, 57, 43); break;
                default: lbl.Text = "Day Off"; lbl.BackColor = Color.FromArgb(30, 30, 55); lbl.ForeColor = Color.FromArgb(136, 136, 204); break;
            }
            IntPtr h = CreateRoundRectRgn(0, 0, lbl.Width, lbl.Height, 8, 8);
            lbl.Region = System.Drawing.Region.FromHrgn(h);
        }

        // ─── GENERATE / FILTER ───────────────────────────────────────────────
        private async Task RunGenerateAsync()
        {
            await BuildDataFromSupabase();
            string q = txtSearch?.Text?.Trim().ToLower() ?? "";
            _filtered = string.IsNullOrEmpty(q)
                ? _allRecords
                : Array.FindAll(_allRecords, r => r.Name.ToLower().Contains(q) || r.ID.ToLower().Contains(q));
            _selectedRow = 0;
            RefreshAll();
        }

        private void RefreshAll()
        {
            LoadAttendanceData();
            UpdateStatCards();
            UpdateDetailPanel();
        }

        // ─── CLOCK ───────────────────────────────────────────────────────────
        private void StartClock()
        {
            CurrentTime.Text = DateTime.Now.ToString("HH:mm:ss");
            UpdateGreeting();
            clockTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();
        }

        private void ClockTimer_Tick(object? sender, EventArgs e)
        {
            CurrentTime.Text = DateTime.Now.ToString("HH:mm:ss");
            UpdateGreeting();
        }

        private void UpdateGreeting()
        {
            int hour = DateTime.Now.Hour;
            if (hour >= 5 && hour < 12) AdjustLbl.Text = "Morning!";
            else if (hour >= 12 && hour < 18) AdjustLbl.Text = "Afternoon!";
            else AdjustLbl.Text = "Evening!";
        }

        // ─── FAKE DATE TIME PICKER ───────────────────────────────────────────
        private void StyleDateTimePicker()
        {
            DateTimePicker.Visible = false;

            dtpPanel = new Panel { Size = new Size(DateTimePicker.Width, DateTimePicker.Height), Location = DateTimePicker.Location, BackColor = Color.FromArgb(40, 40, 40), Cursor = Cursors.Hand };
            dtpPanel.HandleCreated += (object? s, EventArgs e) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, dtpPanel.Width, dtpPanel.Height, 20, 20);
                dtpPanel.Region = System.Drawing.Region.FromHrgn(hRgn);
            };

            dtpLabel = new Label { AutoSize = false, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.White, BackColor = Color.Transparent, Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold), Text = DateTimePicker.Value.ToString("dddd, MMMM d, yyyy") };

            dtpPanel.Click += (object? s, EventArgs e) => { DateTimePicker.Location = dtpPanel.Location; DateTimePicker.Visible = true; DateTimePicker.Focus(); };
            dtpLabel.Click += (object? s, EventArgs e) => { DateTimePicker.Location = dtpPanel.Location; DateTimePicker.Visible = true; DateTimePicker.Focus(); };
            DateTimePicker.ValueChanged += (object? s, EventArgs e) => { dtpLabel.Text = DateTimePicker.Value.ToString("dddd, MMMM d, yyyy"); DateTimePicker.Visible = false; };
            DateTimePicker.Leave += (object? s, EventArgs e) => { DateTimePicker.Visible = false; };

            DateTimePicker.CalendarMonthBackground = Color.FromArgb(40, 40, 40);
            DateTimePicker.CalendarForeColor = Color.White;
            DateTimePicker.CalendarTitleBackColor = Color.FromArgb(20, 20, 20);
            DateTimePicker.CalendarTitleForeColor = Color.White;
            DateTimePicker.CalendarTrailingForeColor = Color.Gray;

            dtpPanel.Controls.Add(dtpLabel);
            DateTimePicker.Parent.Controls.Add(dtpPanel);
            dtpPanel.BringToFront();
        }

        // ─── ACTIVE BUTTON ───────────────────────────────────────────────────
        private void SetActiveButton(Button activeBtn)
        {
            _activeBtn = activeBtn;
            Button[] allButtons = { AdminDashboardBtn, EmployeBtn, AttendanceLogsBtn, ReportBtn };
            foreach (Button btn in allButtons)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Color.FromArgb(20, 20, 20);
                btn.ForeColor = Color.White;
            }
            activeBtn.BackColor = Color.FromArgb(55, 55, 55);
            activeBtn.ForeColor = Color.White;
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
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyRoundedCorners();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (clockTimer != null) { clockTimer.Stop(); clockTimer.Dispose(); }
        }

        // ─── STUBS ───────────────────────────────────────────────────────────
        private void AttendanceLogs_Load(object sender, EventArgs e) { }

        // ─── SIDEBAR PAINT ───────────────────────────────────────────────────
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            MakeButtonRounded(AttendanceLogsBtn, 20);
            MakeButtonRounded(LogOutBtn, 20);
            MakeButtonRounded(AdminDashboardBtn, 20);
            MakeButtonRounded(EmployeBtn, 20);
            MakeButtonRounded(ReportBtn, 20);

            LogOutBtn.FlatStyle = FlatStyle.Flat;
            LogOutBtn.FlatAppearance.BorderSize = 0;
            LogOutBtn.BackColor = Color.FromArgb(20, 20, 20);
            LogOutBtn.ForeColor = Color.White;

            SetActiveButton(_activeBtn);
        }

        // ─── NAVIGATION ──────────────────────────────────────────────────────
        private void AdminDashboardBtn_Click(object sender, EventArgs e) { new AdminDashboard().Show(); this.Hide(); }
        private void EmployeBtn_Click(object sender, EventArgs e) { new EmployeeSchedule().Show(); this.Hide(); }
        private void AttendanceLogsBtn_Click(object sender, EventArgs e) { }
        private void ReportBtn_Click(object sender, EventArgs e) { new AdminReports().Show(); this.Hide(); }
        private void LogOutBtn_Click(object sender, EventArgs e) { new HomeForm().Show(); this.Hide(); }

        // ─── PERIOD HELPERS ──────────────────────────────────────────────────
        private DateTime GetSemiMonthlyPeriodStart()
        {
            string period = cmbWeek?.SelectedItem?.ToString() ?? "";
            try
            {
                string[] parts = period.Split('–');
                string startPart = parts[0].Trim();
                string yearStr = parts.Length > 1 ? parts[1].Trim().Split(' ')[^1].Replace(",", "") : DateTime.Now.Year.ToString();
                return DateTime.Parse($"{startPart}, {yearStr}");
            }
            catch { return new DateTime(DateTime.Now.Year, 1, 1); }
        }

        private int GetSemiMonthlyDayCount(DateTime periodStart)
        {
            DateTime periodEnd = periodStart.AddDays(13);
            DateTime yearEnd = new DateTime(periodStart.Year, 12, 31);
            if (periodEnd > yearEnd) periodEnd = yearEnd;
            return (periodEnd - periodStart).Days + 1;
        }

        private DateTime GetWeeklyPeriodStart()
        {
            string period = cmbWeek?.SelectedItem?.ToString() ?? "";
            try
            {
                string[] parts = period.Split('–');
                string startPart = parts[0].Trim();
                string yearStr = parts.Length > 1 ? parts[1].Trim().Split(' ')[^1].Replace(",", "") : DateTime.Now.Year.ToString();
                return DateTime.Parse($"{startPart}, {yearStr}");
            }
            catch { return new DateTime(DateTime.Now.Year, 1, 1); }
        }

        private int GetWeeklyDayCount(DateTime periodStart)
        {
            DateTime periodEnd = periodStart.AddDays(6);
            DateTime yearEnd = new DateTime(periodStart.Year, 12, 31);
            if (periodEnd > yearEnd) periodEnd = yearEnd;
            return (periodEnd - periodStart).Days + 1;
        }

        // ─── DATAGRIDVIEW SETUP ──────────────────────────────────────────────
        private void SetupAttendanceGrid()
        {
            var dgv = AttendanceLogsDataGridView;

            // ── ROUNDED CORNERS ON GRID ──
            dgv.HandleCreated += (object? s, EventArgs ev) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, dgv.Width, dgv.Height, 14, 14);
                dgv.Region = System.Drawing.Region.FromHrgn(hRgn);
            };

            dgv.BackgroundColor = Color.FromArgb(20, 20, 20);
            dgv.GridColor = Color.FromArgb(35, 35, 35);
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.RowHeadersVisible = true;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(170, 170, 170);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(0, 6, 0, 6);
            dgv.ColumnHeadersHeight = 36;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dgv.DefaultCellStyle.BackColor = Color.FromArgb(28, 28, 28);
            dgv.DefaultCellStyle.ForeColor = Color.FromArgb(220, 220, 220);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 35);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Padding = new Padding(0, 6, 0, 6);
            dgv.RowTemplate.Height = 44;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(24, 24, 24);
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 35);

            dgv.CellPainting -= AttendanceGrid_CellPainting;
            dgv.Columns.Clear();

            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "EMP ID", Name = "EmpID", FillWeight = 55f, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "NAME", Name = "Name", FillWeight = 100f, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleLeft } });

            if (IsSemiMonthly)
            {
                DateTime periodStart = GetSemiMonthlyPeriodStart();
                int dayCount = GetSemiMonthlyDayCount(periodStart);
                for (int i = 0; i < dayCount; i++)
                {
                    DateTime d = periodStart.AddDays(i);
                    dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = d.ToString("MMM d").ToUpper(), Name = $"Day{i}", FillWeight = 38f, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
                }
            }
            else
            {
                DateTime periodStart = GetWeeklyPeriodStart();
                int dayCount = GetWeeklyDayCount(periodStart);
                for (int i = 0; i < dayCount; i++)
                {
                    DateTime d = periodStart.AddDays(i);
                    dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = d.ToString("MMM d").ToUpper(), Name = $"Day{i}", FillWeight = 50f, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
                }
            }

            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "TOTAL HOURS", Name = "TotalHours", FillWeight = 70f, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "PRODUCTIVITY", Name = "Productivity", FillWeight = 70f, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });

            if (!IsSemiMonthly)
                dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "STATUS", Name = "Status", FillWeight = 70f, SortMode = DataGridViewColumnSortMode.NotSortable, DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter } });

            dgv.CellPainting += AttendanceGrid_CellPainting;
            dgv.SelectionChanged += (s, e) =>
            {
                if (dgv.SelectedRows.Count > 0) { _selectedRow = dgv.SelectedRows[0].Index; UpdateDetailPanel(); }
            };
        }

        private void LoadAttendanceData()
        {
            var dgv = AttendanceLogsDataGridView;
            dgv.Rows.Clear();

            if (IsSemiMonthly)
            {
                DateTime periodStart = GetSemiMonthlyPeriodStart();
                int dayCount = GetSemiMonthlyDayCount(periodStart);
                foreach (var r in _filtered)
                {
                    var values = new List<object> { r.ID, r.Name };
                    for (int i = 0; i < dayCount; i++) values.Add(i < r.DayHrs.Length ? r.DayHrs[i] : "");
                    values.Add(r.SMTotalHrs);
                    values.Add(r.SMProd);
                    dgv.Rows.Add(values.ToArray());
                }
            }
            else
            {
                DateTime periodStart = GetWeeklyPeriodStart();
                int dayCount = GetWeeklyDayCount(periodStart);
                foreach (var r in _filtered)
                {
                    string statusLabel = r.Status switch { "ontime" => "On Time", "late" => "Late", "absent" => "Absent", "dayoff" => "Day Off", _ => r.Status };
                    var values = new List<object> { r.ID, r.Name };
                    for (int i = 0; i < dayCount; i++) values.Add(i < r.DayHrs.Length ? r.DayHrs[i] : "");
                    values.Add(r.TotalHrs);
                    values.Add(r.Prod);
                    values.Add(statusLabel);
                    dgv.Rows.Add(values.ToArray());
                }
            }

            if (dgv.Rows.Count > 0 && _selectedRow < dgv.Rows.Count)
                dgv.Rows[_selectedRow].Selected = true;
        }

        // ─── CELL PAINTER ────────────────────────────────────────────────────
        private void AttendanceGrid_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;
            string val = e.Value.ToString()!.Trim();
            string colName = AttendanceLogsDataGridView.Columns[e.ColumnIndex].Name;
            bool isOnTime = val == "8 hrs";
            bool isLate = IsLateValue(val);
            bool isDayOff = val == "DO";
            bool isAbsent = val == "Absent";
            bool isFullProd = val == "100%";
            bool isPartProd = val.EndsWith("%") && !isFullProd;

            if (colName == "Status")
            {
                e.Handled = true;
                bool isSel = (e.State & DataGridViewElementStates.Selected) != 0;
                Color bg = isSel ? Color.FromArgb(55, 55, 35) : (e.RowIndex % 2 == 0 ? Color.FromArgb(28, 28, 28) : Color.FromArgb(24, 24, 24));
                using (SolidBrush br = new SolidBrush(bg)) e.Graphics.FillRectangle(br, e.CellBounds);
                Color txtColor, badgeBg;
                switch (val)
                {
                    case "On Time": txtColor = Color.FromArgb(76, 175, 146); badgeBg = Color.FromArgb(20, 60, 40); break;
                    case "Late": txtColor = Color.FromArgb(200, 168, 75); badgeBg = Color.FromArgb(60, 50, 10); break;
                    case "Absent": txtColor = Color.FromArgb(192, 57, 43); badgeBg = Color.FromArgb(55, 15, 15); break;
                    default: txtColor = Color.FromArgb(136, 136, 204); badgeBg = Color.FromArgb(30, 30, 55); break;
                }
                Rectangle b = e.CellBounds;
                Rectangle badge = new Rectangle(b.X + (b.Width - 68) / 2, b.Y + (b.Height - 20) / 2, 68, 20);
                using (GraphicsPath gp = RoundedRect(badge, 6))
                using (SolidBrush bb = new SolidBrush(badgeBg)) e.Graphics.FillPath(bb, gp);
                TextRenderer.DrawText(e.Graphics, val, new Font("Segoe UI", 8f, FontStyle.Bold), badge, txtColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                using (Pen border = new Pen(Color.FromArgb(35, 35, 35))) e.Graphics.DrawLine(border, b.Left, b.Bottom - 1, b.Right, b.Bottom - 1);
                return;
            }

            if (isLate)
            {
                e.Handled = true;
                bool isSel = (e.State & DataGridViewElementStates.Selected) != 0;
                using (SolidBrush br = new SolidBrush(isSel ? Color.FromArgb(80, 70, 20) : Color.FromArgb(50, 44, 10))) e.Graphics.FillRectangle(br, e.CellBounds);
                using (Pen bp = new Pen(Color.FromArgb(35, 35, 35))) e.Graphics.DrawLine(bp, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                TextRenderer.DrawText(e.Graphics, val, new Font("Segoe UI", 9f, FontStyle.Bold), e.CellBounds, Color.FromArgb(255, 220, 60), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            if (isAbsent)
            {
                e.Handled = true;
                bool isSel = (e.State & DataGridViewElementStates.Selected) != 0;
                using (SolidBrush br = new SolidBrush(isSel ? Color.FromArgb(80, 20, 20) : Color.FromArgb(50, 15, 15))) e.Graphics.FillRectangle(br, e.CellBounds);
                using (Pen bp = new Pen(Color.FromArgb(35, 35, 35))) e.Graphics.DrawLine(bp, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
                TextRenderer.DrawText(e.Graphics, val, new Font("Segoe UI", 9f, FontStyle.Bold), e.CellBounds, Color.FromArgb(220, 80, 80), TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                return;
            }

            e.Handled = false;
            if (isOnTime) { e.CellStyle.ForeColor = Color.FromArgb(76, 175, 146); e.CellStyle.SelectionForeColor = Color.FromArgb(76, 175, 146); }
            else if (isDayOff) { e.CellStyle.ForeColor = Color.FromArgb(100, 100, 100); e.CellStyle.SelectionForeColor = Color.FromArgb(100, 100, 100); }
            else if (isFullProd) { e.CellStyle.ForeColor = Color.FromArgb(76, 175, 146); e.CellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold); e.CellStyle.SelectionForeColor = Color.FromArgb(76, 175, 146); }
            else if (isPartProd) { e.CellStyle.ForeColor = Color.FromArgb(255, 220, 60); e.CellStyle.SelectionForeColor = Color.FromArgb(255, 220, 60); }
        }

        private bool IsLateValue(string val)
        {
            if (val == "8 hrs" || val == "DO" || val == "Absent") return false;
            if (!val.EndsWith("hrs")) return false;
            if (double.TryParse(val.Replace("hrs", "").Trim(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double hours))
                return hours < 8.0;
            return false;
        }

        private GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            int d = radius * 2;
            var path = new GraphicsPath();
            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}