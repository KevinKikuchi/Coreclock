using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
            IntPtr hRgn = CreateRoundRectRgn(0, 0, ctrl.Width, ctrl.Height, radius, radius);
            ctrl.Region = System.Drawing.Region.FromHrgn(hRgn);
        }

        private void MakeButtonRounded(Button btn, int radius)
        {
            IntPtr hRgn = CreateRoundRectRgn(0, 0, btn.Width, btn.Height, radius, radius);
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

        private void LoginFrm_Load(object sender, EventArgs e)
        {
            MakeButtonRounded(SignBtn, 20);
            MakeButtonRounded(RegisterBtn, 20);
            MakeButtonRounded(LoginBtn, 20);
            MakeTextBoxRounded(LoginBtn, 20);

            RegisterBtn.FlatStyle = FlatStyle.Flat;
            RegisterBtn.FlatAppearance.BorderSize = 0;
            RegisterBtn.BackColor = Color.FromArgb(40, 40, 40);
            RegisterBtn.ForeColor = Color.White;

            SignBtn.FlatStyle = FlatStyle.Flat;
            SignBtn.FlatAppearance.BorderSize = 0;
            SignBtn.BackColor = Color.FromArgb(40, 40, 40);
            SignBtn.ForeColor = Color.White;

            LoginBtn.FlatStyle = FlatStyle.Flat;
            LoginBtn.FlatAppearance.BorderSize = 0;
            LoginBtn.BackColor = Color.FromArgb(40, 40, 40);
            LoginBtn.ForeColor = Color.White;

            PasswordBox.ForeColor = Color.White;
            PasswordBox.BackColor = Color.FromArgb(40, 40, 40);
            UsernameBox.BackColor = Color.FromArgb(40, 40, 40);
            UsernameBox.ForeColor = Color.White;
        }

        // ── LOGIN BUTTON ─────────────────────────────────────────────────────
        // NOTE: UsernameBox here is used as EMAIL input on the login screen.
        //       The label on the form should say "Email" — or rename the textbox
        //       in designer to EmailBox if you want cleaner code.
        private async void LoginBtn_Click(object sender, EventArgs e)
        {
            var email    = UsernameBox.Text.Trim();
            var password = PasswordBox.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your email and password.", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            LoginBtn.Enabled = false;
            LoginBtn.Text    = "Logging in...";

            var (success, role, error) = await SupabaseHelper.Instance.SignInAsync(email, password);

            LoginBtn.Enabled = true;
            LoginBtn.Text    = "Login";

            if (success)
            {
                if (role == "admin")
                {
                    AdminDashboard adminDash = new AdminDashboard();
                    adminDash.Show();
                }
                else
                {
                    EmployeeDashboard empDash = new EmployeeDashboard();
                    empDash.Show();
                }
                this.Hide();
            }
            else
            {
                if (SupabaseHelper.Instance.IsInvalidCredentialsError(error))
                {
                    MessageBox.Show("Invalid email or password.", "Login Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Login failed: {error}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ── NAVIGATE TO REGISTER ─────────────────────────────────────────────
        private void button2_Click(object sender, EventArgs e)
        {
            RegisterForm Register = new RegisterForm();
            Register.Show();
            this.Hide();
        }

        private void RegisterBtn_Click(object sender, EventArgs e)
        {
            RegisterForm Register = new RegisterForm();
            Register.Show();
            this.Hide();
        }

        // ── WINDOW CONTROLS ──────────────────────────────────────────────────
        private void pictureBox4_Click(object sender, EventArgs e)    { WindowState = FormWindowState.Minimized; }
        private void pictureBox4_Click_1(object sender, EventArgs e)  { WindowState = FormWindowState.Minimized; }
        private void pictureBox5_Click(object sender, EventArgs e)    { this.Close(); }
        private void pictureBox5_Click_1(object sender, EventArgs e)  { this.Close(); }

        // ── STUBS (keep to avoid designer errors) ────────────────────────────
        private void SignBtn_Click(object sender, EventArgs e)         { }
        private void pictureBox1_Click(object sender, EventArgs e)     { }
        private void textBox1_TextChanged(object sender, EventArgs e)  { }
    }
}