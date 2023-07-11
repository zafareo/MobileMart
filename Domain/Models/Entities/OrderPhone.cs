using Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    [Table("orders_phones")]
    public class OrderPhone : BaseAuditableEntity
    {
        [Column("order_id")]
        public Guid OrderId { get; set; }
        public Order Order { get; set; }

        [Column("product_id")]
        public Guid PhoneId { get; set; }
        public Phone PHONE { get; set; }
    }
}
