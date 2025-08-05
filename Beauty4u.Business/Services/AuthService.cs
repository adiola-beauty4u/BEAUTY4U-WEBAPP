using AutoMapper;
using Azure.Core;
using Beauty4u.Common.Utilities;
using Beauty4u.Interfaces.Api.Auth;
using Beauty4u.Interfaces.DataAccess.B4u;
using Beauty4u.Interfaces.Dto.Users;
using Beauty4u.Interfaces.Services;
using Beauty4u.Models.Api.Auth;
using Beauty4u.Models.Dto.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Beauty4u.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;
        private readonly ISystemSetupService _systemSetupService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public AuthService(IUserRepository userRepository, IConfiguration configuration, IMapper mapper, TokenService tokenService, ISystemSetupService systemSetupService, ICurrentUserService currentUserService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;
            _tokenService = tokenService;
            _systemSetupService = systemSetupService;
            _currentUserService = currentUserService;
        }

        public async Task<ITokenResponse?> LoginAsync(string username, string password)
        {
            var user = await _userRepository.GetUserLoginByUsernameAsync(username);

            if (user is null)
            {
                return null;
            }

            if (!EncryptString.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return await CreateUserTokenResponse(user);
        }

        private async Task<ITokenResponse> CreateUserTokenResponse(IUserDto user)
        {
            return await CreateToken(user);
        }

        public async Task<ITokenResponse> CreateToken(IUserDto user)
        {
            var store = await _systemSetupService.GetSystemSetupAsync();
            var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, $"{user.Firstname} {user.Lastname}"),
                        new Claim("UserCode", user.UserCode),
                        new Claim("UserName", user.Firstname + " " + user.Lastname),
                        new Claim("Role", user.Position),
                        new Claim("StoreCode", store["B"].Value),
                        new Claim("StoreName", store["B"].AltValue),
                        new Claim("StoreAbbr", store["B"].Description),
                        new Claim(ClaimTypes.Role, "Admin") // adjust role as needed
                    };

            string accessToken = _tokenService.GenerateAccessToken(claims);
            string refreshToken = _tokenService.GenerateRefreshToken();

            // Save refresh token to DB
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _userRepository.UserUpdateRefreshTokenAsync(user);

            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<string> ResetPasswordAsync(IResetPasswordRequest resetPasswordRequest)
        {
            var currentUser = _currentUserService.GetCurrentUser();
            if (currentUser != null)
            {
                var user = await _userRepository.GetUserLoginByUsercodeAsync(currentUser?.Claims["UserCode"]!);

                if (!EncryptString.Verify(resetPasswordRequest.CurrentPassword, user.PasswordHash))
                {
                    return "Current password mismatch";
                }

                var hashedPassword = EncryptString.Hash(resetPasswordRequest.NewPassword);

                await _userRepository.UpdateUserPasswordHashAsync(currentUser?.Claims["UserCode"]!, hashedPassword);
                return "Password changed successfully!";
            }
            return "Invalid user";
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }

        public async Task<ITokenResponse?> RefreshTokensAsync(IRefreshTokenRequest refreshTokenRequest)
        {
            var user = await ValidateRefreshTokenAsync(refreshTokenRequest);
            if (user is null)
            {
                return null;
            }
            return await CreateUserTokenResponse(user);
        }

        private async Task<IUserDto> ValidateRefreshTokenAsync(IRefreshTokenRequest refreshTokenRequest)
        {
            var user = await _userRepository.GetUserLoginByUsercodeAsync(refreshTokenRequest.UserCode);

            if (user is null || user.RefreshToken != refreshTokenRequest.RefreshToken || user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                return null;
            }

            return user;
        }
    }
}
