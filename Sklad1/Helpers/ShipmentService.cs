using Microsoft.EntityFrameworkCore;
using Serilog;
using Sklad1.Data;
using Sklad1.Models;
using Sklad1.Properties;

namespace Sklad1.Helpers
{
    public static class ShipmentService
    {
        public static bool ProcessShipment(string client, List<(string Article, int Quantity)> items)
        {
            if (string.IsNullOrWhiteSpace(client) || items == null || items.Count == 0)
            {
                return false;
            }

            using (var bd = new Context())
            {
                using (var transaction = bd.Database.BeginTransaction())
                {
                    try
                    {
                        var shipment = new Shipment
                        {
                            Id = Guid.NewGuid(),
                            UserId = CurrentUser.Id,
                            Client = client.Trim(),
                            Date = DateTime.UtcNow
                        };
                        bd.Shipments.Add(shipment);
                        bd.SaveChanges();

                        foreach (var item in items)
                        {
                            var product = bd.Products.FirstOrDefault(p => p.Article == item.Article);

                            if (product == null || product.Quantity < item.Quantity)
                            {
                                transaction.Rollback();
                                return false;
                            }
                            var availableBatches = bd.ProductBatches
                                .Where(b => b.ProductId == product.Id && b.Status == "active" && b.Quantity > 0 && b.ExpiryDate >= DateTime.UtcNow.Date)
                                .OrderBy(b => b.ExpiryDate) 
                                .ToList();

                            int totalAvailable = availableBatches.Sum(b => b.Quantity);
                            if (totalAvailable < item.Quantity)
                            {
                                transaction.Rollback();
                                MessageBox.Show(Resources.InsufficientGoods);
                                return false;
                            }

                            int remainingToShip = item.Quantity;

                            foreach (var batch in availableBatches)
                            {
                                if (remainingToShip <= 0) break;

                                int shipFromBatch = Math.Min(remainingToShip, batch.Quantity);

                                var shipmentItem = new ShipmentItem
                                {
                                    Id = Guid.NewGuid(),
                                    ShipmentId = shipment.Id,
                                    ProductId = product.Id,
                                    Quantity = shipFromBatch,
                                    PriceAtShipment = product.PurchasePrice,
                                    CostAtShipment = batch.PurchasePrice
                                };
                                bd.ShipmentItems.Add(shipmentItem);

                                batch.Quantity -= shipFromBatch;
                                product.Quantity -= shipFromBatch;
                                remainingToShip -= shipFromBatch;
                            }
                        }
                        bd.SaveChanges();
                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}
