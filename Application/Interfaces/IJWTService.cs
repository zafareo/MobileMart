using Domain.Models.Token;
using Domain.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IJWTService 
    {
        Task<Token> GenerateTokenAsync(User user);
        Task<string> GenerateRefreshTokenAsync(User user);

        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
