using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenServices.Extensions
{
    public class TokenHelper
    {
        private readonly JwtSettings _jwtSettings;

        public TokenHelper(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public JwtSecurityToken CreateAccessToken(List<Claim> authClaims, DateTime currentTime)
        {
            Console.WriteLine(_jwtSettings.Key);
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                claims: authClaims,
                expires: currentTime.AddMinutes(_jwtSettings.TokenValidityInMinutes),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }

        public JwtSecurityToken CreateRefreshToken(List<Claim> authClaims, DateTime currentTime)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.ValidIssuer,
                audience: _jwtSettings.ValidAudience,
                claims: authClaims,
                expires: currentTime.AddDays(_jwtSettings.RefreshTokenValidityInDays),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}

