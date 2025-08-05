using Asp.Versioning;
using Azure.Core;
using Beauty4u.Business.Services;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Beauty4u.WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponse>> Login(LoginRequest request)
        {
            try
            {
                var userToken = await _authService.LoginAsync(request.Username, request.Password);
                if (userToken is null)
                {
                    return BadRequest(new { message = "Invalid username or password" });
                }
                return Ok(userToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal server error",
                    error = new
                    {
                        ex.Message,
                        ex.Source,
                        ex.StackTrace
                    }
                });
            }
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenResponse>> RefreshTokens(RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokensAsync(request);
            if (result is null || result.AccessToken is null || result.RefreshToken is null)
            {
                return Unauthorized("Invalid refresh token");
            }
            return Ok(result);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                var output = await _authService.ResetPasswordAsync(resetPasswordRequest);

                return Ok(new { message = output });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal server error",
                    error = new
                    {
                        ex.Message,
                        ex.Source,
                        ex.StackTrace
                    }
                });
            }
        }
    }
}
