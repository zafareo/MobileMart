using Domain.Models.Token;
using Domain.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRefreshTokenRepository
    {
        Task<bool> IsValidUserAsync(User user);
        Task<UserRefreshToken> AddUserRefreshTokens(UserRefreshToken user);

        Task<UserRefreshToken> GetSavedRefreshTokens(string username, string refreshtoken);

        Task<bool> DeleteUserRefreshTokens(string username, string refreshToken);

        Task<int> SaveCommit();

        Task<UserRefreshToken> UpdateUserRefreshToken(UserRefreshToken user);
    }
}
