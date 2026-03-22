using Sklad1.Data;
using Sklad1.Properties;
using Sklad1.Helpers;
using Serilog;

namespace Sklad1.Forms
{
    /// <summary>
    /// Форма редактирования категории товаров
    /// </summary>
    public partial class FormEditCategory : Form
    {
        private Guid _categoryId;
        public FormEditCategory(Category category)
        {
            InitializeComponent();

            _categoryId = category.Id;
            txtName.Text = category.Name;
            txtDescription.Text = category.Description;

            btnUpdate.Click += BtnUpdate_Click;
            btnCancel.Click += btnCancel_Click;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
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
                    if (bd.Categories.Any(c => c.Name == txtName.Text && c.Id != _categoryId))
                    {
                        MessageBox.Show(Resources.CategoryExists);
                        return;
                    }

                    var category = bd.Categories.Find(_categoryId);
                    if (category != null)
                    {
                        category.Name = txtName.Text;
                        category.Description = txtDescription.Text;
                        bd.SaveChanges();

                        MessageBox.Show(Resources.CategoryEdit);

                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка при редактировании категории {CategoryId}", _categoryId);
                MessageBox.Show(Resources.ErrorSystem);
            }
        }
    }
}

