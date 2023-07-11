using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Application.DTO.Phone
{
    public abstract class PhoneBaseDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        [JsonPropertyName("Phone name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Model is required")]
        [JsonPropertyName("Phone model")]
        public int Model { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(255, ErrorMessage = "Description cannot exceed 255 characters")]
        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a non-negative value")]
        [JsonPropertyName("Price")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Image URL is required")]
        [RegularExpression(@"^(http(s)?:\/\/)?[^\s\.]+\.[^\s]{2,}\.(jpg|png)$",
            ErrorMessage = "Invalid URL format or unsupported image format. Supported formats: JPG, PNG")]
        [JsonPropertyName("Picture")]
        public string Image { get; set; }
    }
}
