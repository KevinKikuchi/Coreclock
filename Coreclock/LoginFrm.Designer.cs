namespace Coreclock
{
    partial class LoginFrm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginFrm));
            label3 = new Label();
            SignBtn = new Button();
            RegisterBtn = new Button();
            label5 = new Label();
            UsernameBox = new TextBox();
            label6 = new Label();
            PasswordBox = new TextBox();
            label11 = new Label();
            label7 = new Label();
            LoginBtn = new Button();
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
            pictureBox1 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.DarkGray;
            label3.Location = new Point(118, 184);
            label3.Name = "label3";
            label3.Size = new Size(92, 13);
            label3.TabIndex = 2;
            label3.Text = "Sign in to continue";
            // 
            // SignBtn
            // 
            SignBtn.BackColor = Color.Black;
            SignBtn.FlatStyle = FlatStyle.Flat;
            SignBtn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            SignBtn.ForeColor = Color.Transparent;
            SignBtn.Location = new Point(34, 208);
            SignBtn.Name = "SignBtn";
            SignBtn.Size = new Size(125, 32);
            SignBtn.TabIndex = 3;
            SignBtn.Text = "Sign In";
            SignBtn.UseVisualStyleBackColor = false;
            // 
            // RegisterBtn
            // 
            RegisterBtn.BackColor = Color.Black;
            RegisterBtn.FlatStyle = FlatStyle.Flat;
            RegisterBtn.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            RegisterBtn.ForeColor = Color.Transparent;
            RegisterBtn.Location = new Point(169, 208);
            RegisterBtn.Name = "RegisterBtn";
            RegisterBtn.Size = new Size(125, 32);
            RegisterBtn.TabIndex = 4;
            RegisterBtn.Text = "Register";
            RegisterBtn.UseVisualStyleBackColor = false;
            RegisterBtn.Click += button2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.DarkGray;
            label5.Location = new Point(34, 276);
            label5.Name = "label5";
            label5.Size = new Size(93, 14);
            label5.TabIndex = 20;
            label5.Text = "EMAIL ADDRESS";
            // 
            // UsernameBox
            // 
            UsernameBox.BackColor = Color.FromArgb(31, 31, 31);
            UsernameBox.BorderStyle = BorderStyle.FixedSingle;
            UsernameBox.Location = new Point(34, 293);
            UsernameBox.Name = "UsernameBox";
            UsernameBox.Size = new Size(260, 23);
            UsernameBox.TabIndex = 21;
            UsernameBox.TextChanged += textBox1_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = Color.Transparent;
            label6.Font = new Font("Arial", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.DarkGray;
            label6.Location = new Point(34, 340);
            label6.Name = "label6";
            label6.Size = new Size(67, 14);
            label6.TabIndex = 22;
            label6.Text = "PASSWORD";
            // 
            // PasswordBox
            // 
            PasswordBox.BackColor = Color.FromArgb(31, 31, 31);
            PasswordBox.BorderStyle = BorderStyle.FixedSingle;
            PasswordBox.Location = new Point(34, 355);
            PasswordBox.Name = "PasswordBox";
            PasswordBox.PasswordChar = '*';
            PasswordBox.Size = new Size(260, 23);
            PasswordBox.TabIndex = 23;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.ForeColor = SystemColors.AppWorkspace;
            label11.Location = new Point(71, 564);
            label11.Name = "label11";
            label11.Size = new Size(164, 15);
            label11.TabIndex = 24;
            label11.Text = "Developed By: B I S A C O D E ";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.Transparent;
            label7.Font = new Font("Calibri", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.Goldenrod;
            label7.Location = new Point(204, 381);
            label7.Name = "label7";
            label7.Size = new Size(90, 13);
            label7.TabIndex = 25;
            label7.Text = "Forgot Password?";
            // 
            // LoginBtn
            // 
            LoginBtn.BackColor = Color.Black;
            LoginBtn.FlatStyle = FlatStyle.Flat;
            LoginBtn.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LoginBtn.ForeColor = Color.Transparent;
            LoginBtn.Location = new Point(34, 426);
            LoginBtn.Name = "LoginBtn";
            LoginBtn.Size = new Size(260, 38);
            LoginBtn.TabIndex = 26;
            LoginBtn.Text = "Log in";
            LoginBtn.UseVisualStyleBackColor = false;
            LoginBtn.Click += LoginBtn_Click;
            // 
            // pictureBox4
            // 
            pictureBox4.BackColor = Color.Transparent;
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(259, 12);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(23, 19);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 28;
            pictureBox4.TabStop = false;
            pictureBox4.Click += pictureBox4_Click_1;
            // 
            // pictureBox5
            // 
            pictureBox5.BackColor = Color.Transparent;
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(288, 12);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(23, 19);
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.TabIndex = 29;
            pictureBox5.TabStop = false;
            pictureBox5.Click += pictureBox5_Click_1;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(12, 37);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(299, 144);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 30;
            pictureBox1.TabStop = false;
            // 
            // LoginFrm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(323, 588);
            Controls.Add(pictureBox1);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox4);
            Controls.Add(LoginBtn);
            Controls.Add(label7);
            Controls.Add(label11);
            Controls.Add(PasswordBox);
            Controls.Add(label6);
            Controls.Add(UsernameBox);
            Controls.Add(label5);
            Controls.Add(RegisterBtn);
            Controls.Add(SignBtn);
            Controls.Add(label3);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LoginFrm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "LoginFrm";
            Load += LoginFrm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label3;
        private Button SignBtn;
        private Button RegisterBtn;
        private Label label4;
        private Label label5;
        private TextBox UsernameBox;
        private Label label6;
        private TextBox PasswordBox;
        private Label label11;
        private Label label7;
        private Button LoginBtn;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
    }
}