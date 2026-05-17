using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Coreclock
{
    public partial class AdminDashboard : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        private System.Windows.Forms.Timer clockTimer;
        private System.Windows.Forms.Timer _midnightTimer;
        private DateTime _lastLoadedDate = DateTime.Today;
        private Panel dtpPanel;
        private Label dtpLabel;
        private Button _activeBtn;
        private Image _profilePhoto = null;


        private TextBox employeeSearchBox;
        private List<object[]> allEmployeeRows = new List<object[]>();

        private Control _empGridOriginalParent;
        private Point _empGridOriginalLocation;
        private Size _empGridOriginalSize;

        public AdminDashboard()
        {
            InitializeComponent();
            var screen = Screen.FromControl(this).WorkingArea;
            if (this.Width > screen.Width) this.Width = screen.Width;
            if (this.Height > screen.Height) this.Height = screen.Height;
            CheckSession();
            ApplyRoundedCorners();
            StartClock();
            StyleDateTimePicker();
            StyleEmployeeGrid();
            StyleProfilePanel();
            StyleAttendanceGrid();

            _empGridOriginalParent = EmployeeDataGridView.Parent;
            _empGridOriginalLocation = EmployeeDataGridView.Location;
            _empGridOriginalSize = EmployeeDataGridView.Size;

            WrapGridWithDarkScroll(EmployeeDataGridView);
            WrapGridWithDarkScroll(AttendanceLogsDataGridView);
            AddEmployeeSearchBar();

            SetActiveButton(AdminDashboardBtn);

            // Load real employees from Supabase after form is shown
            this.Load += async (s, e) =>
            {
                await SupabaseHelper.Instance.MarkAbsentIfLateAsync();
                await LoadEmployeesFromSupabase();
                await LoadTodaysShiftAsync();
                StartMidnightTimer();
            };
        }

        private void CheckSession()
        {
            var session = SupabaseHelper.Instance.CurrentSession;
            if (session == null)
            {
                LoginFrm login = new LoginFrm();
                login.Show();
                this.Hide();
                return;
            }

            if (SupabaseHelper.Instance.CurrentUserProfile?.Role != "admin")
            {
                EmployeeDashboard empDash = new EmployeeDashboard();
                empDash.Show();
                this.Hide();
            }
        }

        private void StartMidnightTimer()
        {
            _midnightTimer = new System.Windows.Forms.Timer();
            _midnightTimer.Interval = 60000;
            _midnightTimer.Tick += async (s, e) =>
            {
                if (DateTime.Today > _lastLoadedDate)
                {
                    _lastLoadedDate = DateTime.Today;
                    await SupabaseHelper.Instance.MarkAbsentIfLateAsync();
                    await LoadEmployeesFromSupabase();
                    await LoadTodaysShiftAsync();
                }
            };
            _midnightTimer.Start();
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

        // ─── EMPLOYEE GRID ───────────────────────────────────────────────────
        private void StyleEmployeeGrid()
        {
            EmployeeDataGridView.Columns.Clear();

            var colID = new DataGridViewTextBoxColumn { Name = "EmpID", HeaderText = "ID", Width = 120 };
            var colName = new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill };
            var colPos = new DataGridViewTextBoxColumn { Name = "Position", HeaderText = "Position", Width = 100 };

            foreach (var col in new DataGridViewTextBoxColumn[] { colID, colName, colPos })
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            EmployeeDataGridView.Columns.Add(colID);
            EmployeeDataGridView.Columns.Add(colName);
            EmployeeDataGridView.Columns.Add(colPos);

            ApplyGridStyle(EmployeeDataGridView);
            EmployeeDataGridView.CellPainting += EmployeeCellPainting;
            EmployeeDataGridView.ScrollBars = ScrollBars.None;
            // Rows are loaded asynchronously via LoadEmployeesFromSupabase()
        }

        // ─── LOAD EMPLOYEES FROM SUPABASE ────────────────────────────────────
        private async Task LoadEmployeesFromSupabase()
        {
            try
            {
                var employees = await SupabaseHelper.Instance.FetchAllEmployeesAsync();

                allEmployeeRows.Clear();
                EmployeeDataGridView.Rows.Clear();

                foreach (var emp in employees)
                {
                    var row = new object[] { emp.EmployeeId, emp.FullName, emp.Position };
                    allEmployeeRows.Add(row);
                    EmployeeDataGridView.Rows.Add(row);
                }

                // Update stat card
                if (label6 != null)
                    label6.Text = employees.Count.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load employees: {ex.Message}");
            }
        }

        // ─── MERGED ATTENDANCE + SHIFT GRID ─────────────────────────────────
        private void StyleAttendanceGrid()
        {
            AttendanceLogsDataGridView.Columns.Clear();

            var colID = new DataGridViewTextBoxColumn { Name = "EmpID", HeaderText = "ID", Width = 110 };
            var colName = new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill };
            var colShift = new DataGridViewTextBoxColumn { Name = "Shift", HeaderText = "Shift", Width = 85 };
            var colIn = new DataGridViewTextBoxColumn { Name = "TimeIn", HeaderText = "Time In", Width = 78 };
            var colOut = new DataGridViewTextBoxColumn { Name = "TimeOut", HeaderText = "Time Out", Width = 78 };
            var colHours = new DataGridViewTextBoxColumn { Name = "Hours", HeaderText = "Hours", Width = 70 };
            var colStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", Width = 90 };

            foreach (var col in new DataGridViewTextBoxColumn[] { colID, colName, colShift, colIn, colOut, colHours, colStatus })
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            AttendanceLogsDataGridView.Columns.Add(colID);
            AttendanceLogsDataGridView.Columns.Add(colName);
            AttendanceLogsDataGridView.Columns.Add(colShift);
            AttendanceLogsDataGridView.Columns.Add(colIn);
            AttendanceLogsDataGridView.Columns.Add(colOut);
            AttendanceLogsDataGridView.Columns.Add(colHours);
            AttendanceLogsDataGridView.Columns.Add(colStatus);

            ApplyGridStyle(AttendanceLogsDataGridView);
            AttendanceLogsDataGridView.CellPainting += AttendanceCellPainting;
            AttendanceLogsDataGridView.ScrollBars = ScrollBars.None;

        }

        // ─── LOAD TODAY'S SHIFT ──────────────────────────────────────────────
        private async Task LoadTodaysShiftAsync()
        {
            try
            {
                string todayDay = DateTime.Now.ToString("ddd");
                string today = DateTime.Now.ToString("yyyy-MM-dd");

                var employees = await SupabaseHelper.Instance.FetchAllEmployeesAsync();
                var logs = await SupabaseHelper.Instance.FetchAllAttendanceLogsAsync(today);

                AttendanceLogsDataGridView.Rows.Clear();

                foreach (var emp in employees)
                {
                    if (!IsWorkingToday(emp.WorkDays, todayDay)) continue;

                    var log = logs.FirstOrDefault(l => l.UserId == emp.Id);

                    string timeIn    = string.IsNullOrEmpty(log?.TimeIn)     ? "—" : log.TimeIn;
                    string timeOut   = string.IsNullOrEmpty(log?.TimeOut)    ? "—" : log.TimeOut;
                    string hours     = string.IsNullOrEmpty(log?.TotalHours) ? "—" : log.TotalHours;
                    string status    = log == null ? DetermineStatus(emp) : log.Status;

                    AttendanceLogsDataGridView.Rows.Add(
                        emp.EmployeeId,
                        emp.FullName,
                        emp.ShiftType,
                        timeIn,
                        timeOut,
                        hours,
                        status
                    );
                }

                int presentCount = 0;
                int absentCount = 0;
                int lateCount = 0;

                foreach (DataGridViewRow row in AttendanceLogsDataGridView.Rows)
                {
                    string status = row.Cells["Status"].Value?.ToString() ?? "";
                    if (status == "Present")
                        presentCount++;
                    else if (status == "Late")
                        lateCount++;
                    else if (status == "Absent")
                        absentCount++;
                }

                label4.Text = presentCount.ToString();
                label8.Text = absentCount.ToString();
                label10.Text = lateCount.ToString();

                var reports = await SupabaseHelper.Instance.FetchAllReportsAsync();
                int reportsToday = reports.Count(r =>
                    DateTimeOffset.TryParse(r.CreatedAt, out var d) &&
                    d.ToLocalTime().Date == DateTime.Today);
                label14.Text = reportsToday.ToString();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ LoadTodaysShift: {ex.Message}");
            }
        }

        private bool IsWorkingToday(string workDays, string todayDay)
        {
            if (string.IsNullOrEmpty(workDays)) return true;

            var days = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            int todayIndex = Array.IndexOf(days, todayDay);
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
                        if (start <= end)
                            return todayIndex >= start && todayIndex <= end;
                        else
                            return todayIndex >= start || todayIndex <= end;
                    }
                }
            }

            return workDays.Contains(todayDay, StringComparison.OrdinalIgnoreCase);
        }

        private string DetermineStatus(UserProfile emp)
        {
            if (!DateTime.TryParse(emp.ShiftTimeIn, out var shiftStart))
                return "Working Soon";

            var now = DateTime.Now.TimeOfDay;
            var shiftStartTime = shiftStart.TimeOfDay;
            var cutoff = shiftStartTime.Add(TimeSpan.FromHours(1));

            if (now < shiftStartTime)
                return "Working Soon";
            else if (now >= shiftStartTime && now < cutoff)
                return "Working Soon";
            else
                return "Absent";
        }

        // ─── EMPLOYEE SEARCH BAR ─────────────────────────────────────────────
        private void AddEmployeeSearchBar()
        {
            int searchW = 200;
            int searchH = 28;
            int x = _empGridOriginalLocation.X + _empGridOriginalSize.Width - searchW;
            int y = _empGridOriginalLocation.Y - searchH - 6;

            Panel searchPanel = new Panel();
            searchPanel.Size = new Size(searchW, searchH);
            searchPanel.Location = new Point(x, y);
            searchPanel.BackColor = Color.FromArgb(50, 50, 50);
            searchPanel.Cursor = Cursors.IBeam;

            searchPanel.HandleCreated += (object? s, EventArgs e) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, searchPanel.Width, searchPanel.Height, 14, 14);
                searchPanel.Region = System.Drawing.Region.FromHrgn(hRgn);
            };

            Label searchIcon = new Label();
            searchIcon.Text = "🔍";
            searchIcon.Font = new Font("Segoe UI", 8f);
            searchIcon.ForeColor = Color.FromArgb(160, 160, 160);
            searchIcon.BackColor = Color.Transparent;
            searchIcon.AutoSize = false;
            searchIcon.Size = new Size(24, searchH);
            searchIcon.Location = new Point(4, 0);
            searchIcon.TextAlign = ContentAlignment.MiddleCenter;

            employeeSearchBox = new TextBox();
            employeeSearchBox.BorderStyle = BorderStyle.None;
            employeeSearchBox.BackColor = Color.FromArgb(50, 50, 50);
            employeeSearchBox.ForeColor = Color.FromArgb(130, 130, 130);
            employeeSearchBox.Font = new Font("Segoe UI", 8.5f);
            employeeSearchBox.Size = new Size(searchW - 32, 18);
            employeeSearchBox.Location = new Point(28, 5);
            employeeSearchBox.Text = "Search by name, ID...";

            employeeSearchBox.GotFocus += (object? s, EventArgs e) =>
            {
                if (employeeSearchBox.Text == "Search by name, ID...")
                { employeeSearchBox.Text = ""; employeeSearchBox.ForeColor = Color.White; }
            };
            employeeSearchBox.LostFocus += (object? s, EventArgs e) =>
            {
                if (string.IsNullOrWhiteSpace(employeeSearchBox.Text))
                { employeeSearchBox.Text = "Search by name, ID..."; employeeSearchBox.ForeColor = Color.FromArgb(130, 130, 130); }
            };
            employeeSearchBox.TextChanged += (object? s, EventArgs e) =>
            {
                string query = employeeSearchBox.Text.Trim().ToLower();
                if (query == "search by name, id...") query = "";
                EmployeeDataGridView.Rows.Clear();
                foreach (var row in allEmployeeRows)
                {
                    bool match = string.IsNullOrEmpty(query) ||
                                 row[0].ToString()!.ToLower().Contains(query) ||
                                 row[1].ToString()!.ToLower().Contains(query) ||
                                 row[2].ToString()!.ToLower().Contains(query) ||
                                 row[3].ToString()!.ToLower().Contains(query) ||
                                 row[4].ToString()!.ToLower().Contains(query);
                    if (match) EmployeeDataGridView.Rows.Add(row);
                }
            };

            searchPanel.Controls.Add(searchIcon);
            searchPanel.Controls.Add(employeeSearchBox);
            _empGridOriginalParent.Controls.Add(searchPanel);
            searchPanel.BringToFront();
        }

        // ─── EMPLOYEE CELL PAINTER ───────────────────────────────────────────
        private void EmployeeCellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (sender is not DataGridView dgv) return;
            if (!dgv.Columns.Contains("Status")) return;
            if (e.ColumnIndex == dgv.Columns["Status"].Index && e.RowIndex >= 0)
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
                TextRenderer.DrawText(e.Graphics, status, new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                    e.CellBounds, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

        // ─── ATTENDANCE CELL PAINTER ─────────────────────────────────────────
        private void AttendanceCellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (sender is not DataGridView dgv) return;
            if (e.RowIndex < 0) return;

            if (dgv.Columns.Contains("Shift") && e.ColumnIndex == dgv.Columns["Shift"].Index)
            {
                string? shift = e.Value?.ToString();
                Color textColor = shift switch
                {
                    "Morning" => Color.FromArgb(255, 210, 80),
                    "Afternoon" => Color.FromArgb(100, 180, 255),
                    "Night" => Color.FromArgb(180, 120, 255),
                    _ => Color.White
                };
                e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                TextRenderer.DrawText(e.Graphics, shift, new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                    e.CellBounds, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }

            if (dgv.Columns.Contains("Status") && e.ColumnIndex == dgv.Columns["Status"].Index)
            {
                string? status = e.Value?.ToString();
                Color textColor = status switch
                {
                    "Present" => Color.FromArgb(80, 200, 80),
                    "Late" => Color.FromArgb(255, 180, 0),
                    "Absent" => Color.FromArgb(220, 60, 60),
                    "On Shift" => Color.FromArgb(100, 180, 255),
                    "Offline" => Color.FromArgb(220, 60, 60),
                    "Off Shift" => Color.FromArgb(120, 120, 120),
                    "Working Soon" => Color.FromArgb(255, 210, 80),
                    _ => Color.White
                };
                e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                TextRenderer.DrawText(e.Graphics, status, new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                    e.CellBounds, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }
        }

        // ─── CUSTOM DARK SCROLLBAR WRAPPER (with rounded corners) ────────────
        private void WrapGridWithDarkScroll(DataGridView dgv)
        {
            dgv.ScrollBars = ScrollBars.None;

            Panel wrapper = new Panel();
            wrapper.Size = dgv.Size;
            wrapper.Location = dgv.Location;
            wrapper.Parent = dgv.Parent;
            wrapper.BackColor = Color.FromArgb(20, 20, 20);

            // ── ROUNDED CORNERS ON WRAPPER ──
            wrapper.HandleCreated += (object? sw, EventArgs ev) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, wrapper.Width, wrapper.Height, 14, 14);
                wrapper.Region = System.Drawing.Region.FromHrgn(hRgn);
            };

            dgv.Parent.Controls.Remove(dgv);
            wrapper.Controls.Add(dgv);
            dgv.Location = new Point(0, 0);
            dgv.Width = wrapper.Width - 10;
            dgv.Height = wrapper.Height;

            Panel scrollTrack = new Panel();
            scrollTrack.Width = 6;
            scrollTrack.BackColor = Color.FromArgb(35, 35, 35);
            scrollTrack.Dock = DockStyle.Right;

            Panel scrollThumb = new Panel();
            scrollThumb.Width = 6;
            scrollThumb.Height = 40;
            scrollThumb.Left = 0;
            scrollThumb.Top = 0;
            scrollThumb.BackColor = Color.FromArgb(80, 80, 80);
            scrollThumb.Cursor = Cursors.Hand;

            scrollThumb.Paint += (object? s, PaintEventArgs e) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, scrollThumb.Width, scrollThumb.Height, 6, 6);
                scrollThumb.Region = System.Drawing.Region.FromHrgn(hRgn);
            };

            scrollTrack.Controls.Add(scrollThumb);
            wrapper.Controls.Add(scrollTrack);
            wrapper.BringToFront();

            void UpdateThumb()
            {
                if (dgv.RowCount == 0) return;
                int visibleRows = dgv.DisplayedRowCount(false);
                int totalRows = dgv.RowCount;
                int trackHeight = scrollTrack.Height;
                if (totalRows <= visibleRows) { scrollThumb.Visible = false; return; }
                scrollThumb.Visible = true;
                int thumbHeight = Math.Max(20, trackHeight * visibleRows / totalRows);
                scrollThumb.Height = thumbHeight;
                int firstRow = dgv.FirstDisplayedScrollingRowIndex;
                int scrollRange = totalRows - visibleRows;
                int thumbRange = trackHeight - thumbHeight;
                scrollThumb.Top = (scrollRange > 0) ? (firstRow * thumbRange / scrollRange) : 0;
            }

            bool dragging = false;
            int dragStartY = 0;
            int dragStartTop = 0;

            scrollThumb.MouseDown += (object? s, MouseEventArgs e) =>
            {
                if (e.Button == MouseButtons.Left)
                { dragging = true; dragStartY = scrollThumb.PointToScreen(e.Location).Y; dragStartTop = scrollThumb.Top; }
            };
            scrollThumb.MouseMove += (object? s, MouseEventArgs e) =>
            {
                if (!dragging) return;
                int currentY = scrollThumb.PointToScreen(e.Location).Y;
                int delta = currentY - dragStartY;
                int newTop = Math.Max(0, Math.Min(scrollTrack.Height - scrollThumb.Height, dragStartTop + delta));
                scrollThumb.Top = newTop;
                int totalRows = dgv.RowCount;
                int visibleRows = dgv.DisplayedRowCount(false);
                int scrollRange = totalRows - visibleRows;
                int thumbRange = scrollTrack.Height - scrollThumb.Height;
                if (thumbRange > 0 && scrollRange > 0)
                {
                    int newFirst = Math.Max(0, Math.Min(dgv.RowCount - 1, newTop * scrollRange / thumbRange));
                    dgv.FirstDisplayedScrollingRowIndex = newFirst;
                }
            };
            scrollThumb.MouseUp += (object? s, MouseEventArgs e) => { dragging = false; };

            dgv.MouseWheel += (object? s, MouseEventArgs e) =>
            {
                if (dgv.RowCount == 0) return;
                int current = dgv.FirstDisplayedScrollingRowIndex;
                int delta = e.Delta > 0 ? -3 : 3;
                int newFirst = Math.Max(0, Math.Min(dgv.RowCount - 1, current + delta));
                dgv.FirstDisplayedScrollingRowIndex = newFirst;
                UpdateThumb();
            };

            dgv.Scroll += (object? s, ScrollEventArgs e) => UpdateThumb();
            wrapper.Layout += (object? s, LayoutEventArgs e) => UpdateThumb();
            wrapper.Resize += (object? s, EventArgs e) => UpdateThumb();

            scrollThumb.MouseEnter += (object? s, EventArgs e) => scrollThumb.BackColor = Color.FromArgb(110, 110, 110);
            scrollThumb.MouseLeave += (object? s, EventArgs e) => { if (!dragging) scrollThumb.BackColor = Color.FromArgb(80, 80, 80); };
        }

        // ─── SHARED GRID STYLE ───────────────────────────────────────────────
        private void ApplyGridStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.FromArgb(40, 40, 40);
            dgv.GridColor = Color.FromArgb(60, 60, 60);
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 20, 20);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(200, 200, 200);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(20, 20, 20);
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.FromArgb(200, 200, 200);
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeight = 35;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.DefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 55);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.RowTemplate.Height = 35;

            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dgv.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Color.White;
        }

        // ─── ACTIVE BUTTON ───────────────────────────────────────────────────
        private void SetActiveButton(Button activeBtn)
        {
            _activeBtn = activeBtn;
            Button[] allButtons = { AdminDashboardBtn, EmployeBtn, ReportBtn, AttendanceLogsBtn };
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
            this.Region = System.Drawing.Region.FromHrgn(
                CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            ApplyRoundedCorners();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            clockTimer?.Stop();
            clockTimer?.Dispose();
            _midnightTimer?.Stop();
            _midnightTimer?.Dispose();
        }

        // ─── STUBS ───────────────────────────────────────────────────────────
        private void AdminDashboard_Load(object sender, EventArgs e) { }

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
          // ─── PROFILE PANEL ────────────────────────────────────────────────────
        private void StyleProfilePanel()
        {
            ProfilePanel.Controls.Clear();
            ProfilePanel.BackColor = Color.FromArgb(30, 30, 30);

            // ── Get user data from Supabase ──
            var profile = SupabaseHelper.Instance.CurrentUserProfile;

            // Get initials from full name
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

            // ── Contact Number label ──
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

            // ── Divider ──
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

        // ─── PANEL PAINTS ────────────────────────────────────────────────────
        private void TimeInPanel_Paint(object sender, PaintEventArgs e)
        {
            MakePanelRounded(TotalEmployeePanel, 20);
            TotalEmployeePanel.BackColor = Color.FromArgb(40, 40, 40);
        }

        private void PresentTodayPanel_Paint(object sender, PaintEventArgs e)
        {
            MakePanelRounded(PresentTodayPanel, 20);
            PresentTodayPanel.BackColor = Color.FromArgb(40, 40, 40);
        }

        private void AbsentTodayPanel_Paint(object sender, PaintEventArgs e)
        {
            MakePanelRounded(AbsentTodayPanel, 20);
            AbsentTodayPanel.BackColor = Color.FromArgb(40, 40, 40);
        }

        private void LateTodayPanel_Paint(object sender, PaintEventArgs e)
        {
            MakePanelRounded(LateTodayPanel, 20);
            LateTodayPanel.BackColor = Color.FromArgb(40, 40, 40);
        }

        private void ReportPanel_Paint(object sender, PaintEventArgs e)
        {
            MakePanelRounded(ReportPanel, 20);
            ReportPanel.BackColor = Color.FromArgb(40, 40, 40);
        }

        // ─── NAVIGATION ──────────────────────────────────────────────────────
        private void LogOutBtn_Click(object sender, EventArgs e)
        {
            new HomeForm().Show();
            this.Hide();
        }

        private void EmployeBtn_Click(object sender, EventArgs e)
        {
            new EmployeeSchedule().Show();
            this.Hide();
        }

        private void AttendanceBtn_Click(object sender, EventArgs e)
        {
            new AttendanceLogs().Show();
            this.Hide();
        }

        private void ReportBtn_Click(object sender, EventArgs e)
        {
            new AdminReports().Show();
            this.Hide();
        }

        private void MyDashboardBtn_Click(object sender, EventArgs e) { }

        private void AttendanceLogsBtn_Click(object sender, EventArgs e)
        {
            new AttendanceLogs().Show();
            this.Hide();
        }

        private void AdjustLbl_Click(object sender, EventArgs e) { }
    }
}