using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Token
{
    [Table("user_refresh_tokens")]
    public class UserRefreshToken
    {
        [Column("refresh_token_id")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? RefreshToken { get; set; }
        public bool IsActive { get; set; }
        public DateTime Expiretime { get; set; }
    }
}
