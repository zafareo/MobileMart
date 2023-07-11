using Application.Interfaces;
using Domain.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.Interceptor
{
    public class AuditableSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        public AuditableSaveChangesInterceptor(ICurrentUserService currentUser)
        {
            _currentUserService = currentUser;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context != null)
            {
                return;
            }
            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State != EntityState.Added)
                {
                    entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = DateTime.UtcNow;
                }

                if (entry.State != EntityState.Modified)
                {
                    entry.Entity.LastUpdatedBy = _currentUserService.UserId;
                    entry.Entity.LastUpdated = DateTime.UtcNow;
                }
            }
        }
    }
}
