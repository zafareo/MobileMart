using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Models.Token
{
    public class UserCredential
    {
        [JsonPropertyName("user_name")]
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        [PasswordPropertyText]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; }
    }
}

