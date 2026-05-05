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
    public partial class Team : Form
    {
        // ─── GDI32 IMPORT (same as AdminDashboard) ───────────────────────────
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect,
            int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse
        );

        public Team()
        {
            InitializeComponent();
            ApplyRoundedCorners();
            StylePanels();
        }

        // ─── ROUNDED FORM CORNERS ────────────────────────────────────────────
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

        // ─── ROUNDED HELPER ──────────────────────────────────────────────────
        private void MakePanelRounded(Panel panel, int radius)
        {
            IntPtr hRgn = CreateRoundRectRgn(0, 0, panel.Width, panel.Height, radius, radius);
            panel.Region = System.Drawing.Region.FromHrgn(hRgn);
        }

        // ─── STYLE ALL 3 MEMBER PANELS ───────────────────────────────────────
        private void StylePanels()
        {
            // Apply rounded corners + dark bg to all 3 member cards
            foreach (Panel p in new[] { panel1, panel2, panel3 })
            {
                p.BackColor = Color.FromArgb(31, 31, 31);

                // Apply rounded corners after handle is created
                // (safe even if handle already exists)
                if (p.IsHandleCreated)
                {
                    MakePanelRounded(p, 20);
                }
                else
                {
                    p.HandleCreated += (object? s, EventArgs e) =>
                    {
                        MakePanelRounded(p, 20);
                    };
                }
            }
        }

        // ─── STUBS ───────────────────────────────────────────────────────────
        private void label7_Click(object sender, EventArgs e) { }

        private void Team_Load(object sender, EventArgs e)
        {
            // Re-apply after load to ensure regions are set correctly
            ApplyRoundedCorners();
            StylePanels();
        }
    }
}