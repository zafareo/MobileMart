using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Models.Common;

namespace Domain.Models.UserModels
{
    [Table("user_roles")]
    public class UserRole : BaseAuditableEntity
    {

        [ForeignKey("user_id")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }

        [ForeignKey("role_id")]
        public Guid RoleId { get; set; }

        [JsonIgnore]
        public virtual Role? Role { get; set; }
    }
}
