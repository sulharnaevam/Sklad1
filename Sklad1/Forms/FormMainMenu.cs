using Sklad1.Data;
using Sklad1.Helpers;
using Sklad1.Models;
using Sklad1.Properties;

namespace Sklad1.Forms
{
    /// <summary>
    /// Главное меню приложения
    /// </summary>
    public partial class FormMainMenu : Form
    {
        public FormMainMenu()
        {
            InitializeComponent();
            SetPermissions();
            this.FormClosing += FormMainMenu_FormClosing; 
        }

        private void SetPermissions()
        {
            if (CurrentUser.Role != UserRole.Admin)
            {
                tsmiReports.Visible = false;
                tsmiSettings.Visible = false;
            }
        }

        private void tsmiSklad_Click(object sender, EventArgs e)
        {
            var form = new FormMain();
            form.ShowDialog();
        }

        private void tsmiSupply_Click(object sender, EventArgs e)
        {
            var form = new FormSupply();
            form.ShowDialog();
        }

        private void tsmiSupplyImport_Click(object sender, EventArgs e)
        {
            var form = new FormSupplyImport();
            form.ShowDialog();
        }

        private void tsmiExpiry_Click(object sender, EventArgs e)
        {
            var form = new FormExpiryDates();
            form.ShowDialog();
        }

        private void tsmiReports_Click(object sender, EventArgs e)
        {
            var form = new FormAnalyticReport();
            form.ShowDialog();
        }

        private void tsmiSettings_Click(object sender, EventArgs e)
        {
            var form = new FormCurrencySettings();
            form.ShowDialog();
        }

        private void tsmiSupplies_Click(object sender, EventArgs e)
        {
            //чтоб открыть подпункты
        }

        private void tsmiLogOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.LogOut, Resources.LogOutText, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FormLogin loginForm = new FormLogin();
                loginForm.Show();

                this.Close();
            }
        }
        private void FormMainMenu_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(Resources.LogOut, Resources.LogOutText, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FormLogin loginForm = new FormLogin();
                loginForm.Show();
            }
            else
            {
                e.Cancel = true;  
            }
        }
    }
}