using Sklad1.Data;
using Sklad1.Helpers;
using Sklad1.Models;
using Sklad1.Properties;

namespace Sklad1.Forms
{
    public partial class FormShipment : Form
    {
        private List<ShipmentItemTemp> _items = new List<ShipmentItemTemp>();

        public FormShipment()
        {
            InitializeComponent();

            LoadProducts();

            btnAdd.Click += BtnAdd_Click;
            btnShip.Click += BtnShip_Click;
            btnCancel.Click += btnCancel_Click;
            cmbProduct.SelectedIndexChanged += cmbProduct_SelectedIndexChanged;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void LoadProducts()
        {
            using (var bd = new Context())
            {
                var products = bd.Products.Where(p => p.Quantity > 0).Select(p => new ProductItem
                {
                    Article = p.Article,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    PurchasePrice = p.PurchasePrice
                })
                    .ToList();

                cmbProduct.DisplayMember = nameof(ProductItem.Name);
                cmbProduct.ValueMember = nameof(ProductItem.Article);
                cmbProduct.DataSource = products;
            }
        }

        private void UpdateQuantityDropdown()
        {
            cmbQuantity.Items.Clear();
            if (cmbProduct.SelectedItem == null) return;

            var selectedProduct = (ProductItem)cmbProduct.SelectedItem;
            int maxQuantity = selectedProduct.Quantity;

            int maxItemsToShow = Math.Min(maxQuantity, 20);
            for (int i = 1; i <= maxItemsToShow; i++)
            {
                cmbQuantity.Items.Add(i);
            }

            if (maxQuantity > 20)
            {
                cmbQuantity.Items.Add($"Другое до {maxQuantity}");
            }
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateQuantityDropdown();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateClient())
                return;

            if (!ValidateSelection())
                return;

            if (!int.TryParse(cmbQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show(Resources.InvalidQuantity);
                return;
            }

            var selected = (ProductItem)cmbProduct.SelectedItem;

            if (quantity > selected.Quantity)
            {
                MessageBox.Show(Resources.InsufficientStock);
                return;
            }

            AddOrUpdateItem(selected, quantity);
            UpdateGrid();
            cmbQuantity.Text = "";
        }

        private bool ValidateSelection()
        {
            if (cmbProduct.SelectedItem == null)
            {
                MessageBox.Show(Resources.SelectProduct);
                return false;
            }
            return true;
        }

        private void AddOrUpdateItem(ProductItem selected, int quantity)
        {
            var currentClient = txtClient.Text.Trim();

            var existing = _items.FirstOrDefault(i => i.Article == selected.Article && i.Client == currentClient);

            if (existing != null)
            { 
                existing.Quantity += quantity;
            }

            else
            {
                _items.Add(new ShipmentItemTemp
                {
                    Article = selected.Article,
                    Name = selected.Name,
                    Quantity = quantity,
                    Price = selected.PurchasePrice,
                    Client = currentClient
                });
            }
        }

        private void UpdateGrid()
        {
            dgvItems.DataSource = null;
            dgvItems.DataSource = _items;

            dgvItems.Columns[nameof(ShipmentItemTemp.Article)].HeaderText = Resources.Article;
            dgvItems.Columns[nameof(ShipmentItemTemp.Name)].HeaderText = Resources.Name;
            dgvItems.Columns[nameof(ShipmentItemTemp.Quantity)].HeaderText = Resources.Quantity;
            dgvItems.Columns[nameof(ShipmentItemTemp.Price)].HeaderText = Resources.Price;
            dgvItems.Columns[nameof(ShipmentItemTemp.Client)].HeaderText = Resources.Client;

            btnShip.Enabled = _items.Count > 0;
        }

        private void BtnShip_Click(object sender, EventArgs e)
        { 
            if (!ValidateItems())
                return;

            foreach (var group in _items.GroupBy(i => i.Client))
            {
                var itemsForShipment = group.Select(i => (i.Article, i.Quantity)).ToList();

                if (!ShipmentService.ProcessShipment(group.Key, itemsForShipment))
                {
                    MessageBox.Show(Resources.ShipmentError);
                    return;
                }
            }

            MessageBox.Show(Resources.ShipmentSuccess);
            DialogResult = DialogResult.OK;
            Close();
        }

        private bool ValidateClient()
        {
            if (string.IsNullOrWhiteSpace(txtClient.Text))
            {
                MessageBox.Show(Resources.ShipmentNoClient);
                txtClient.Focus();
                return false;
            }

            var client = txtClient.Text.Trim();

            if (!System.Text.RegularExpressions.Regex.IsMatch(client, @"^[а-яА-ЯёЁa-zA-Z0-9\s\-\.]+$"))
            {
                MessageBox.Show(Resources.InvalidClientName);
                txtClient.Focus();
                return false;
            }

            return true;
        }

        private bool ValidateItems()
        {
            if (_items.Count == 0)
            {
                MessageBox.Show(Resources.ShipmentNoItems);
                return false;
            }
            return true;
        }
    }
}
