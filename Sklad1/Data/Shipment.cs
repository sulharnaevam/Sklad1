using System.ComponentModel.DataAnnotations.Schema;

namespace Sklad1.Data
{
    /// <summary>
    /// Модель отгрузки 
    /// </summary>
    [Table("shipments")]
    public class Shipment
    {
        /// <summary>
        /// Идентификатор отгрузки 
        /// </summary>
        [Column("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор пользователя, совершившего отгрузку
        /// </summary>
        [Column("user_id")]
        public Guid UserId { get; set; }

        /// <summary>
        /// Клиент
        /// </summary>
        [Column("client")]
        public string Client { get; set; }

        /// <summary>
        /// Дата отгрузки
        /// </summary>
        [Column("date")]
        public DateTime Date { get; set; }
    }
}
