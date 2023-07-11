using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO.RolePermission
{
    public abstract class RolePermissionBaseDTO
    {
        [JsonPropertyName("Role Id")]
        public Guid RoleId { get; set; }

        [JsonPropertyName("Permission Id")]
        public Guid PermissionId { get; set; }

    }
}
