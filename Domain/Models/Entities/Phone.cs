using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.Common;
using System.Text.Json.Serialization;

namespace Domain.Models.Entities
{
    [Table("phones")]
    public class Phone : BaseAuditableEntity
    {

        [Column("name")]
        public string Name { get; set; }

        [Column("model")]
        public int Model { get; set; }

        [Column("category_id")]
        public Guid SubCategoryId { get; set; }

        [JsonIgnore]
        public SubCategory? Subcategory { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("image")]
        public string Image { get; set; }

        [JsonIgnore]
        public virtual IList<OrderPhone> OrderPhones { get; set; }
    }
}
