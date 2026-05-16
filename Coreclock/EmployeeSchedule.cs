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
    public partial class EmployeeSchedule : Form
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


        // ─── SEARCH + DATA ───────────────────────────────────────────────────
        private TextBox scheduleSearchBox;
        private List<object[]> allScheduleRows = new List<object[]>();
        private Dictionary<string, string> employeeIdToUuid = new Dictionary<string, string>();

        private Control _schedGridOriginalParent;
        private Point _schedGridOriginalLocation;
        private Size _schedGridOriginalSize;
        private Button _activeBtn;
        private Image _profilePhoto = null;


        public EmployeeSchedule()
        {
            InitializeComponent();
            SetActiveButton(EmployeBtn);
            ApplyRoundedCorners();
            StartClock();
            StyleDateTimePicker();
            StyleScheduleGrid();
            StyleProfilePanel(); 



            _schedGridOriginalParent = ScheduleDataGridView.Parent;
            _schedGridOriginalLocation = ScheduleDataGridView.Location;
            _schedGridOriginalSize = ScheduleDataGridView.Size;

            WrapGridWithDarkScroll(ScheduleDataGridView);
            AddScheduleSearchBar();
            AddActionButtons();

            // Load real employees from Supabase after form is shown
            this.Load += async (s, e) => await LoadScheduleFromSupabase();
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

        // ─── SCHEDULE GRID ───────────────────────────────────────────────────
        private void StyleScheduleGrid()
        {
            ScheduleDataGridView.Columns.Clear();

            var colID = new DataGridViewTextBoxColumn { Name = "EmpID", HeaderText = "ID", Width = 120 };
            var colName = new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill };
            var colPost = new DataGridViewTextBoxColumn { Name = "Position", HeaderText = "Position", Width = 100 };
            var colShift = new DataGridViewTextBoxColumn { Name = "Shift", HeaderText = "Shift", Width = 80 };
            var colDay = new DataGridViewTextBoxColumn { Name = "WorkDays", HeaderText = "Work Days", Width = 130 };
            var colIn = new DataGridViewTextBoxColumn { Name = "TimeIn", HeaderText = "Time In", Width = 75 };
            var colOut = new DataGridViewTextBoxColumn { Name = "TimeOut", HeaderText = "Time Out", Width = 75 };

            colID.SortMode = DataGridViewColumnSortMode.NotSortable;
            colName.SortMode = DataGridViewColumnSortMode.NotSortable;
            colPost.SortMode = DataGridViewColumnSortMode.NotSortable;
            colShift.SortMode = DataGridViewColumnSortMode.NotSortable;
            colDay.SortMode = DataGridViewColumnSortMode.NotSortable;
            colIn.SortMode = DataGridViewColumnSortMode.NotSortable;
            colOut.SortMode = DataGridViewColumnSortMode.NotSortable;

            ScheduleDataGridView.Columns.Add(colID);
            ScheduleDataGridView.Columns.Add(colName);
            ScheduleDataGridView.Columns.Add(colPost);
            ScheduleDataGridView.Columns.Add(colShift);
            ScheduleDataGridView.Columns.Add(colDay);
            ScheduleDataGridView.Columns.Add(colIn);
            ScheduleDataGridView.Columns.Add(colOut);

            // Allow editing
            ScheduleDataGridView.ReadOnly = false;
            ScheduleDataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            ApplyGridStyle(ScheduleDataGridView);
            ScheduleDataGridView.ScrollBars = ScrollBars.None;
            // Rows are loaded asynchronously via LoadScheduleFromSupabase()
        }

        // ─── LOAD SCHEDULE FROM SUPABASE ──────────────────────────────────────
        private async Task LoadScheduleFromSupabase()
        {
            try
            {
                var employees = await SupabaseHelper.Instance.FetchAllEmployeesAsync();

                allScheduleRows.Clear();
                ScheduleDataGridView.Rows.Clear();
                employeeIdToUuid.Clear();

                foreach (var emp in employees)
                {
                    var row = new object[] { emp.EmployeeId, emp.FullName, emp.Position, emp.ShiftType, emp.WorkDays, emp.ShiftTimeIn, emp.ShiftTimeOut };
                    allScheduleRows.Add(row);
                    ScheduleDataGridView.Rows.Add(row);

                    // Store mapping: employeeId → userId (UUID)
                    if (!string.IsNullOrEmpty(emp.EmployeeId) && !string.IsNullOrEmpty(emp.Id))
                        employeeIdToUuid[emp.EmployeeId] = emp.Id;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load schedule: {ex.Message}");
            }
        }

        // ─── ACTION BUTTONS (Add / Delete / Save) ────────────────────────────
        private void AddActionButtons()
        {
            int btnY = _schedGridOriginalLocation.Y - 36;
            int btnX = _schedGridOriginalLocation.X;

            // ── ADD ROW button ──
            Button btnAdd = new Button();
            btnAdd.Text = "+ Add";
            btnAdd.Size = new Size(70, 28);
            btnAdd.Location = new Point(btnX, btnY);
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.BackColor = Color.FromArgb(60, 140, 60);
            btnAdd.ForeColor = Color.White;
            btnAdd.Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold);
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.HandleCreated += (object? s, EventArgs e) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, btnAdd.Width, btnAdd.Height, 12, 12);
                btnAdd.Region = System.Drawing.Region.FromHrgn(hRgn);
            };
            btnAdd.Click += (object? s, EventArgs e) =>
            {
                // Add a blank editable row
                object[] blank = new object[] { "", "", "", "", "", "", "" };
                allScheduleRows.Add(blank);
                ScheduleDataGridView.Rows.Add(blank);

                // Scroll to and select the new row
                int lastIndex = ScheduleDataGridView.Rows.Count - 1;
                ScheduleDataGridView.FirstDisplayedScrollingRowIndex = lastIndex;
                ScheduleDataGridView.CurrentCell = ScheduleDataGridView.Rows[lastIndex].Cells[0];
                ScheduleDataGridView.BeginEdit(true);
            };

            // ── DELETE ROW button ──
            Button btnDelete = new Button();
            btnDelete.Text = "✕ Delete";
            btnDelete.Size = new Size(75, 28);
            btnDelete.Location = new Point(btnX + 78, btnY);
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.BackColor = Color.FromArgb(180, 50, 50);
            btnDelete.ForeColor = Color.White;
            btnDelete.Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.HandleCreated += (object? s, EventArgs e) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, btnDelete.Width, btnDelete.Height, 12, 12);
                btnDelete.Region = System.Drawing.Region.FromHrgn(hRgn);
            };
            btnDelete.Click += async (object? s, EventArgs e) =>
            {
                if (ScheduleDataGridView.CurrentRow == null) return;
                int idx = ScheduleDataGridView.CurrentRow.Index;
                if (idx < 0 || idx >= ScheduleDataGridView.Rows.Count) return;

                // Get Employee ID from the selected row to find the UUID
                var empId = ScheduleDataGridView.Rows[idx].Cells["EmpID"].Value?.ToString() ?? "";

                var confirm = MessageBox.Show(
                    $"Are you sure you want to permanently delete employee {empId}?",
                    "Confirm Delete",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (confirm == DialogResult.Yes)
                {
                    bool canRemoveFromGrid = true;

                    // If the employee exists in Supabase, delete them there first
                    if (!string.IsNullOrEmpty(empId) && employeeIdToUuid.ContainsKey(empId))
                    {
                        string userId = employeeIdToUuid[empId];
                        var result = await SupabaseHelper.Instance.DeleteEmployeeAsync(userId);
                        
                        if (!result.success)
                        {
                            MessageBox.Show($"Failed to delete from database:\n{result.error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            canRemoveFromGrid = false;
                        }
                    }

                    if (canRemoveFromGrid)
                    {
                        // Remove from master list and grid
                        if (idx < allScheduleRows.Count)
                            allScheduleRows.RemoveAt(idx);
                        
                        ScheduleDataGridView.Rows.RemoveAt(idx);

                        // Clean up the mapping
                        if (!string.IsNullOrEmpty(empId))
                            employeeIdToUuid.Remove(empId);
                    }
                }
            };

            // ── SAVE button ──
            Button btnSave = new Button();
            btnSave.Text = "💾 Save";
            btnSave.Size = new Size(70, 28);
            btnSave.Location = new Point(btnX + 161, btnY);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.BackColor = Color.FromArgb(40, 100, 180);
            btnSave.ForeColor = Color.White;
            btnSave.Font = new Font("Segoe UI Semibold", 8.5f, FontStyle.Bold);
            btnSave.Cursor = Cursors.Hand;
            btnSave.HandleCreated += (object? s, EventArgs e) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, btnSave.Width, btnSave.Height, 12, 12);
                btnSave.Region = System.Drawing.Region.FromHrgn(hRgn);
            };
            btnSave.Click += async (object? s, EventArgs e) =>
            {
                // Commit any active edit
                ScheduleDataGridView.EndEdit();

                // Sync grid back to master list and save to Supabase
                allScheduleRows.Clear();
                int savedCount = 0;
                string lastError = "";

                foreach (DataGridViewRow row in ScheduleDataGridView.Rows)
                {
                    if (row.IsNewRow) continue;
                    var empId = row.Cells["EmpID"].Value?.ToString() ?? "";
                    var name = row.Cells["Name"].Value?.ToString() ?? "";
                    var position = row.Cells["Position"].Value?.ToString() ?? "";
                    var shift = row.Cells["Shift"].Value?.ToString() ?? "";
                    var workDays = row.Cells["WorkDays"].Value?.ToString() ?? "";
                    var timeIn = row.Cells["TimeIn"].Value?.ToString() ?? "";
                    var timeOut = row.Cells["TimeOut"].Value?.ToString() ?? "";

                    allScheduleRows.Add(new object[] { empId, name, position, shift, workDays, timeIn, timeOut });

                    // Save to Supabase using UUID (id) instead of employee_id
                    if (!string.IsNullOrEmpty(empId) && employeeIdToUuid.ContainsKey(empId))
                    {
                        string userId = employeeIdToUuid[empId];
                        var result = await SupabaseHelper.Instance.SaveScheduleAsync(userId, workDays, shift, timeIn, timeOut);
                        if (result.success)
                        {
                            savedCount++;
                            await SupabaseHelper.Instance.DeleteTodayAbsentAsync(userId);
                        }
                        else lastError = result.error;
                    }
                }

                if (string.IsNullOrEmpty(lastError))
                    MessageBox.Show($"Schedule saved successfully! ({savedCount} employees updated)", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show($"Save failed:\n{lastError}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            _schedGridOriginalParent.Controls.Add(btnAdd);
            _schedGridOriginalParent.Controls.Add(btnDelete);
            _schedGridOriginalParent.Controls.Add(btnSave);
            btnAdd.BringToFront();
            btnDelete.BringToFront();
            btnSave.BringToFront();
        }

        // ─── SEARCH BAR ──────────────────────────────────────────────────────
        private void AddScheduleSearchBar()
        {
            int searchW = 200;
            int searchH = 28;
            int x = _schedGridOriginalLocation.X + _schedGridOriginalSize.Width - searchW;
            int y = _schedGridOriginalLocation.Y - searchH - 6;

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

            scheduleSearchBox = new TextBox();
            scheduleSearchBox.BorderStyle = BorderStyle.None;
            scheduleSearchBox.BackColor = Color.FromArgb(50, 50, 50);
            scheduleSearchBox.ForeColor = Color.FromArgb(130, 130, 130);
            scheduleSearchBox.Font = new Font("Segoe UI", 8.5f);
            scheduleSearchBox.Size = new Size(searchW - 32, 18);
            scheduleSearchBox.Location = new Point(28, 5);
            scheduleSearchBox.Text = "Search by name, ID...";

            scheduleSearchBox.GotFocus += (object? s, EventArgs e) =>
            {
                if (scheduleSearchBox.Text == "Search by name, ID...")
                {
                    scheduleSearchBox.Text = "";
                    scheduleSearchBox.ForeColor = Color.White;
                }
            };

            scheduleSearchBox.LostFocus += (object? s, EventArgs e) =>
            {
                if (string.IsNullOrWhiteSpace(scheduleSearchBox.Text))
                {
                    scheduleSearchBox.Text = "Search by name, ID...";
                    scheduleSearchBox.ForeColor = Color.FromArgb(130, 130, 130);
                }
            };

            scheduleSearchBox.TextChanged += (object? s, EventArgs e) =>
            {
                string query = scheduleSearchBox.Text.Trim().ToLower();
                if (query == "search by name, id...") query = "";

                ScheduleDataGridView.Rows.Clear();
                foreach (var row in allScheduleRows)
                {
                    bool match = string.IsNullOrEmpty(query) ||
                                 row[0].ToString()!.ToLower().Contains(query) ||
                                 row[1].ToString()!.ToLower().Contains(query) ||
                                 row[2].ToString()!.ToLower().Contains(query) ||
                                 row[3].ToString()!.ToLower().Contains(query) ||
                                 row[4].ToString()!.ToLower().Contains(query);
                    if (match)
                        ScheduleDataGridView.Rows.Add(row);
                }
            };

            searchPanel.Controls.Add(searchIcon);
            searchPanel.Controls.Add(scheduleSearchBox);
            _schedGridOriginalParent.Controls.Add(searchPanel);
            searchPanel.BringToFront();
        }

        // ─── CUSTOM DARK SCROLLBAR WRAPPER ───────────────────────────────────
        private void WrapGridWithDarkScroll(DataGridView dgv)
        {
            dgv.ScrollBars = ScrollBars.None;

            Panel wrapper = new Panel();
            wrapper.Size = dgv.Size;
            wrapper.Location = dgv.Location;
            wrapper.Parent = dgv.Parent;
            wrapper.BackColor = Color.FromArgb(20, 20, 20);

            // ── ROUNDED CORNERS on the wrapper ──────────────────────────────
            wrapper.HandleCreated += (object? s, EventArgs e) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, wrapper.Width, wrapper.Height, 20, 20);
                wrapper.Region = System.Drawing.Region.FromHrgn(hRgn);
            };

            wrapper.Resize += (object? s, EventArgs e) =>
            {
                IntPtr hRgn = CreateRoundRectRgn(0, 0, wrapper.Width, wrapper.Height, 20, 20);
                wrapper.Region = System.Drawing.Region.FromHrgn(hRgn);
            };
            // ────────────────────────────────────────────────────────────────

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

                if (totalRows <= visibleRows)
                {
                    scrollThumb.Visible = false;
                    return;
                }

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
                {
                    dragging = true;
                    dragStartY = scrollThumb.PointToScreen(e.Location).Y;
                    dragStartTop = scrollThumb.Top;
                }
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
                    int newFirst = newTop * scrollRange / thumbRange;
                    newFirst = Math.Max(0, Math.Min(dgv.RowCount - 1, newFirst));
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
            scrollThumb.MouseLeave += (object? s, EventArgs e) =>
            {
                if (!dragging) scrollThumb.BackColor = Color.FromArgb(80, 80, 80);
            };
        }

        // ─── SHARED GRID STYLE ───────────────────────────────────────────────
        private void ApplyGridStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.FromArgb(40, 40, 40);
            dgv.GridColor = Color.FromArgb(60, 60, 60);
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = true;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeRows = false;
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

        private void SetActiveButton(Button activeBtn)
        {
            _activeBtn = activeBtn;
            Button[] allButtons = { AttendanceLogsBtn, AdminDashboardBtn, EmployeBtn, ReportBtn };
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

        // ─── STUBS ───────────────────────────────────────────────────────────
        private void EmployeeSchedule_Load(object sender, EventArgs e) { }

        // ─── SIDEBAR ─────────────────────────────────────────────────────────
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
        private void AdminDashboardBtn_Click(object sender, EventArgs e)
        {
            AdminDashboard dashboard = new AdminDashboard();
            dashboard.Show();
            this.Hide();
        }

        private void EmployeBtn_Click(object sender, EventArgs e) { }

        private void AttendanceBtn_Click(object sender, EventArgs e)
        {
            AttendanceLogs logs = new AttendanceLogs();
            logs.Show();
            this.Hide();
        }

        private void ReportBtn_Click(object sender, EventArgs e) {
            AdminReports AdminR = new AdminReports();
            AdminR.Show();
            this.Hide();
        }

        private void LogOutBtn_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }

        private void AttendanceLogsBtn_Click(object sender, EventArgs e)
        {
            AttendanceLogs AttendanceL = new AttendanceLogs();
            AttendanceL.Show();
            this.Hide();
        }

        private void LogOutBtn_Click_1(object sender, EventArgs e)
        {
            HomeForm Home = new HomeForm();
            Home.Show();
            this.Hide();
        }
    }
}