using Application.DTO.Permission;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO.Role
{
    public abstract class RoleBaseDTO
    {
        [Required(ErrorMessage = "Role name is required")]
        [StringLength(50, ErrorMessage = "Role name must be between 1 and 50 characters", MinimumLength = 1)]
        [JsonPropertyName("Role name")]
        public string Name { get; set; }

        [JsonPropertyName("permission Id")]
        public Guid[] Permissions { get; set; }
    }
}
