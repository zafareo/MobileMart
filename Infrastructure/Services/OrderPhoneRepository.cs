using Application.Abstraction;
using Application.Interfaces;
using Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class OrderPhoneRepository : Repository<OrderPhone>, IOrderPhoneRepository
    {
        public OrderPhoneRepository(IApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
