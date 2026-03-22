using Sklad1.Data;
using Sklad1.Properties;
using Sklad1.Helpers;
using Serilog;

namespace Sklad1.Forms
{
    /// <summary>
    /// Форма создания нового товара
    /// </summary>
    public partial class FormProduct : Form
    {
        public FormProduct()
        {
            InitializeComponent();
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += btnCancel_Click;
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtArticle.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtCategory.Text) ||
                string.IsNullOrWhiteSpace(txtPurchasePrice.Text))
            {
                MessageBox.Show(Resources.FillAllFields);
                return;
            }

            if (!decimal.TryParse(txtPurchasePrice.Text, out decimal price))
            {
                MessageBox.Show(Resources.InvalidPrice);
                return;
            }

            try
            {
                using (var bd = new Context())
                {
                    if (bd.Products.Any(p => p.Article == txtArticle.Text))
                    {
                        MessageBox.Show(Resources.ArticleExists);
                        return;
                    }

                    var category = bd.Categories.FirstOrDefault(c => c.Name == txtCategory.Text);
                    if (category == null)
                    {
                        MessageBox.Show(Resources.CategoryNotFound);
                        return;
                    }

                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Article = txtArticle.Text,
                        Name = txtName.Text,
                        CategoryId = category.Id,
                        PurchasePrice = price,
                    };

                    bd.Products.Add(product);
                    bd.SaveChanges();

                    MessageBox.Show(Resources.ProductCreate);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Ошибка при создании товара: {Article}", txtArticle.Text);
                MessageBox.Show(Resources.ErrorSystem);
            }
        }
    }
}
