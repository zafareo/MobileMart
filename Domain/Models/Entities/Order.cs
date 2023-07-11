using Domain.Models.Common;
using Domain.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    [Table("orders")]
    public class Order : BaseAuditableEntity
    {
        [Column("user_id")]
        public Guid UserId { get; set; }
        public User User { get; set; }
        public virtual IList<OrderPhone> OrderPhones { get; set; }
    }
}
