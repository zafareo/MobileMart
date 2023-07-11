using Application.Interfaces;
using Domain.Models.IdentityEntites;
using Domain.Models.Token;
using Domain.Models.UserModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services
{
    public class JWTServices : IJWTService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        public JWTServices(IConfiguration configuration, IUserRepository userRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
        }
        public async Task<string> GenerateRefreshTokenAsync(User user)
        {
            string userJwt = await _userRepository.ComputeHashAsync(user.UserName + DateTime.UtcNow.ToString());
            return userJwt;
        }

        public async Task<Token> GenerateTokenAsync(User user)
        {
            var USER = await _userRepository.GetAsync(x => x.UserName == user.UserName);
            List<Claim> permissions = new()
        {
            new Claim(ClaimTypes.Name, user.UserName)
        };

            foreach (UserRole role in USER.UserRoles)
            {
                foreach (RolePermission rolePermission in role.Role.RolePermissions)
                {
                    permissions.Add(new Claim(ClaimTypes.Role, rolePermission.Permission.PermissionName));
                }
            }
            int min = 4;
            if (int.TryParse(_configuration["JWT:ExpiresInMinutes"], out int _min))
            {
                min = _min;
            }
            JwtSecurityToken jwtSecurityToken = new(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: permissions,
                expires: DateTime.UtcNow.AddMinutes(min),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                    SecurityAlgorithms.HmacSha256)
                );
            var res = new Token()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                RefreshToken = await GenerateRefreshTokenAsync(user)
            };
            return res;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"],
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }


            return principal;
        }
    }
}
