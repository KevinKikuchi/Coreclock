using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Windows.Forms;

namespace Coreclock
{
    public partial class AdminReports : Form
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
        private Button _activeBtn;
        private Image _profilePhoto = null;


        // ─── COLORS ───────────────────────────────────────────────────────────
        private readonly Color Gold = Color.FromArgb(200, 168, 75);
        private readonly Color BgDark = Color.FromArgb(20, 20, 20);
        private readonly Color BgPanel = Color.FromArgb(30, 30, 30);
        private readonly Color BgCard = Color.FromArgb(38, 38, 38);
        private readonly Color BgInput = Color.FromArgb(45, 45, 45);
        private readonly Color Green = Color.FromArgb(80, 200, 120);
        private readonly Color Red = Color.FromArgb(220, 60, 60);
        private readonly Color Yellow = Color.FromArgb(255, 180, 0);
        private readonly Color TextMuted = Color.FromArgb(140, 140, 140);
        private readonly Color BorderCol = Color.FromArgb(55, 55, 55);

        // ─── REPORT DATA ──────────────────────────────────────────────────────
        private class ReportEntry
        {
            public string ID, Name, ReportTitle, Period, Message;
            public bool IsRead;
        }

        private List<ReportEntry> _reports = new List<ReportEntry>
        {
            new ReportEntry { ID="EMP-001", Name="Kevin Kikuchi",  ReportTitle="Leave Request",        Period="May 1, 2026",  Message="Good day, Admin. I would like to request a leave of absence on May 16, 2026 for personal reasons. I have already coordinated with my team to ensure my tasks are covered. Thank you.", IsRead=false },
            new ReportEntry { ID="EMP-002", Name="Rojamin Merari Pantrollia",    ReportTitle="Overtime Report",      Period="May 2, 2026",  Message="Hi Admin, I am submitting my overtime report for this week. I rendered 3 extra hours on Monday and 2 extra hours on Wednesday to meet the project deadline. Kindly approve. Thank you.", IsRead=false },
            new ReportEntry { ID="EMP-003", Name="Remixon Ipanag",    ReportTitle="Incident Report",      Period="May 2, 2026",  Message="Admin, I am reporting an incident that occurred in the office on May 2. The air conditioning unit in the Finance department malfunctioned and caused a minor water leak. Maintenance has been notified.", IsRead=false },
            
        };

        private List<ReportEntry> _filtered;
        private int _selectedIndex = 0;

        // ─── CONTENT CONTROLS ─────────────────────────────────────────────────
        private Panel pnlLeft, pnlRight, pnlDetail;
        private TextBox txtSearch;
        private DataGridView dgvList;
        private Label lblMsgFrom, lblMsgTitle, lblMsgDate, lblMsgBody, lblMsgReadBadge;
        private Label lblNoSel;
        private Button btnDone, btnPrint, btnPdf;

