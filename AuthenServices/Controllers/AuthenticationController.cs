using AuthenServices.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.AuthenDTO;

namespace AuthenServices.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenServices _authenServices;

        public AuthenticationController(IAuthenServices authenServices)
        {
            _authenServices = authenServices;
        }

        [HttpPost("login", Name = "LoginWithEmailAndPassword")]
        public async Task<IActionResult> LoginWithEmailAndPassword([FromBody] LoginDto accountRequestModel)
        {

                var result = await _authenServices.LoginByEmailAndPassword(accountRequestModel.Email, accountRequestModel.Password);
                return Ok(result);
        }

    }
}
