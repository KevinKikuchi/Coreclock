using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Coreclock
{
    public partial class LoginFrm : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        public LoginFrm()
        {
            InitializeComponent();
            ApplyRoundedCorners();

        }

        private void MakeTextBoxRounded(Control ctrl, int radius)
        {
            IntPtr hRgn = CreateRoundRectRgn(
                0, 0, ctrl.Width, ctrl.Height, radius, radius
            );
            ctrl.Region = System.Drawing.Region.FromHrgn(hRgn);
        }

        private void LoginFrm_Load(object sender, EventArgs e)
        {

            MakeButtonRounded(SignBtn, 20);
            MakeButtonRounded(RegisterBtn, 20);
            MakeButtonRounded(LoginBtn, 20);
            MakeTextBoxRounded(LoginBtn, 20);


            //Dark theme for Register
            RegisterBtn.FlatStyle = FlatStyle.Flat;
            RegisterBtn.FlatAppearance.BorderSize = 0;
            RegisterBtn.BackColor = Color.FromArgb(40, 40, 40);
            RegisterBtn.ForeColor = Color.White;

            //Dark theme for SignUp
            SignBtn.FlatStyle = FlatStyle.Flat;
            SignBtn.FlatAppearance.BorderSize = 0;
            SignBtn.BackColor = Color.FromArgb(40, 40, 40);
            SignBtn.ForeColor = Color.White;

            //dark theme for login button
            LoginBtn.FlatStyle = FlatStyle.Flat;
            LoginBtn.FlatAppearance.BorderSize = 0;
            LoginBtn.BackColor = Color.FromArgb(40, 40, 40);
            LoginBtn.ForeColor = Color.White;

            //datk theme for usernmabeox and passwordbox
            PasswordBox.ForeColor = Color.White;
            PasswordBox.BackColor = Color.FromArgb(40, 40, 40);
            UsernameBox.BackColor = Color.FromArgb(40, 40, 40);
            UsernameBox.ForeColor = Color.White;

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

        private void pictureBox1_Click(object sender, EventArgs e) { }

        private void SignBtn_Click(object sender, EventArgs e)
        {
            // TODO: add login logic
            // HomeFrm home = new HomeFrm();
            // home.Show();
            // this.Hide();
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            // TODO: add register logic
            // RegisterFrm reg = new RegisterFrm();
            // reg.Show();
            // this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }

        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RegisterForm Register = new RegisterForm();
            Register.Show();
            this.Hide();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            EmployeeDashboard EmployeeD = new EmployeeDashboard();
            EmployeeD.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click_1(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;

        }
    }
}