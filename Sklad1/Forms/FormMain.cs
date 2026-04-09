using Microsoft.EntityFrameworkCore;
using Serilog;
using Sklad1.Data;
using Sklad1.Helpers;
using Sklad1.Properties;

namespace Sklad1.Forms
{
    /// <summary>
    /// Главная форма приложения, отображает список товаров
    /// </summary>
    public partial class FormMain : Form
    {
        /// <summary>
        /// Свойство для сохранения роли пользователя 
        /// </summary>
        public static UserRole UserRole { get; set; }

        public FormMain()
        {
            InitializeComponent();

            var displayRole = UserRole == UserRole.Admin ? Resources.Admin : Resources.Storekeeper;

            this.Text = string.Format(Resources.Title, displayRole);

            LoadProducts();

            btnCreate.Click += btnCreate_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;
            menuEditProduct.Click += menuEditProduct_Click;
            btnHistory.Click += btnHistory_Click;
            menuShipment.Click += menuShipment_Click;

            if (!IsAdmin())
            {
                btnDelete.Visible = btnEdit.Visible = btnHistory.Visible = false;
                menuProduct.Visible = menuCategory.Visible = false;
                menuEditProduct.Visible = menuEditCategory.Visible = false;
            }

            if (IsAdmin())
            {
                menuShipment.Visible = false;
            }
        }

        private bool IsAdmin() => UserRole == UserRole.Admin;

        private void LoadProducts()
        {
            try
            {
                using (var bd = new Context())
                {
                    var products = bd.Products.ToList();
                    var categories = bd.Categories.ToDictionary(c => c.Id, c => c.Name);

                    var data = products.Select(p => new
                    {
                        p.Article,
                        p.Name,
                        Category = categories.ContainsKey(p.CategoryId) ? categories[p.CategoryId] : string.Empty,
                        Quantity = p.InitialQuantity,
                        p.PurchasePrice,
                        Stock = p.Quantity
                    }).ToList();

                    dgvProducts.DataSource = data;

                    dgvProducts.Columns["Article"].HeaderText = Resources.Article;
                    dgvProducts.Columns["Name"].HeaderText = Resources.Name;
                    dgvProducts.Columns["Category"].HeaderText = Resources.Category;
                    dgvProducts.Columns["Quantity"].HeaderText = Resources.InitialQuantity;
                    dgvProducts.Columns["PurchasePrice"].HeaderText = Resources.PurchasePrice;
                    dgvProducts.Columns["Stock"].HeaderText = Resources.Stock;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, Resources.ProductLoadError);
                MessageBox.Show(Resources.ProductLoadError);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            CreateMenu.Show(btnCreate, 0, btnCreate.Height);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditMenu.Show(btnEdit, 0, btnEdit.Height);
        }

        private void menuCategory_Click(object sender, EventArgs e)
        {
            var form = new FormCategory();

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        private void menuProduct_Click(object sender, EventArgs e)
        {
            var form = new FormProduct();

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        private void menuEditCategory_Click(object sender, EventArgs e)
        {
            var selectedRow = dgvProducts.SelectedRows[0];
            var categoryName = selectedRow.Cells["Category"].Value.ToString();

            using (var bd = new Context())
            {
                var category = bd.Categories.FirstOrDefault(c => c.Name == categoryName);

                if (category == null)
                {
                    MessageBox.Show(Resources.CategoryNotFound);
                    return;
                }

                var form = new FormEditCategory(category);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadProducts();
                }

            }
        }
        private void menuEditProduct_Click(object sender, EventArgs e)
        {

            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show(Resources.SelectProduct);
                return;
            }

            var selectedRow = dgvProducts.SelectedRows[0];
            var article = selectedRow.Cells["Article"].Value.ToString();

            using (var bd = new Context())
            {
                var product = bd.Products.FirstOrDefault(p => p.Article == article);
                if (product != null)
                {
                    var form = new FormEditProduct(product);

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadProducts();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (dgvProducts.SelectedRows.Count == 0)
            {
                MessageBox.Show(Resources.SelectProduct);
                return;
            }

            var selectedRow = dgvProducts.SelectedRows[0];
            var article = selectedRow.Cells["Article"].Value.ToString();

            if (MessageBox.Show(Resources.ConfirmDeleteProductText, Resources.ConfirmDelete, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    using (var bd = new Context())
                    {
                        var product = bd.Products
                       .Include(p => p.ShipmentItems).FirstOrDefault(p => p.Article == article);

                        if (product != null)
                        {
                            bd.Products.Remove(product);
                            bd.SaveChanges();

                            MessageBox.Show(Resources.ProductDelete);
                            LoadProducts();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, Resources.ErrorDeletingProduct);
                    MessageBox.Show(Resources.ErrorSystem);
                }
            }
        }

        private void btnHistory_Click(object sender, EventArgs e)
        {
            new FormShipmentHistory().ShowDialog();
        }

        private void menuShipment_Click(object sender, EventArgs e)
        {
            var form = new FormShipment();

            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadProducts();
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.LogOut, Resources.LogOutText, MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                FormLogin loginForm = new FormLogin();
                loginForm.Show();

                this.Close(); 
            }
        }
    }
}
