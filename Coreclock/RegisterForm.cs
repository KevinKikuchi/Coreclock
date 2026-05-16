using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
        );

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
            IntPtr hRgn = CreateRoundRectRgn(0, 0, btn.Width, btn.Height, radius, radius);
            btn.Region = System.Drawing.Region.FromHrgn(hRgn);
        }

        private void MakeTextBoxRounded(Control ctrl, int radius)
        {
            IntPtr hRgn = CreateRoundRectRgn(0, 0, ctrl.Width, ctrl.Height, radius, radius);
            ctrl.Region = System.Drawing.Region.FromHrgn(hRgn);
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            MakeButtonRounded(SignBtn, 20);
            MakeButtonRounded(RegisterBtn, 20);
            MakeButtonRounded(RegisterButton, 20);

            RegisterBtn.FlatStyle = FlatStyle.Flat;
            RegisterBtn.FlatAppearance.BorderSize = 0;
            RegisterBtn.BackColor = Color.FromArgb(40, 40, 40);
            RegisterBtn.ForeColor = Color.White;

            SignBtn.FlatStyle = FlatStyle.Flat;
            SignBtn.FlatAppearance.BorderSize = 0;
            SignBtn.BackColor = Color.FromArgb(40, 40, 40);
            SignBtn.ForeColor = Color.White;

            RegisterButton.FlatStyle = FlatStyle.Flat;
            RegisterButton.FlatAppearance.BorderSize = 0;
            RegisterButton.BackColor = Color.FromArgb(40, 40, 40);
            RegisterButton.ForeColor = Color.White;
        }

        // ── REGISTER BUTTON ──────────────────────────────────────────────────
        private async void RegisterButton_Click(object sender, EventArgs e)
        {
            // ── Read fields ──
            // UsernameBox  = First Name  (based on your designer)
            // textBox5     = Last Name
            // ContactBox   = Contact Number
            // textBox4     = Email
            // textBox2     = Password
            // textBox3     = Confirm Password
            var firstName       = UsernameBox.Text.Trim();
            var lastName        = textBox5.Text.Trim();
            var contact         = ContactBox.Text.Trim();
            var email           = textBox4.Text.Trim();
            var password        = textBox2.Text;
            var confirmPassword = textBox3.Text;
            var fullName        = $"{firstName} {lastName}".Trim();

            // ── Validation ──
            if (string.IsNullOrEmpty(firstName)  ||
                string.IsNullOrEmpty(lastName)   ||
                string.IsNullOrEmpty(contact)    ||
                string.IsNullOrEmpty(email)      ||
                string.IsNullOrEmpty(password)   ||
                string.IsNullOrEmpty(confirmPassword))
            {
                MessageBox.Show("Please fill in all fields.", "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show(
                    "Passwords do not match.\n\nPlease re-type both password fields carefully.",
                    "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Text = "";
                textBox3.Text = "";
                textBox2.Focus();
                return;
            }

            if (password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.",
                    "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ── Call Supabase ──
            RegisterButton.Enabled = false;
            RegisterButton.Text    = "Registering...";

            var (success, error) = await SupabaseHelper.Instance.SignUpAsync(
                email, password, fullName, contact);

            RegisterButton.Enabled = true;
            RegisterButton.Text    = "Register";

            if (success)
            {
                MessageBox.Show(
                    "Registration successful! You can now log in.",
                    "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoginFrm login = new LoginFrm();
                login.Show();
                this.Hide();
            }
            else
            {
                if (error?.Contains("already registered", StringComparison.OrdinalIgnoreCase) == true)
                {
                    MessageBox.Show("This email is already registered.", "Registration Failed",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Registration failed: {error}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // ── SIGN IN BUTTON ───────────────────────────────────────────────────
        private void SignBtn_Click(object sender, EventArgs e)
        {
            LoginFrm Login = new LoginFrm();
            Login.Show();
            this.Hide();
        }

        // ── WINDOW CONTROLS ──────────────────────────────────────────────────
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
            // Tab toggle button — no action needed
        }
    }
}