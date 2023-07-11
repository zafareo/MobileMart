using Application.Abstraction;
using Application.Interfaces;
using Domain.Models;
using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PhoneRepository : Repository<Phone> ,IPhoneRepository
    {
        public PhoneRepository(IApplicationDbContext context)
            : base(context)
        {
            
        }
    }
}
