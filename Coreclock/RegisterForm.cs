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
    public partial class RegisterForm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        ); // ✅ semicolon lang — dili curly brace

        public RegisterForm()
        {
            InitializeComponent();
            ApplyRoundedCorners();
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

        private void MakeButtonRounded(Button btn, int radius)
        {
            IntPtr hRgn = CreateRoundRectRgn(
                0, 0, btn.Width, btn.Height, radius, radius
            );
            btn.Region = System.Drawing.Region.FromHrgn(hRgn);
        }

        private void MakeTextBoxRounded(Control ctrl, int radius)
        {
            IntPtr hRgn = CreateRoundRectRgn(
                0, 0, ctrl.Width, ctrl.Height, radius, radius
            );
            ctrl.Region = System.Drawing.Region.FromHrgn(hRgn);
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            MakeButtonRounded(SignBtn, 20);
            MakeButtonRounded(RegisterBtn, 20);
            MakeButtonRounded(RegisterButton, 20);

            // Dark theme for Register
            RegisterBtn.FlatStyle = FlatStyle.Flat;
            RegisterBtn.FlatAppearance.BorderSize = 0;
            RegisterBtn.BackColor = Color.FromArgb(40, 40, 40);
            RegisterBtn.ForeColor = Color.White;

            // Dark theme for Sign
            SignBtn.FlatStyle = FlatStyle.Flat;
            SignBtn.FlatAppearance.BorderSize = 0;
            SignBtn.BackColor = Color.FromArgb(40, 40, 40);
            SignBtn.ForeColor = Color.White;

            // dark theme register button
            RegisterButton.FlatStyle = FlatStyle.Flat;
            RegisterButton.FlatAppearance.BorderSize = 0;
            RegisterButton.BackColor = Color.FromArgb(40, 40, 40);
            RegisterButton.ForeColor = Color.White;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {

        }

        private void SignBtn_Click(object sender, EventArgs e)
        {
            LoginFrm Login = new LoginFrm();
            Login.Show();
            this.Hide();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            LoginFrm Login = new LoginFrm();
            Login.Show();
            this.Hide();
        }
    }
}