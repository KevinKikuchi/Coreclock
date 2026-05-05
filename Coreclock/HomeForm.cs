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
    public partial class HomeForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        private System.Windows.Forms.Timer clockTimer;

        public HomeForm()
        {
            InitializeComponent();
            ApplyRoundedCorners();
            StartClock();
        }

        private void StartClock()
        {
            lblCurrentTime.Text = DateTime.Now.ToString("HH:mm:ss");

            clockTimer = new System.Windows.Forms.Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();
        }

        // ✅ One ClockTimer_Tick lang — clock update ra
        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            lblCurrentTime.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void MakeButtonRounded(Button btn, int radius)
        {
            IntPtr hRgn = CreateRoundRectRgn(
                0, 0, btn.Width, btn.Height, radius, radius
            );
            btn.Region = System.Drawing.Region.FromHrgn(hRgn);
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

        // ✅ Button styling diri sa Load — walay delay, no duplicate
        private void HomeForm_Load(object sender, EventArgs e)
        {
            MakeButtonRounded(SignInBtn, 20);
            MakeButtonRounded(RegisterBtn, 20);

            RegisterBtn.FlatStyle = FlatStyle.Flat;
            RegisterBtn.FlatAppearance.BorderSize = 0;
            RegisterBtn.BackColor = Color.FromArgb(40, 40, 40);
            RegisterBtn.ForeColor = Color.White;

            SignInBtn.FlatStyle = FlatStyle.Flat;
            SignInBtn.FlatAppearance.BorderSize = 0;
            SignInBtn.BackColor = Color.FromArgb(40, 40, 40);
            SignInBtn.ForeColor = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoginFrm LoginF = new LoginFrm();
            LoginF.Show();
            this.Hide();
        }

        private void lblCurrentTime_Click(object sender, EventArgs e) { }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            EmployeeDashboard EmployeeD = new EmployeeDashboard();
            EmployeeD.Show();
            this.Hide();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            AdminDashboard AdminD = new AdminDashboard();
            AdminD.Show();
            this.Hide();
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            RegisterForm RegisterF = new RegisterForm();
            RegisterF.Show();
            this.Hide();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            Team bisakols = new Team();
            bisakols.Show();
            this.Hide();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            AdminDashboard adminD = new AdminDashboard();
           adminD.Show(); 
            this.Hide();
        }
    }
}