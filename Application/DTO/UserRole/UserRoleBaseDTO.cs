using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO.UserRole
{
    public abstract class UserRoleBaseDTO
    {
        [JsonPropertyName("User Id")]
        public Guid UserId { get; set; }

        [JsonPropertyName("Role Id")]
        public Guid RoleId { get; set; }
    }
}
