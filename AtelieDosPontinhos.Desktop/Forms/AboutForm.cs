using AtelieDosPontinhos.Desktop.Helpers;
using AtelieDosPontinhos.Desktop.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AtelieDosPontinhos.Desktop.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            ThemeManager.ApplyTheme(
                this,
                animate: false);

            ThemeManager.ThemeChanged += OnThemeChanged;
            FormClosing += AboutForm_FormClosing;
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            BackColor = Color.FromArgb(177, 145, 217);
            Tag = "KeepBackColor";
        }

        private void AboutForm_FormClosing(
            object? sender,
            FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown)
                return;

            FormAnimator.AnimateOnClosing(
                this,
                e,
                closeWithSlide: true,
                durationMs: 320,
                offset: 40);
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void OnThemeChanged(bool isDark)
        {
            if (!IsHandleCreated ||
                IsDisposed ||
                Disposing)
            {
                return;
            }

            BeginInvoke((MethodInvoker)(() =>
            {
                if (!IsDisposed)
                {
                    ThemeManager.ApplyTheme(
                        this,
                        animate: true,
                        durationMs: 300);
                }
            }));
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            ThemeManager.ThemeChanged -= OnThemeChanged;
            base.OnFormClosed(e);
        }

        private void lblLinkGitHub_LinkClicked(
            object sender,
            LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName =
                    "https://github.com/jessicagavazzi2006-dev/AtelieDosPontinhos",
                UseShellExecute = true
            });
        }

        private void lblLinkReadme_LinkClicked(
            object sender,
            LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName =
                    "https://github.com/jessicagavazzi2006-dev/AtelieDosPontinhos/blob/master/README.md",
                UseShellExecute = true
            });
        }
    }
}