using Domain.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTO.User
{
    public abstract class UserBaseDTO
    {

        [Required(ErrorMessage = "User name is required")]
        [StringLength(50, ErrorMessage = "User name must be between 1 and 50 characters", MinimumLength = 1)]
        [JsonPropertyName("Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        [JsonPropertyName("Password")]
        public string Password { get; set; }

        [JsonPropertyName("Phone number")]
        public string PhoneNumber { get; set; }
    }
}
