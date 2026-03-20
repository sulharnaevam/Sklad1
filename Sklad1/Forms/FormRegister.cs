using Sklad1.Data;
using Sklad1.Helpers;
using Sklad1.Properties;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Sklad1.Forms
{
    /// <summary>
    /// Форма регистрации нового пользователя
    /// </summary>
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
            btnRegister.Click += BtnRegister_Click;
        }

        private bool IsValidName(string name)
        {
            var trimmed = name.Trim();
            return trimmed.Length >= 2 && trimmed.Length <= 50 && Regex.IsMatch(trimmed, @"^[а-яА-ЯёЁa-zA-Z\-]+$");
        }

        private bool IsValidEmail(string email)
        {
            var trimmed = email.Trim();

            if (trimmed.Length < 5 || trimmed.Length > 100)
                return false;

            if (trimmed.Contains("\0") || trimmed.Contains("\n") || trimmed.Contains("\r"))
                return false;

            try
            {
                var addr = new MailAddress(trimmed);
                return addr.Address == trimmed && !trimmed.Contains(" ");
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
            var trimmed = password.Trim();

            if (trimmed.Length < 6 || trimmed.Length > 50)
                return false;

            if (trimmed.Contains(" ") || trimmed.Contains("\0") ||
                trimmed.Contains("\n") || trimmed.Contains("\r"))
                return false;

            foreach (char c in trimmed)
            {
                if ((c >= 0x1F300 && c <= 0x1F6FF) ||
                    (c >= 0x2600 && c <= 0x26FF) ||
                    (c >= 0x2700 && c <= 0x27BF))
                {
                    return false;
                }
            }

            return true;
        }


        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (!AllFieldsFilled())
                return;

            if (!AllDataValid())
                return;

            SaveUser();
        }

        private bool AllFieldsFilled()
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show(Resources.FillAllFields);
                return false;
            }

            return true;
        }

        private bool AllDataValid()
        {
            if (!IsValidName(txtLastName.Text))
            {
                MessageBox.Show(Resources.InvalidLastName);
                return false;
            }

            if (!IsValidName(txtFirstName.Text))
            {
                MessageBox.Show(Resources.InvalidFirstName);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtMiddleName.Text) && !IsValidName(txtMiddleName.Text))
            {
                MessageBox.Show(Resources.InvalidMiddleName);
                return false;
            }

            if (!IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show(Resources.InvalidEmail);
                return false;
            }

            if (!IsValidPassword(txtPassword.Text))
            {
                MessageBox.Show(Resources.InvalidPassword);
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show(Resources.PasswordsDontMatch);
                return false;
            }

            return true;
        }

        private void SaveUser()
        {
            try
            {
                using (var bd = new Context())
                {
                    if (bd.Users.Any(u => u.Email.ToLower() == txtEmail.Text.Trim().ToLower()))
                    {
                        MessageBox.Show(Resources.EmailExists);
                        return;
                    }

                    var newUser = new User
                    {
                        Id = Guid.NewGuid(),
                        LastName = txtLastName.Text.Trim(),
                        FirstName = txtFirstName.Text.Trim(),
                        MiddleName = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? string.Empty : txtMiddleName.Text.Trim(),
                        Email = txtEmail.Text.Trim().ToLower(),
                        PasswordHash = Password.HashPassword(txtPassword.Text),
                        Role = UserRole.Кладовщик
                    };

                    bd.Users.Add(newUser);
                    bd.SaveChanges();

                    MessageBox.Show(Resources.RegisterSuccess);
                    new FormLogin().Show();
                    Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(Resources.ErrorRegister, ex);
                MessageBox.Show(Resources.ErrorSystem);
            }
        }
    }
}          
