using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Models.Common;
using Domain.Models.Entities;

namespace Domain.Models.UserModels
{
    [Table("users")]
    public class User : BaseAuditableEntity
    {
        
        [Column("username")]
        public string UserName { get; set; }
        
        [Column("password")]
        public string Password { get; set; }

        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserRole>? UserRoles { get; set; }

        [JsonIgnore]
        public IList<Order>? Orders { get; set; }

    }
}
