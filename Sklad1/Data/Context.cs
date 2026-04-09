using Microsoft.EntityFrameworkCore;
using Sklad1.Models;

namespace Sklad1.Data
{
    /// <summary>
    /// Контекст базы данных для работы с PostgreSQL
    /// </summary>
    public class Context : DbContext
    {
        /// <summary>
        /// Таблица пользователей
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Таблица категорий товаров
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Таблица товаров
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Таблица отгрузок
        /// </summary>
        public DbSet<Shipment> Shipments { get; set; }

        /// <summary>
        /// Таблица позиций отгрузок
        /// </summary>
        public DbSet<ShipmentItem> ShipmentItems { get; set; }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=Sklad_BD;Username=postgres;Password=milanaANDmadina");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>(); 
        }
    }
}
