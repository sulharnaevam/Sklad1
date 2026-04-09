using Serilog;
using Sklad1.Data;
using Sklad1.Models;
using Sklad1.Properties;
using System.Text.RegularExpressions;

namespace Sklad1
{
    /// <summary>
    /// Форма редактирования товара
    /// </summary>
    public partial class FormEditProduct : Form
    {
        private Guid _productId;

        public FormEditProduct(Product product)
        {
            InitializeComponent();

            LoadCategories();

            _productId = product.Id;
            txtArticle.Text = product.Article;
            txtName.Text = product.Name;

            using (var bd = new Context())
            {
                var category = bd.Categories.Find(product.CategoryId);
                cmbCategory.Text = category?.Name ?? string.Empty;
            }

            txtPurchasePrice.Text = product.PurchasePrice.ToString();

            btnSave.Click += BtnSave_Click;
            btnCancel.Click += btnCancel_Click;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool IsValidName(string text)
        {
            var trimmed = text.Trim();
            return Regex.IsMatch(trimmed, @"^[а-яА-ЯёЁa-zA-Z0-9\s\-]+$");
        }

        private bool IsValidArticle(string text)
        {
            var trimmed = text.Trim();
            return Regex.IsMatch(trimmed, @"^[а-яА-ЯёЁa-zA-Z0-9\-]+$");
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var article = txtArticle.Text.Trim();
            var name = txtName.Text.Trim();
            var categoryName = cmbCategory.Text.Trim();
            var priceText = txtPurchasePrice.Text.Trim();

            if (string.IsNullOrWhiteSpace(article) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(priceText))
            {
                MessageBox.Show(Resources.FillAllFields);
                return;
            }

            if (cmbCategory.SelectedItem == null)
            {
                MessageBox.Show(Resources.CategoryError);
                return;
            }

            if (!IsValidName(name))
            {
                MessageBox.Show(Resources.InvalidProductName);
                return;
            }

            if (!IsValidArticle(article))
            {
                MessageBox.Show(Resources.InvalidArticle);
                return;
            }

            if (!IsValidName(categoryName))
            {
                MessageBox.Show(Resources.InvalidCategoryName);
                return;
            }

            if (!decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show(Resources.InvalidPrice);
                return;
            }

            if (price <= 0)
            {
                MessageBox.Show(Resources.InvalidPositivePrice);
                return;
            }

            try
            {
                using (var bd = new Context())
                {
                    if (bd.Products.Any(p => p.Article == article && p.Id != _productId))
                    {
                        MessageBox.Show(Resources.ArticleExists);
                        return;
                    }

                    if (bd.Products.Any(p => p.Name == name && p.Id != _productId))
                    {
                        MessageBox.Show(Resources.ProductNameExists);
                        return;
                    }

                    var category = bd.Categories.FirstOrDefault(c => c.Name == categoryName);

                    if (category == null)
                    {
                        MessageBox.Show(Resources.CategoryNotFound);
                        return;
                    }

                    var product = bd.Products.Find(_productId);

                    if (product != null)
                    {
                        product.Article = article;
                        product.Name = name;
                        product.CategoryId = category.Id;
                        product.PurchasePrice = price;
                        bd.SaveChanges();

                        MessageBox.Show(Resources.ProductEdit);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Resources.ErrorEditProduct);
                MessageBox.Show(Resources.ErrorSystem);
            }
        }

        private void LoadCategories()
        {
            using (var bd = new Context())
            {
                var categories = bd.Categories.OrderBy(c => c.Name).ToList();

                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "Name";
                cmbCategory.ValueMember = "Id";
            }
        }
    }
}
