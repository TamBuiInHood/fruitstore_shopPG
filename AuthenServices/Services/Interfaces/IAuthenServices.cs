using Shared.DTOs.AuthenDTO;
using Shared.SeedWork;

namespace AuthenServices.Services.Interfaces
{
    public interface IAuthenServices
    {
        public Task<ApiResult<TokenResponseDto>> LoginByEmailAndPassword(string email, string password);
        public ApiResult<TokenResponseDto> RefreshAccessToken(string refreshToken, string username);

    }
}
