﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO.User
{
    public class UserUpdateDTO : UserBaseDTO
    {
        public Guid UserId { get; set; }
    }
}
