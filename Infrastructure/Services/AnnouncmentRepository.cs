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
    public class AnnouncmentRepository : Repository<Announcement>, IAnnouncementRepository
    {
        public AnnouncmentRepository(IApplicationDbContext context)
            : base(context)
        {

        }

    }
}
