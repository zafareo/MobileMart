using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Common
{
    public abstract class BaseAuditableEntity : BaseEntity 
    {
        public DateTime Created { get; set; } = DateTime.Now.ToUniversalTime();
        public string? CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }=DateTime.Now.ToUniversalTime();
        public string? LastUpdatedBy { get; set;}
    }
}
