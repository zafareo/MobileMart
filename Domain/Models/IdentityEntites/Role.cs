using Domain.Models.Common;
using Domain.Models.IdentityEntites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.UserModels
{
    [Table("roles")]
    public class Role : BaseAuditableEntity
    {

        [Column("name")]
        public string? Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<RolePermission>? RolePermissions { get; set; }

        [JsonIgnore]
        public virtual ICollection<UserRole>? UserRoles { get; set; }

        [NotMapped]
        public Guid[]? PermissionIds { get; set; }
    }
}

