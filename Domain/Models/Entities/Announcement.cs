using Domain.Models.Common;
using Domain.Models.UserModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Entities
{
    [Table("announcements")]
    public class Announcement : BaseAuditableEntity
    {
        [Column("user_id")]
        public Guid UserId { get; set; }
        public User user { get; set; }

        [Column("phone_id")]
        public Guid PhoneId { get; set; }
        public Phone phone { get; set; }
    }
}
