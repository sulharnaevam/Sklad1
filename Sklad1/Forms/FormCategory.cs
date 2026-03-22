using Sklad1.Data;
using Sklad1.Properties;
using Sklad1.Helpers;
using Serilog;

namespace Sklad1.Forms
{
    /// <summary>
    /// Форма создания новой категории товаров
    /// </summary>
    public partial class FormCategory : Form
    {
        public FormCategory()
        {
            InitializeComponent();
            btnCreate.Click += BtnCreate_Click;
            btnCancel.Click += BtnCancel_Click;
        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(Resources.EnterCategoryName);
                return;
            }
            try
            {
                using (var bd = new Context())
                {
                    if (bd.Categories.Any(c => c.Name == txtName.Text))
                    {
                        MessageBox.Show(Resources.CategoryExists);
                        return;
                    }
                    var category = new Category
                    {
                        Id = Guid.NewGuid(),
                        Name = txtName.Text,
                        Description = txtDescription.Text
                    };

                    bd.Categories.Add(category);
                    bd.SaveChanges();

                    MessageBox.Show(Resources.CategoryCreate);

                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка при создании категории {CategoryName}", txtName.Text);
                MessageBox.Show(Resources.ErrorSystem);
            }
        }
    }
}

