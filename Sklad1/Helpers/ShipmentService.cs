using Sklad1.Data;
using Serilog;
using Sklad1.Models;

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

                            var shipmentItem = new ShipmentItem
                            {
                                Id = Guid.NewGuid(),
                                ShipmentId = shipment.Id,
                                ProductId = product.Id,
                                Quantity = item.Quantity,
                                PriceAtShipment = product.PurchasePrice,
                            };
                            bd.ShipmentItems.Add(shipmentItem);

                            product.Quantity -= item.Quantity;
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
