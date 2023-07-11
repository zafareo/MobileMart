using Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    [Table("subcategories")]
    public class SubCategory : BaseAuditableEntity  
    {
        [Column("subcategory_name")]
        public string SubCategoryName { get; set; }
        public ICollection<Phone> Phones { get; set; }
    }
}
