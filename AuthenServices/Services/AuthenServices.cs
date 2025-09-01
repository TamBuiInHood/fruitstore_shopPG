using AuthenServices.Extensions;
using AuthenServices.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Configurations;
using Shared.DTOs.AuthenDTO;
using Shared.SeedWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenServices.Services
{
    public class AuthenServices : IAuthenServices
    {
        private readonly IConfiguration _configuration;
        private readonly TokenHelper _tokenHelper;
        private readonly JwtSettings _jwtSettings;

        public AuthenServices(IConfiguration configuration, JwtSettings jwtSettings, TokenHelper tokenHelper)
        {
            _configuration = configuration;
            _jwtSettings = jwtSettings;
            _tokenHelper = tokenHelper;
        }


        public async Task<ApiResult<TokenResponseDto>> LoginByEmailAndPassword(string username, string password)
        {
            try
            {
                var existUser = "";
                if (existUser == null)
                {
                    return new ApiErrorResult<TokenResponseDto>();
                }

                var verifyPassword = PasswordHelper.ConvertToEncrypt(password);

                string accessToken = GenerateAccessToken(username);
                string refreshToken = GenerateRefreshToken(username, null);

                return new ApiSuccessResult<TokenResponseDto>(new TokenResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public ApiResult<TokenResponseDto> RefreshAccessToken(string refreshToken, string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            try
            {
                // Validate refresh token
                tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = _jwtSettings.ValidateIssuer,
                    ValidIssuer = _jwtSettings.ValidIssuer,
                    ValidateAudience = _jwtSettings.ValidateAudience,
                    ValidAudience = _jwtSettings.ValidAudience,
                    ValidateLifetime = true, // check hạn của refresh token
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                // Nếu token còn hạn -> cấp lại access Token mới
                var claims = jwtToken.Claims.ToList();
                var newAccessToken = GenerateAccessToken(username); ;
                var newRefreshToken = GenerateRefreshToken(username, null); ;
                return new ApiSuccessResult<TokenResponseDto>(new TokenResponseDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken,
                });
            }
            catch (SecurityTokenExpiredException)
            {
                // Refresh Token hết hạn -> bắt buộc login lại
                throw new SecurityTokenException("Refresh token expired, please login again.");
            }
            catch (Exception)
            {
                throw new SecurityTokenException("Invalid refresh token.");
            }
        }

        private string GenerateAccessToken(string userName)
        {
            // TODO: check user from DB
            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, userName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var accessToken = _tokenHelper.CreateAccessToken(authClaims, DateTime.UtcNow);
            var result = new JwtSecurityTokenHandler().WriteToken(accessToken);
            return result;
        }

        private string GenerateRefreshToken(string username, DateTime? beginTimeRefreshToken)
        {
            var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var refreshToken = _tokenHelper.CreateRefreshToken(authClaims, DateTime.Now);

            var result = new JwtSecurityTokenHandler().WriteToken(refreshToken);
            return result;
        }
    }
}
