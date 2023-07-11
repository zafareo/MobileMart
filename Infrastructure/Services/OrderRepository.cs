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
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(IApplicationDbContext context)
            : base(context)
        {
        }
    }
}
