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
    public partial class ShiftForm : Form
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

        private TextBox shiftSearchBox;
        private List<object[]> allShiftRows = new List<object[]>();

        private Control _shiftGridOriginalParent;
        private Point _shiftGridOriginalLocation;
        private Size _shiftGridOriginalSize;
        private Button _activeBtn;

        public ShiftForm()
        {
            InitializeComponent();
            SetActiveButton(AttendanceBtn);
            ApplyRoundedCorners();
            StartClock();
            StyleDateTimePicker();
            StyleShiftGrid();

            _shiftGridOriginalParent = ShiftDataGridView.Parent;
            _shiftGridOriginalLocation = ShiftDataGridView.Location;
            _shiftGridOriginalSize = ShiftDataGridView.Size;

            WrapGridWithDarkScroll(ShiftDataGridView);
            AddShiftSearchBar();
            AddActionButtons();
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
                FilterByDate(DateTimePicker.Value);
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

        // ─── SHIFT GRID ──────────────────────────────────────────────────────
        private void StyleShiftGrid()
        {
            ShiftDataGridView.Columns.Clear();

            var colID = new DataGridViewTextBoxColumn { Name = "EmpID", HeaderText = "ID", Width = 50 };
            var colName = new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Name", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill };
            var colDept = new DataGridViewTextBoxColumn { Name = "Department", HeaderText = "Department", Width = 110 };
            var colShift = new DataGridViewTextBoxColumn { Name = "Shift", HeaderText = "Shift", Width = 90 };
            var colDay = new DataGridViewTextBoxColumn { Name = "WorkDays", HeaderText = "Work Days", Width = 130 };
            var colIn = new DataGridViewTextBoxColumn { Name = "TimeIn", HeaderText = "Time In", Width = 80 };
            var colOut = new DataGridViewTextBoxColumn { Name = "TimeOut", HeaderText = "Time Out", Width = 80 };
            var colStatus = new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "Status", Width = 90 };

            foreach (var col in new[] { colID, colName, colDept, colShift, colDay, colIn, colOut, colStatus })
                col.SortMode = DataGridViewColumnSortMode.NotSortable;

            ShiftDataGridView.Columns.Add(colID);
            ShiftDataGridView.Columns.Add(colName);
            ShiftDataGridView.Columns.Add(colDept);
            ShiftDataGridView.Columns.Add(colShift);
            ShiftDataGridView.Columns.Add(colDay);
            ShiftDataGridView.Columns.Add(colIn);
            ShiftDataGridView.Columns.Add(colOut);
            ShiftDataGridView.Columns.Add(colStatus);

            ShiftDataGridView.ReadOnly = false;
            ShiftDataGridView.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;

            ApplyGridStyle(ShiftDataGridView);
            ShiftDataGridView.ScrollBars = ScrollBars.None;
            ShiftDataGridView.CellPainting += ShiftDataGridView_CellPainting;

            allShiftRows.Add(new object[] { "001", "Kevin Kikuchi", "Engineering", "Morning", "Mon-Fri", "08:00 AM", "05:00 PM", GetShiftStatus("Morning") });
            allShiftRows.Add(new object[] { "002", "Remixon Ipanag", "HR", "Morning", "Mon-Fri", "08:00 AM", "05:00 PM", GetShiftStatus("Morning") });
            allShiftRows.Add(new object[] { "003", "Rojamin Merari Pantrollia", "Finance", "Afternoon", "Mon-Sat", "02:00 PM", "11:00 PM", GetShiftStatus("Afternoon") });
            allShiftRows.Add(new object[] { "004", "Wara Gud", "Engineering", "Night", "Mon-Fri", "10:00 PM", "06:00 AM", GetShiftStatus("Night") });
            allShiftRows.Add(new object[] { "005", "Ana Reyes", "IT", "Morning", "Mon-Fri", "08:00 AM", "05:00 PM", GetShiftStatus("Morning") });
            allShiftRows.Add(new object[] { "006", "Marco Dela Cruz", "Security", "Night", "Mon-Sun", "10:00 PM", "06:00 AM", GetShiftStatus("Night") });
            allShiftRows.Add(new object[] { "007", "Liza Santos", "Finance", "Afternoon", "Mon-Fri", "02:00 PM", "11:00 PM", GetShiftStatus("Afternoon") });
            allShiftRows.Add(new object[] { "008", "Ben Alonzo", "HR", "Morning", "Mon-Sat", "08:00 AM", "05:00 PM", GetShiftStatus("Morning") });
            allShiftRows.Add(new object[] { "009", "Claire Navarro", "Engineering", "Afternoon", "Tue-Sat", "02:00 PM", "11:00 PM", GetShiftStatus("Afternoon") });
            allShiftRows.Add(new object[] { "010", "Dante Villanueva", "Security", "Night", "Mon-Fri", "10:00 PM", "06:00 AM", GetShiftStatus("Night") });

            foreach (var row in allShiftRows)
                ShiftDataGridView.Rows.Add(row);
        }

        private string GetShiftStatus(string shift)
        {
            int hour = DateTime.Now.Hour;
            return shift switch
            {
                "Morning" => (hour >= 8 && hour < 17) ? "On Shift" : "Off Shift",
                "Afternoon" => (hour >= 14 && hour < 23) ? "On Shift" : "Off Shift",
                "Night" => (hour >= 22 || hour < 6) ? "On Shift" : "Off Shift",
                _ => "Off Shift"
            };
        }

        private void ShiftDataGridView_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex == ShiftDataGridView.Columns["Status"].Index && e.RowIndex >= 0)
            {
                string? status = e.Value?.ToString();
                Color textColor = status switch
                {
                    "On Shift" => Color.FromArgb(80, 200, 80),
                    "Off Shift" => Color.FromArgb(160, 160, 160),
                    _ => Color.White
                };
                e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);
                TextRenderer.DrawText(e.Graphics, status, new Font("Segoe UI Semibold", 9f, FontStyle.Bold),
                    e.CellBounds, textColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                e.Handled = true;
            }

            if (e.ColumnIndex == ShiftDataGridView.Columns["Shift"].Index && e.RowIndex >= 0)
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
        }

        private void FilterByDate(DateTime date)
        {
            ShiftDataGridView.Rows.Clear();
            foreach (var row in allShiftRows)
            {
                row[7] = GetShiftStatus(row[3].ToString()!);
                ShiftDataGridView.Rows.Add(row);
            }
        }

        // ─── ACTION BUTTONS ──────────────────────────────────────────────────
        private void AddActionButtons()
        {
            int btnY = _shiftGridOriginalLocation.Y - 36;
            int btnX = _shiftGridOriginalLocation.X;

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
                object[] blank = new object[] { "", "", "", "", "", "", "", "" };
                allShiftRows.Add(blank);
                ShiftDataGridView.Rows.Add(blank);
                int lastIndex = ShiftDataGridView.Rows.Count - 1;
                ShiftDataGridView.FirstDisplayedScrollingRowIndex = lastIndex;
                ShiftDataGridView.CurrentCell = ShiftDataGridView.Rows[lastIndex].Cells[0];
                ShiftDataGridView.BeginEdit(true);
            };

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
            btnDelete.Click += (object? s, EventArgs e) =>
            {
                if (ShiftDataGridView.CurrentRow == null) return;
                int idx = ShiftDataGridView.CurrentRow.Index;
                if (idx < 0 || idx >= ShiftDataGridView.Rows.Count) return;
                var confirm = MessageBox.Show("Delete this shift entry?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    if (idx < allShiftRows.Count) allShiftRows.RemoveAt(idx);
                    ShiftDataGridView.Rows.RemoveAt(idx);
                }
            };

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
            btnSave.Click += (object? s, EventArgs e) =>
            {
                ShiftDataGridView.EndEdit();
                allShiftRows.Clear();
                foreach (DataGridViewRow row in ShiftDataGridView.Rows)
                {
                    if (row.IsNewRow) continue;
                    allShiftRows.Add(new object[]
                    {
                        row.Cells["EmpID"].Value      ?? "",
                        row.Cells["Name"].Value       ?? "",
                        row.Cells["Department"].Value ?? "",
                        row.Cells["Shift"].Value      ?? "",
                        row.Cells["WorkDays"].Value   ?? "",
                        row.Cells["TimeIn"].Value     ?? "",
                        row.Cells["TimeOut"].Value    ?? "",
                        row.Cells["Status"].Value     ?? ""
                    });
                }
                MessageBox.Show("Shifts saved successfully!", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            _shiftGridOriginalParent.Controls.Add(btnAdd);
            _shiftGridOriginalParent.Controls.Add(btnDelete);
            _shiftGridOriginalParent.Controls.Add(btnSave);
            btnAdd.BringToFront();
            btnDelete.BringToFront();
            btnSave.BringToFront();
        }

        // ─── SEARCH BAR ──────────────────────────────────────────────────────
        private void AddShiftSearchBar()
        {
            int searchW = 200;
            int searchH = 28;
            int x = _shiftGridOriginalLocation.X + _shiftGridOriginalSize.Width - searchW;
            int y = _shiftGridOriginalLocation.Y - searchH - 6;

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

            shiftSearchBox = new TextBox();
            shiftSearchBox.BorderStyle = BorderStyle.None;
            shiftSearchBox.BackColor = Color.FromArgb(50, 50, 50);
            shiftSearchBox.ForeColor = Color.FromArgb(130, 130, 130);
            shiftSearchBox.Font = new Font("Segoe UI", 8.5f);
            shiftSearchBox.Size = new Size(searchW - 32, 18);
            shiftSearchBox.Location = new Point(28, 5);
            shiftSearchBox.Text = "Search name, shift...";

            shiftSearchBox.GotFocus += (object? s, EventArgs e) =>
            {
                if (shiftSearchBox.Text == "Search name, shift...")
                { shiftSearchBox.Text = ""; shiftSearchBox.ForeColor = Color.White; }
            };
            shiftSearchBox.LostFocus += (object? s, EventArgs e) =>
            {
                if (string.IsNullOrWhiteSpace(shiftSearchBox.Text))
                { shiftSearchBox.Text = "Search name, shift..."; shiftSearchBox.ForeColor = Color.FromArgb(130, 130, 130); }
            };
            shiftSearchBox.TextChanged += (object? s, EventArgs e) =>
            {
                string query = shiftSearchBox.Text.Trim().ToLower();
                if (query == "search name, shift...") query = "";
                ShiftDataGridView.Rows.Clear();
                foreach (var row in allShiftRows)
                {
                    bool match = string.IsNullOrEmpty(query) ||
                                 row[0].ToString()!.ToLower().Contains(query) ||
                                 row[1].ToString()!.ToLower().Contains(query) ||
                                 row[2].ToString()!.ToLower().Contains(query) ||
                                 row[3].ToString()!.ToLower().Contains(query) ||
                                 row[7].ToString()!.ToLower().Contains(query);
                    if (match) ShiftDataGridView.Rows.Add(row);
                }
            };

            searchPanel.Controls.Add(searchIcon);
            searchPanel.Controls.Add(shiftSearchBox);
            _shiftGridOriginalParent.Controls.Add(searchPanel);
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
                int delta = e.Delta > 0 ? -3 : 3;
                int newFirst = Math.Max(0, Math.Min(dgv.RowCount - 1, dgv.FirstDisplayedScrollingRowIndex + delta));
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
            Button[] allButtons = { AttendanceLogsBtn, AdminDashboardBtn, EmployeBtn, AttendanceBtn, ReportBtn };
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
            if (clockTimer != null) { clockTimer.Stop(); clockTimer.Dispose(); }
        }

        // ─── STUBS ───────────────────────────────────────────────────────────
        private void ShiftForm_Load(object sender, EventArgs e) { }
        private void AdjustLbl_Click(object sender, EventArgs e) { }

        // ─── SIDEBAR ─────────────────────────────────────────────────────────
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            MakeButtonRounded(AttendanceLogsBtn, 20);
            MakeButtonRounded(LogOutBtn, 20);
            MakeButtonRounded(AdminDashboardBtn, 20);
            MakeButtonRounded(EmployeBtn, 20);
            MakeButtonRounded(AttendanceBtn, 20);
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

        private void EmployeBtn_Click(object sender, EventArgs e)
        {
            EmployeeSchedule schedule = new EmployeeSchedule();
            schedule.Show();
            this.Hide();
        }

        private void AttendanceBtn_Click(object sender, EventArgs e)
        {
            AttendanceLogs logs = new AttendanceLogs();
            logs.Show();
            this.Hide();
        }
        private void ReportBtn_Click(object sender, EventArgs e) { }

        private void LogOutBtn_Click(object sender, EventArgs e)
        {
            HomeForm home = new HomeForm();
            home.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}