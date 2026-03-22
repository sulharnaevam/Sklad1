using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Database=Sklad_BD;Username=postgres;Password=admin123");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>(); 
        }
    }
}
