using Sklad1.Data;
using Sklad1.Helpers;
using Sklad1.Properties;

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

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
            {
                MessageBox.Show(Properties.Resources.FillAllFields);
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show(Resources.PasswordsDontMatch);
                return;
            }

            try
            {
                using (var bd = new Context())
                {
                    if (bd.Users.Any(u => u.Email == txtEmail.Text))
                    {
                        MessageBox.Show(Resources.EmailExists);
                        return;
                    }

                    var newUser = new User
                    {
                        Id = Guid.NewGuid(),
                        LastName = txtLastName.Text,
                        FirstName = txtFirstName.Text,
                        MiddleName = txtMiddleName.Text,
                        Email = txtEmail.Text,
                        PasswordHash = Password.HashPassword(txtPassword.Text),
                        Role = UserRole.Кладовщик
                    };

                    bd.Users.Add(newUser);
                    bd.SaveChanges();

                    MessageBox.Show(Resources.RegisterSuccess);

                    var login = new FormLogin();
                    login.Show();
                    this.Close();
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