        // ─── CONSTRUCTOR ──────────────────────────────────────────────────────
        public AdminReports()
        {
            InitializeComponent();
            ApplyRoundedCorners();
            StartClock();
            StyleDateTimePicker();
            SetActiveButton(ReportBtn);
            _filtered = new List<ReportEntry>(_reports);
            BuildReportsHub();
            StyleProfilePanel();
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

        // ─── BUILD REPORTS HUB ────────────────────────────────────────────────
        private void BuildReportsHub()
        {
            Control parent = DateTimePicker.Parent;
            int sidebarRight = 230; // sidebar panel1 is ~220px wide

            Label lblTitle = new Label();
            lblTitle.Text = "Reports Hub";
            lblTitle.Font = new Font("Segoe UI", 15f, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(sidebarRight, 78);
            parent.Controls.Add(lblTitle);

            Label lblSub = new Label();
            lblSub.Text = "Employee reports & messages inbox";
            lblSub.Font = new Font("Segoe UI", 9f);
            lblSub.ForeColor = TextMuted;
            lblSub.BackColor = Color.Transparent;
            lblSub.AutoSize = true;
            lblSub.Location = new Point(sidebarRight + 2, 106);
            parent.Controls.Add(lblSub);

            int splitY = 132;
            int availW = parent.Width - sidebarRight - 20;
            int availH = parent.Height - splitY - 20;

            BuildLeftPanel(parent, splitY, availW, availH, sidebarRight);
            BuildRightPanel(parent, splitY, availW, availH, sidebarRight);
            LoadList();
        }

        // ─── LEFT PANEL ───────────────────────────────────────────────────────
        private void BuildLeftPanel(Control parent, int y, int availW, int availH, int sidebarRight)
        {
            int panelW = 340;

            pnlLeft = new Panel();
            pnlLeft.BackColor = BgPanel;
            pnlLeft.Location = new Point(sidebarRight, y);
            pnlLeft.Size = new Size(panelW, availH);
            pnlLeft.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            RoundPanel(pnlLeft, 14);
            parent.Controls.Add(pnlLeft);

            Label lbl = new Label();
            lbl.Text = "Reports Inbox";
            lbl.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            lbl.ForeColor = Gold;
            lbl.BackColor = Color.Transparent;
            lbl.AutoSize = true;
            lbl.Location = new Point(12, 12);
            pnlLeft.Controls.Add(lbl);

            // Search bar
            Panel searchPanel = new Panel();
            searchPanel.Size = new Size(panelW - 28, 28);
            searchPanel.Location = new Point(12, 40);
            searchPanel.BackColor = BgInput;
            searchPanel.Cursor = Cursors.IBeam;
            searchPanel.HandleCreated += (s, e) =>
            {
                IntPtr h = CreateRoundRectRgn(0, 0, searchPanel.Width, searchPanel.Height, 14, 14);
                searchPanel.Region = System.Drawing.Region.FromHrgn(h);
            };

            Label ico = new Label();
            ico.Text = "🔍";
            ico.Font = new Font("Segoe UI", 8f);
            ico.ForeColor = TextMuted;
            ico.BackColor = Color.Transparent;
            ico.AutoSize = false;
            ico.Size = new Size(24, 28);
            ico.Location = new Point(4, 0);
            ico.TextAlign = ContentAlignment.MiddleCenter;

            txtSearch = new TextBox();
            txtSearch.BorderStyle = BorderStyle.None;
            txtSearch.BackColor = BgInput;
            txtSearch.ForeColor = TextMuted;
            txtSearch.Font = new Font("Segoe UI", 8.5f);
            txtSearch.Size = new Size(panelW - 66, 18);
            txtSearch.Location = new Point(28, 5);
            txtSearch.Text = "Search by name, ID, title...";
            txtSearch.GotFocus += (s, e) => { if (txtSearch.Text == "Search by name, ID, title...") { txtSearch.Text = ""; txtSearch.ForeColor = Color.White; } };
            txtSearch.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "Search by name, ID, title..."; txtSearch.ForeColor = TextMuted; } };
            txtSearch.TextChanged += (s, e) => FilterList();

            searchPanel.Controls.Add(ico);
            searchPanel.Controls.Add(txtSearch);
            pnlLeft.Controls.Add(searchPanel);

            // Delete buttons row
            Button btnDelSel = MakeActionButton("🗑  Delete Selected", 12, availH - 44, 135, 30, Color.White, Color.FromArgb(60, 25, 25));
            btnDelSel.Click += (s, e) => DeleteSelected();
            pnlLeft.Controls.Add(btnDelSel);

            Button btnDelAll = MakeActionButton("🗑  Delete All", panelW - 147, availH - 44, 120, 30, Color.White, Color.FromArgb(80, 20, 20));
            btnDelAll.Click += (s, e) => DeleteAll();
            pnlLeft.Controls.Add(btnDelAll);

            // Grid
            dgvList = new DataGridView();
            dgvList.Location = new Point(0, 78);
            dgvList.Size = new Size(panelW - 4, availH - 130);
            dgvList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ApplyGridStyle(dgvList);
            dgvList.Cursor = Cursors.Hand;
            dgvList.MultiSelect = true;

            dgvList.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "ID", Name = "ID", Width = 72, SortMode = DataGridViewColumnSortMode.NotSortable });
            dgvList.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Report Title", Name = "Title", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, SortMode = DataGridViewColumnSortMode.NotSortable });

            dgvList.CellPainting += DgvList_CellPainting;
            dgvList.SelectionChanged += (s, e) =>
            {
                if (dgvList.SelectedRows.Count > 0)
                {
                    _selectedIndex = dgvList.SelectedRows[0].Index;
                    ShowDetail();
                }
            };

            pnlLeft.Controls.Add(dgvList);
        }

        // ─── RIGHT PANEL ──────────────────────────────────────────────────────
        private void BuildRightPanel(Control parent, int y, int availW, int availH, int sidebarRight)
        {
            int leftW = 340;
            int gap = 16;
            int rightX = sidebarRight + leftW + gap;
            int rightW = availW - leftW - gap;

            pnlRight = new Panel();
            pnlRight.BackColor = BgPanel;
            pnlRight.Location = new Point(rightX, y);
            pnlRight.Size = new Size(rightW, availH);
            pnlRight.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            RoundPanel(pnlRight, 14);
            parent.Controls.Add(pnlRight);

            Label lblTitle = new Label();
            lblTitle.Text = "Report Detail";
            lblTitle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblTitle.ForeColor = Gold;
            lblTitle.BackColor = Color.Transparent;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(14, 14);
            pnlRight.Controls.Add(lblTitle);

            Panel divider = new Panel();
            divider.BackColor = BorderCol;
            divider.Location = new Point(0, 42);
            divider.Size = new Size(rightW, 1);
            divider.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlRight.Controls.Add(divider);

            lblNoSel = new Label();
            lblNoSel.Text = "← Select a report from the inbox to read";
            lblNoSel.Font = new Font("Segoe UI", 10f);
            lblNoSel.ForeColor = TextMuted;
            lblNoSel.BackColor = Color.Transparent;
            lblNoSel.AutoSize = false;
            lblNoSel.TextAlign = ContentAlignment.MiddleCenter;
            lblNoSel.Location = new Point(0, 46);
            lblNoSel.Size = new Size(rightW, availH - 46);
            lblNoSel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlRight.Controls.Add(lblNoSel);

            pnlDetail = new Panel();
            pnlDetail.BackColor = Color.Transparent;
            pnlDetail.Location = new Point(0, 46);
            pnlDetail.Size = new Size(rightW, availH - 46);
            pnlDetail.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlDetail.AutoScroll = true;
            pnlDetail.Visible = false;
            pnlRight.Controls.Add(pnlDetail);

            BuildDetailWidgets(rightW);
        }

        private void BuildDetailWidgets(int pw)
        {
            int cw = pw - 28;

            // ── Message header card ──
            Panel hdrCard = MakeCard(14, 12, cw, 90);
            pnlDetail.Controls.Add(hdrCard);

            lblMsgTitle = new Label();
            lblMsgTitle.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblMsgTitle.ForeColor = Color.White;
            lblMsgTitle.BackColor = Color.Transparent;
            lblMsgTitle.AutoSize = true;
            lblMsgTitle.Location = new Point(14, 12);
            hdrCard.Controls.Add(lblMsgTitle);

            lblMsgFrom = new Label();
            lblMsgFrom.Font = new Font("Segoe UI", 9f);
            lblMsgFrom.ForeColor = TextMuted;
            lblMsgFrom.BackColor = Color.Transparent;
            lblMsgFrom.AutoSize = true;
            lblMsgFrom.Location = new Point(14, 40);
            hdrCard.Controls.Add(lblMsgFrom);

            lblMsgDate = new Label();
            lblMsgDate.Font = new Font("Segoe UI", 8.5f);
            lblMsgDate.ForeColor = TextMuted;
            lblMsgDate.BackColor = Color.Transparent;
            lblMsgDate.AutoSize = true;
            lblMsgDate.Location = new Point(14, 62);
            hdrCard.Controls.Add(lblMsgDate);

            lblMsgReadBadge = new Label();
            lblMsgReadBadge.Font = new Font("Segoe UI", 8f, FontStyle.Bold);
            lblMsgReadBadge.AutoSize = false;
            lblMsgReadBadge.Size = new Size(72, 22);
            lblMsgReadBadge.Location = new Point(cw - 86, 12);
            lblMsgReadBadge.TextAlign = ContentAlignment.MiddleCenter;
            hdrCard.Controls.Add(lblMsgReadBadge);

            // ── Message body card ──
            Panel bodyCard = MakeCard(14, 114, cw, 280);
            bodyCard.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlDetail.Controls.Add(bodyCard);

            bodyCard.Controls.Add(new Label { Text = "Message", Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = Gold, BackColor = Color.Transparent, AutoSize = true, Location = new Point(14, 12) });

            Panel msgDivider = new Panel();
            msgDivider.BackColor = BorderCol;
            msgDivider.Location = new Point(14, 36);
            msgDivider.Size = new Size(cw - 28, 1);
            bodyCard.Controls.Add(msgDivider);

            lblMsgBody = new Label();
            lblMsgBody.Font = new Font("Segoe UI", 10f);
            lblMsgBody.ForeColor = Color.FromArgb(220, 220, 220);
            lblMsgBody.BackColor = Color.Transparent;
            lblMsgBody.AutoSize = false;
            lblMsgBody.Size = new Size(cw - 28, 220);
            lblMsgBody.Location = new Point(14, 46);
            lblMsgBody.TextAlign = ContentAlignment.TopLeft;
            bodyCard.Controls.Add(lblMsgBody);

            // ── Action buttons card ──
            Panel actCard = MakeCard(14, 458, cw, 56);
            pnlDetail.Controls.Add(actCard);

            btnDone = MakeActionButton("✔  Mark as Done", 12, 14, 145, 28, Color.White, Color.FromArgb(20, 70, 45));
            btnDone.Click += (s, e) => MarkCurrentDone();
            actCard.Controls.Add(btnDone);

            btnPrint = MakeActionButton("🖨  Print", 170, 14, 100, 28, Gold, BgDark);
            btnPrint.Click += (s, e) => MessageBox.Show("Printing report...", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
            actCard.Controls.Add(btnPrint);

            btnPdf = MakeActionButton("⬇  PDF", 282, 14, 100, 28, Color.White, Color.FromArgb(50, 50, 50));
            btnPdf.Click += (s, e) => MessageBox.Show("Downloading PDF...", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
            actCard.Controls.Add(btnPdf);
        }

        // ─── SHOW DETAIL ──────────────────────────────────────────────────────
        private void ShowDetail()
        {
            if (_selectedIndex < 0 || _selectedIndex >= _filtered.Count) return;
            var r = _filtered[_selectedIndex];

            lblMsgTitle.Text = r.ReportTitle;
            lblMsgFrom.Text = $"From:  {r.Name}  ({r.ID})";
            lblMsgDate.Text = $"Date:  {r.Period}";
            lblMsgBody.Text = r.Message;

            // Read badge
            if (r.IsRead)
            {
                lblMsgReadBadge.Text = "✔ Read";
                lblMsgReadBadge.BackColor = Color.FromArgb(20, 60, 40);
                lblMsgReadBadge.ForeColor = Green;
                btnDone.Text = "✔  Done";
                btnDone.BackColor = Color.FromArgb(50, 50, 50);
                btnDone.ForeColor = TextMuted;
            }
            else
            {
                lblMsgReadBadge.Text = "● Unread";
                lblMsgReadBadge.BackColor = Color.FromArgb(60, 50, 10);
                lblMsgReadBadge.ForeColor = Yellow;
                btnDone.Text = "✔  Mark as Done";
                btnDone.BackColor = Color.FromArgb(20, 70, 45);
                btnDone.ForeColor = Color.White;
            }
            IntPtr br = CreateRoundRectRgn(0, 0, lblMsgReadBadge.Width, lblMsgReadBadge.Height, 10, 10);
            lblMsgReadBadge.Region = System.Drawing.Region.FromHrgn(br);

            lblNoSel.Visible = false;
            pnlDetail.Visible = true;
        }

        // ─── MARK DONE ───────────────────────────────────────────────────────
        private void MarkCurrentDone()
        {
            if (_selectedIndex < 0 || _selectedIndex >= _filtered.Count) return;
            _filtered[_selectedIndex].IsRead = true;
            ShowDetail();
            dgvList.InvalidateRow(_selectedIndex);
        }

        // ─── LIST HELPERS ─────────────────────────────────────────────────────
        private void LoadList()
        {
            dgvList.Rows.Clear();
            foreach (var r in _filtered)
                dgvList.Rows.Add(r.ID, r.ReportTitle);
            if (dgvList.Rows.Count > 0)
                dgvList.Rows[0].Selected = true;
        }

        private void FilterList()
        {
            string q = txtSearch.Text.Trim().ToLower();
            if (q == "search by name, id, title...") q = "";
            _filtered = string.IsNullOrEmpty(q)
                ? new List<ReportEntry>(_reports)
                : _reports.FindAll(r =>
                    r.Name.ToLower().Contains(q) ||
                    r.ID.ToLower().Contains(q) ||
                    r.ReportTitle.ToLower().Contains(q));
            _selectedIndex = 0;
            LoadList();
        }

        private void DeleteSelected()
        {
            if (dgvList.SelectedRows.Count == 0) return;
            var result = MessageBox.Show($"Delete {dgvList.SelectedRows.Count} selected report(s)?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            var toRemove = new List<int>();
            foreach (DataGridViewRow row in dgvList.SelectedRows)
                toRemove.Add(row.Index);
            toRemove.Sort();
            toRemove.Reverse();
            foreach (int idx in toRemove)
            {
                if (idx < _filtered.Count)
                {
                    var entry = _filtered[idx];
                    _reports.Remove(entry);
                    _filtered.Remove(entry);
                }
            }
            _selectedIndex = 0;
            LoadList();
            pnlDetail.Visible = false;
            lblNoSel.Visible = true;
        }

        private void DeleteAll()
        {
            if (_filtered.Count == 0) return;
            var result = MessageBox.Show("Delete ALL reports?", "Confirm Delete All", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            foreach (var entry in new List<ReportEntry>(_filtered))
                _reports.Remove(entry);
            _filtered.Clear();
            _selectedIndex = 0;
            LoadList();
            pnlDetail.Visible = false;
            lblNoSel.Visible = true;
        }

        // ─── CELL PAINTER ─────────────────────────────────────────────────────
        private void DgvList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex != dgvList.Columns["ID"].Index) return;
            e.Handled = true;

            bool isSel = (e.State & DataGridViewElementStates.Selected) != 0;
            Color bg = isSel ? Color.FromArgb(55, 55, 55)
                             : (e.RowIndex % 2 == 0 ? Color.FromArgb(38, 38, 38) : Color.FromArgb(44, 44, 44));
            using (var br = new SolidBrush(bg))
                e.Graphics.FillRectangle(br, e.CellBounds);

            if (e.RowIndex < _filtered.Count)
            {
                // Show green dot for read, yellow dot for unread
                Color dot = _filtered[e.RowIndex].IsRead ? Green : Yellow;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                int ds = 8, dx = e.CellBounds.X + 6, dy = e.CellBounds.Y + (e.CellBounds.Height - ds) / 2;
                using (var db = new SolidBrush(dot))
                    e.Graphics.FillEllipse(db, dx, dy, ds, ds);
            }

            // Use bold for unread reports
            bool isUnread = e.RowIndex < _filtered.Count && !_filtered[e.RowIndex].IsRead;
            var font = new Font("Segoe UI", 9f, isUnread ? FontStyle.Bold : FontStyle.Regular);
            TextRenderer.DrawText(e.Graphics, e.Value?.ToString() ?? "",
                font,
                new Rectangle(e.CellBounds.X + 18, e.CellBounds.Y, e.CellBounds.Width - 18, e.CellBounds.Height),
                isSel ? Color.White : Color.FromArgb(220, 220, 220),
                TextFormatFlags.VerticalCenter | TextFormatFlags.Left);

            using (var pen = new Pen(Color.FromArgb(50, 50, 50)))
                e.Graphics.DrawLine(pen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right, e.CellBounds.Bottom - 1);
        }



        private Panel MakeCard(int x, int y, int w, int h)
        {
            var p = new Panel { BackColor = BgCard, Location = new Point(x, y), Size = new Size(w, h) };
            IntPtr r = CreateRoundRectRgn(0, 0, w, h, 10, 10);
            p.Region = System.Drawing.Region.FromHrgn(r);
            return p;
        }

        private Button MakeActionButton(string text, int x, int y, int w, int h, Color fore, Color back)
        {
            var btn = new Button { Text = text, Font = new Font("Segoe UI", 9f, FontStyle.Bold), ForeColor = fore, BackColor = back, FlatStyle = FlatStyle.Flat, Location = new Point(x, y), Size = new Size(w, h), Cursor = Cursors.Hand };
            btn.FlatAppearance.BorderSize = 0;
            IntPtr r = CreateRoundRectRgn(0, 0, w, h, 10, 10);
            btn.Region = System.Drawing.Region.FromHrgn(r);
            return btn;
        }

        private void RoundPanel(Panel p, int radius)
        {
            IntPtr r = CreateRoundRectRgn(0, 0, p.Width, p.Height, radius, radius);
            p.Region = System.Drawing.Region.FromHrgn(r);
        }

        private void ApplyGridStyle(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.FromArgb(38, 38, 38);
            dgv.GridColor = Color.FromArgb(50, 50, 50);
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = BgDark;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(200, 200, 200);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9f, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = BgDark;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.ColumnHeadersHeight = 36;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(38, 38, 38);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9f);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(55, 55, 55);
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.RowTemplate.Height = 38;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(44, 44, 44);
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(60, 60, 60);
        }

        // ─── CLOCK ────────────────────────────────────────────────────────────
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
            if (hour >= 5 && hour < 12) AdjustLbl.Text = "Morning!";
            else if (hour >= 12 && hour < 18) AdjustLbl.Text = "Afternoon!";
            else AdjustLbl.Text = "Evening!";
        }

        // ─── FAKE DATE TIME PICKER ────────────────────────────────────────────
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
            { DateTimePicker.Location = dtpPanel.Location; DateTimePicker.Visible = true; DateTimePicker.Focus(); };
            dtpLabel.Click += (object? s, EventArgs e) =>
            { DateTimePicker.Location = dtpPanel.Location; DateTimePicker.Visible = true; DateTimePicker.Focus(); };
            DateTimePicker.ValueChanged += (object? s, EventArgs e) =>
            { dtpLabel.Text = DateTimePicker.Value.ToString("dddd, MMMM d, yyyy"); DateTimePicker.Visible = false; };
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

        // ─── ACTIVE BUTTON ────────────────────────────────────────────────────
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
            if (clockTimer != null) { clockTimer.Stop(); clockTimer.Dispose(); }
        }

        // ─── SIDEBAR PAINT ────────────────────────────────────────────────────
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

        // ─── STUBS ────────────────────────────────────────────────────────────
        private void AdminReports_Load(object sender, EventArgs e) { }

        // ─── NAVIGATION ───────────────────────────────────────────────────────
        private void AdminDashboardBtn_Click(object sender, EventArgs e)
        { new AdminDashboard().Show(); this.Hide(); }

        private void EmployeBtn_Click(object sender, EventArgs e)
        { new EmployeeSchedule().Show(); this.Hide(); }

        private void AttendanceLogsBtn_Click(object sender, EventArgs e)
        { new AttendanceLogs().Show(); this.Hide(); }

        private void ReportBtn_Click(object sender, EventArgs e) { }

        private void LogOutBtn_Click(object sender, EventArgs e)
        { new HomeForm().Show(); this.Hide(); }

        private void AttendanceLogsBtn_Click_1(object sender, EventArgs e)
        {
            AttendanceLogs AttendanceL = new AttendanceLogs();
            AttendanceL.Show();
            this.Hide();
        }

        private void EmployeBtn_Click_1(object sender, EventArgs e)
        {
            EmployeeSchedule emp = new EmployeeSchedule();
            emp.Show();
            this.Hide();
        }

        private void AdminDashboardBtn_Click_1(object sender, EventArgs e)
        {
            AdminDashboard AdminD = new AdminDashboard();
            AdminD.Show();
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