using Application.Abstraction;
using Application.Interfaces;
using Domain.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly IApplicationDbContext _context;
        public UserRepository(IApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

            public Task<string> ComputeHashAsync(string input)
            {
                using SHA256 sha256 = SHA256.Create();
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder builder = new();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }

                return Task.FromResult(builder.ToString());
            }

        public override async Task<IQueryable<User>> GetAllAsync(Expression<Func<User, bool>>? expression = null)
        {
            return expression == null ? await Task.FromResult(_context.Users.AsQueryable()) :
                 await Task.FromResult(_context.Users.Where(expression));
        }

        public override async Task<User> CreateAsync(User user)
        {
            user.Password = await ComputeHashAsync(user.Password);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public override async Task<User?> GetAsync(Expression<Func<User, bool>> expression)
        {
            User? user = await _context.Users.Where(expression)
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .ThenInclude(x => x.RolePermissions)
                .ThenInclude(x => x.Permission)
                .Select(x => x).FirstOrDefaultAsync();
            return user;
        }
    }
}
