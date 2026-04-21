using NLog;
using Sklad1.Data;
using Sklad1.Helpers;
using Sklad1.Models;
using Sklad1.Properties;

namespace Sklad1.Forms
{
    /// <summary>
    /// Форма ручного ввода поставки
    /// </summary>
    public partial class FormSupply : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private List<SupplyItemTemp> _items = new List<SupplyItemTemp>();

        public FormSupply()
        {
            InitializeComponent();
            LoadProducts();
            LoadCurrencies();
            LoadUnits();
        }

        private void LoadProducts()
        {
            try
            {
                using (var bd = new Context())
                {
                    var products = bd.Products.ToList();
                    cmbName.DisplayMember = "Name";
                    cmbName.ValueMember = "Id";
                    cmbName.DataSource = products;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Resources.ErrorLoadProducts);
                MessageBox.Show(Resources.ErrorSystem);
            }
        }

        private void LoadCurrencies()
        {
            cmbCurrency.Items.Clear();
            cmbCurrency.Items.AddRange(new string[] { "RUB", "USD", "EUR", "CNY" });
            if (cmbCurrency.Items.Count > 0)
                cmbCurrency.SelectedIndex = 0;
        }

        private void LoadUnits()
        {
            cmbUnit.Items.Clear();
            cmbUnit.Items.AddRange(new string[] { "шт", "кг", "л", "уп", "м", "пач", "кор" });
            cmbUnit.SelectedIndex = 0;
        }

        private decimal ConvertToRub(decimal price, string currency)
        {
            try
            {
                using (var bd = new Context())
                {
                    var rate = bd.CurrencyRates.FirstOrDefault(c => c.Code == currency);
                    if (rate != null && rate.RateToRub > 0)
                    {
                        return price * rate.RateToRub;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(Resources.ErrorCurrencyConversion);
            }

            MessageBox.Show(Resources.ErrorSaveSupply);
            return price;
        }

        private bool ValidateFields()
        {
            if (cmbName.SelectedItem == null)
            {
                MessageBox.Show(Resources.SelectProduct);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show(Resources.EnterQuantity);
                txtQuantity.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show(Resources.EnterPrice);
                txtPrice.Focus();
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            txtQuantity.Clear();
            txtPrice.Clear();
            dtpExpiryDate.Value = DateTime.Now.AddMonths(6);
            cmbName.Focus();
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields())
                return;

            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show(Resources.InvalidQuantity);
                txtQuantity.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show(Resources.InvalidCost);
                txtPrice.Focus();
                return;
            }

            if (dtpExpiryDate.Value.Date < DateTime.Today)
            {
                MessageBox.Show(Resources.InvalidExpiryDate);
                return;
            }

            var selectedProduct = (Product)cmbName.SelectedItem;
            var selectedCurrency = cmbCurrency.SelectedItem?.ToString() ?? "RUB";

            if (selectedCurrency != "RUB")
            {
                price = ConvertToRub(price, selectedCurrency);
            }

            _items.Add(new SupplyItemTemp
            {
                ProductId = selectedProduct.Id,
                ProductName = selectedProduct.Name,
                Quantity = quantity,
                PurchasePrice = price,
                ExpiryDate = dtpExpiryDate.Value,
                Unit = cmbUnit.SelectedItem?.ToString() ?? Resources.DefaultUnit
            });

            MessageBox.Show(Resources.ProductAdded);
            ClearFields();
        }

        private async Task SaveSupply()
        {
            if (_items.Count == 0) return;

            try
            {
                using (var bd = new Context())
                {
                    using (var transaction = await bd.Database.BeginTransactionAsync())
                    {
                        var supply = new Supply
                        {
                            Id = Guid.NewGuid(),
                            UserId = CurrentUser.Id,
                            Supplier = Resources.ManualInput,
                            Date = DateTime.UtcNow,
                            Source = "manual"
                        };
                        bd.Supplies.Add(supply);
                        await bd.SaveChangesAsync();

                        foreach (var item in _items)
                        {
                            var product = await bd.Products.FindAsync(item.ProductId);
                            if (product == null) continue;

                            var expiryDateUtc = DateTime.SpecifyKind(item.ExpiryDate, DateTimeKind.Utc);

                            var batch = new ProductBatch
                            {
                                Id = Guid.NewGuid(),
                                ProductId = item.ProductId,
                                Quantity = item.Quantity,
                                PurchasePrice = item.PurchasePrice,
                                ExpiryDate = expiryDateUtc,
                                Status = "active"
                            };
                            bd.ProductBatches.Add(batch);

                            product.Quantity += item.Quantity;

                            var supplyItem = new SupplyItem
                            {
                                Id = Guid.NewGuid(),
                                SupplyId = supply.Id,
                                ProductId = item.ProductId,
                                BatchId = batch.Id,
                                Quantity = item.Quantity,
                                PurchasePrice = item.PurchasePrice
                            };
                            bd.SupplyItems.Add(supplyItem);
                        }

                        await bd.SaveChangesAsync();
                        await transaction.CommitAsync();

                        MessageBox.Show(Resources.SupplySaved);
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Resources.ErrorSaveSupply);
                MessageBox.Show(Resources.ErrorSaveSupply);
            }
        }

        private class SupplyItemTemp
        {
            public Guid ProductId { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal PurchasePrice { get; set; }
            public DateTime ExpiryDate { get; set; }
            public string Unit { get; set; }
        }

        private async void btnCancel_Click(object sender, EventArgs e)
        {
            if (_items.Count == 0)
            {
                Close();
                return;
            }

            var result = MessageBox.Show(string.Format(Resources.ConfirmSaveSupply, _items.Count),
                Resources.Confirmation, MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                await SaveSupply();
                DialogResult = DialogResult.OK;
                Close();
            }
            else if (result == DialogResult.No)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}