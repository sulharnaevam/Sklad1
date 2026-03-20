using Sklad1.Data;
using Sklad1.Forms;
using Sklad1.Helpers;
using Sklad1.Properties;
using System.Net.Mail;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace Sklad1
{
    /// <summary>
    /// Форма входа 
    /// </summary>
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
            btnLogin.Click += BtnLogin_Click;
            lnkRegister.Click += lnkRegister_Click;
        }

        private void lnkRegister_Click(object sender, EventArgs e)
        {
            new FormRegister().Show();
            this.Hide();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (!FieldsFilled())
                return;

            if (!IsValidEmail(txtEmail.Text))
                return;

            FindUser();

        }
        private bool FieldsFilled()
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show(Resources.EnterEmailAndPassword);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            var trimmed = email.Trim();
            if (email.Contains(" "))
            {
                MessageBox.Show(Resources.InvalidEmail);
                return false;
            }

            try
            {
                var addr = new MailAddress(email);
                if (addr.Address != email)
                {
                    MessageBox.Show(Resources.InvalidEmail);
                    return false;
                }
            }
            catch
            {
                MessageBox.Show(Resources.InvalidEmail);
                return false;
            }

            return true;
        }
        
        private void FindUser()
        {
            try
            {
                using (var bd = new Context())
                {
                    var user = bd.Users.FirstOrDefault(u =>
                        u.Email == txtEmail.Text.Trim() &&
                        u.PasswordHash == Password.HashPassword(txtPassword.Text.Trim()));

                    if (user == null)
                    {
                        MessageBox.Show(Resources.UserNotFound);
                        return;
                    }

                    if (user.PasswordHash != Password.HashPassword(txtPassword.Text.Trim()))
                    {
                        MessageBox.Show(Resources.WrongPassword);
                        return;
                    }

                    FormMain.UserRole = user.Role;

                    var mainForm = new FormMain();
                    mainForm.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(Resources.ErrorLogin, ex);
                MessageBox.Show(Resources.ErrorSystem);
            }
        }
    }
}
