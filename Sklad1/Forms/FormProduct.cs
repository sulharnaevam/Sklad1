using Serilog;
using Sklad1.Data;
using Sklad1.Helpers;
using Sklad1.Properties;
using System.Text.RegularExpressions;
using Sklad1.Models;

namespace Sklad1.Forms
{
    public partial class FormProduct : Form
    {
        public FormProduct()
        {
            InitializeComponent();

            LoadCategories();

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
            return Regex.IsMatch(trimmed, @"^[0-9\s\-]+$");
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            var article = txtArticle.Text.Trim();
            var name = txtName.Text.Trim();
            var categoryName = cmbCategory.Text.Trim();
            var priceText = txtPurchasePrice.Text.Trim();
            var quantityText = txtQuantity.Text.Trim();

            if (string.IsNullOrWhiteSpace(article) ||
                string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(categoryName) ||
                string.IsNullOrWhiteSpace(priceText) ||
                string.IsNullOrWhiteSpace(quantityText))
            {
                MessageBox.Show(Resources.FillAllFields);
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

            if (!int.TryParse(quantityText, out int quantity) || quantity <= 0)
            {
                MessageBox.Show(Resources.InvalidQuantity);
                return;
            }

            try
            {
                using (var bd = new Context())
                {
                    if (bd.Products.Any(p => p.Article == article))
                    {
                        MessageBox.Show(Resources.ArticleExists);
                        return;
                    }

                    if (bd.Products.Any(p => p.Name == name))
                    {
                        MessageBox.Show(Resources.ProductExists);
                        return;
                    }

                    var category = bd.Categories.FirstOrDefault(c => c.Name == categoryName);
                    if (category == null)
                    {
                        MessageBox.Show(Resources.CategoryNotFound);
                        return;
                    }

                    var product = new Product
                    {
                        Id = Guid.NewGuid(),
                        Article = article,
                        Name = name,
                        CategoryId = category.Id,
                        PurchasePrice = price,
                        InitialQuantity = quantity,
                        Quantity = quantity
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
                Log.Error(ex, Resources.ErrorCreateProduct);
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