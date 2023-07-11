using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.Phone
{
    public class PhoneUpdateDTO : PhoneBaseDTO
    {
        public Guid PhoneId { get; set; }
    }
}
