using Microsoft.EntityFrameworkCore;
using NLog;
using Sklad1.Data;
using Sklad1.Helpers;
using Sklad1.Models;
using Sklad1.Properties;

namespace Sklad1.Forms
{
    /// <summary>
    /// Форма контроля сроков годности
    /// </summary>
    public partial class FormExpiryDates : Form
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public FormExpiryDates()
        {
            InitializeComponent();
            LoadBatches();
        }

        private void LoadBatches()
        {
            try
            {
                using (var bd = new Context())
                {
                    var data = bd.ProductBatches
                        .Include(b => b.Product)
                        .Where(b => b.Quantity > 0 && b.Product != null)
                        .Select(b => new
                        {
                            Article = b.Product.Article ?? Resources.NoArticle,
                            ProductName = b.Product.Name ?? Resources.UnknownProduct,
                            b.Quantity,
                            ExpiryDate = b.ExpiryDate.ToString("dd.MM.yyyy"),
                            DaysLeft = (b.ExpiryDate - DateTime.UtcNow.Date).Days,
                            b.PurchasePrice,
                            b.Status
                        }).ToList();

                    dgvExpDates.DataSource = data;

                    dgvExpDates.Columns["Article"].HeaderText = Resources.Article;
                    dgvExpDates.Columns["ProductName"].HeaderText = Resources.ProductName;
                    dgvExpDates.Columns["Quantity"].HeaderText = Resources.Quantity;
                    dgvExpDates.Columns["ExpiryDate"].HeaderText = Resources.ExpiryDate;
                    dgvExpDates.Columns["DaysLeft"].HeaderText = Resources.DaysLeft;
                    dgvExpDates.Columns["PurchasePrice"].HeaderText = Resources.PurchasePrice;
                    dgvExpDates.Columns["Status"].HeaderText = Resources.Status;
                    dgvExpDates.Columns["ExpiryDate"].DefaultCellStyle.Format = "dd.MM.yyyy";

                    foreach (DataGridViewRow row in dgvExpDates.Rows)
                    {
                        if (row.Cells["DaysLeft"].Value == null) continue;
                        int daysLeft = Convert.ToInt32(row.Cells["DaysLeft"].Value);
                        if (daysLeft < 0) row.DefaultCellStyle.BackColor = Color.LightCoral;
                        else if (daysLeft <= 3) row.DefaultCellStyle.BackColor = Color.Orange;
                        else if (daysLeft <= 7) row.DefaultCellStyle.BackColor = Color.LightYellow;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Resources.ErrorLoadBatches);
                MessageBox.Show(Resources.ErrorSystem);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            LoadBatches();
        }

        private async void btnWriteOff_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(Resources.ConfirmWriteOff, Resources.Confirmation, MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            btnWriteOff.Enabled = false;
            btnWriteOff.Text = Resources.WritingOff;

            try
            {
                var count = await ExpiryService.WriteOffExpiredBatches();
                MessageBox.Show(string.Format(Resources.WriteOffComplete, count));
                LoadBatches();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, Resources.ErrorWriteOff);
                MessageBox.Show(Resources.ErrorSystem);
            }
            finally
            {
                btnWriteOff.Enabled = true;
                btnWriteOff.Text = Resources.WriteOff;
            }
        }
    }
}
