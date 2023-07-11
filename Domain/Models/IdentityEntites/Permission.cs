using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Models.Common;
using Domain.Models.IdentityEntites;

namespace Domain.Models.UserModels
{
    [Table("permissions")]
    public class Permission : BaseAuditableEntity
    {

        [Column("permission_name")]
        public string? PermissionName { get; set; }

        [JsonIgnore]
        public virtual ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
