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
            this.Text = string.Format(Resources.Title, UserRole);
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                using (var bd = new Context())
                {
                    var products = bd.Products.ToList();
                    var categories = bd.Categories.ToDictionary(c => c.Id, c => c.Name);

                    var data = new List<dynamic>();
                    foreach (var p in products)
                    {
                        var catName = categories.ContainsKey(p.CategoryId) ? categories[p.CategoryId] : string.Empty;

                        data.Add(new
                        {
                            Article = p.Article,
                            Name = p.Name,
                            Category = catName,
                            Quantity = p.Quantity,
                            PurchasePrice = p.PurchasePrice,
                            Stock = p.Quantity
                        });
                    }

                    dgvProducts.DataSource = data;

                    dgvProducts.Columns["Article"].HeaderText = "Артикул";
                    dgvProducts.Columns["Name"].HeaderText = "Название";
                    dgvProducts.Columns["Category"].HeaderText = "Категория";
                    dgvProducts.Columns["Quantity"].HeaderText = "Количество";
                    dgvProducts.Columns["PurchasePrice"].HeaderText = "Цена закупки";
                    dgvProducts.Columns["Stock"].HeaderText = "Текущий остаток";
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(Resources.ErrorLoadProducts, ex);
                MessageBox.Show(Resources.ProductLoadError);
            }
        }
    }
}
