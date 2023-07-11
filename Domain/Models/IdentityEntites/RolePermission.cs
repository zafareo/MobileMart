using Domain.Models.Common;
using Domain.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.IdentityEntites
{
    [Table("role_permission")]
    public class RolePermission : BaseAuditableEntity
    {
        [Column("role_id")]
        public Guid RoleId { get; set; }
        public virtual Role? Role { get; set; }

        [Column("permission_id")]
        public Guid PermissionId { get; set; }
        public Permission? Permission { get; set; }
    }
}
