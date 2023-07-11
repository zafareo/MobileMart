using Application.Abstraction;
using Application.Interfaces;
using Domain.Models.Token;
using Domain.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserRefreshTokenRepository : IUserRefreshTokenRepository
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserRepository _userRepository;
        public UserRefreshTokenRepository(IApplicationDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        public async Task<UserRefreshToken> AddUserRefreshTokens(UserRefreshToken user)
        {
            _context.UserRefreshToken.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserRefreshTokens(string username, string refreshToken)
        {
            var token = _context.UserRefreshToken
                .FirstOrDefault(x => x.UserName == x.UserName && x.RefreshToken == refreshToken);
            if (token != null)
            {
                _context.UserRefreshToken.Remove(token);
                int res = await _context.SaveChangesAsync();
                return res > 0;
            }
            return false;
        }

        public async Task<UserRefreshToken> GetSavedRefreshTokens(string username, string refreshtoken)
        {
            UserRefreshToken? token = await _context.UserRefreshToken.FirstOrDefaultAsync(x => x.UserName == username && x.RefreshToken == refreshtoken);
            return token;
        }

        public async Task<bool> IsValidUserAsync(User user)
        {
            string HashedPsw = await _userRepository.ComputeHashAsync(user.Password);
            var user1 = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == user.UserName && x.Password == user.Password);
            if (user1 != null)
            {
                return true;
            }
            return false;
        }

        public async Task<int> SaveCommit()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<UserRefreshToken> UpdateUserRefreshToken(UserRefreshToken user)
        {
            var refreshToken = await _context.UserRefreshToken
                .FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if (refreshToken != null)
            {
                refreshToken.RefreshToken = user.RefreshToken;
                refreshToken.Expiretime = user.Expiretime;
                _context.UserRefreshToken.Update(refreshToken);
                await _context.SaveChangesAsync();
                return user;
            }
            else
            {
                await AddUserRefreshTokens(user);
                return user;
            }
        }
    }
}
