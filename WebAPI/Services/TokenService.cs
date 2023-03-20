using HelperClasses.DTOs;
using HelperClasses.DTOs.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Interfaces.IServices;
using WebAPI.Models;

namespace WebAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly IConfiguration _config;
        private readonly FougitoContext _context;
        private readonly UserManager<AppUser> _userManager;

        public TokenService(ILogger<TokenService> logger, IConfiguration config,
            FougitoContext context, UserManager<AppUser> userManager)
        {
            _config = config;
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public async Task<AccessToken> GenerateAccessToken(string UserId, string DeviceTrackId = null)
        {
            string DeviceTrackIdString;
            if (string.IsNullOrEmpty(DeviceTrackId))
            {
                Guid obj = Guid.NewGuid();
                DeviceTrackIdString = obj.ToString();
            }
            else
            {
                DeviceTrackIdString = DeviceTrackId;
            }

            try
            {
                AppUser User = await _userManager.FindByIdAsync(UserId);
                List<Claim> Claims = (await _userManager.GetClaimsAsync(User)).ToList();

                String AccessToken = GenerateJwtToken(User, DeviceTrackIdString, Claims, out DateTime ExpiryDateTime);
                String RefreshToken = GenerateRefreshToken();

                await UpdateRefreshToken(User.Id, DeviceTrackIdString, RefreshToken);
                _logger.LogInformation(string.Format("Refresh token for UserID {0} is {1}", User.Id, RefreshToken));

                return new AccessToken(AccessToken, RefreshToken, ExpiryDateTime, _config["JWT:ValidIssuer"]);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<AccessToken> RefreshAccessToken(AccessToken Token)
        {
            ClaimsPrincipal Principal = GetPrincipalFromExpiredToken(Token.Token);

            //Get User ID from claims
            Claim Name = Principal.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);
            Claim DeviceTrackId = Principal.Claims.First(c => c.Type == "DeviceTrackId");

            String UserID = Name.Value.ToString();

            AppUser user = await _userManager.FindByIdAsync(UserID);

            if(!user.IsActive)
                throw new SecurityTokenException("Account Suspended!");

            if (user.IsDeleted)
                throw new SecurityTokenException("No User Found!");

            //retrieve the refresh token from a data store
            String SavedRefreshToken = await GetRefreshTokenFromDB(UserID, DeviceTrackId.Value.ToString());

            if (SavedRefreshToken != Token.RefreshToken)
                throw new SecurityTokenException("Invalid refresh token");


            return await GenerateAccessToken(UserID, DeviceTrackId.Value.ToString());
        }

        private async Task UpdateRefreshToken(String UserId, String DeviceTrackId, String NewRefreshToken)
        {
            UserRefreshToken UserToken = await _context.UserRefreshTokens.Where(x => x.UserId == UserId && x.DeviceTrackId == DeviceTrackId).FirstOrDefaultAsync();

            if (UserToken == default)
                await InsertRefreshToken(UserId, DeviceTrackId, NewRefreshToken);
            else
            {
                UserToken.RefreshToken = NewRefreshToken;
                UserToken.TokenDate = DateTime.UtcNow;

                _context.Entry(UserToken).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
        }

        private async Task<string> GetRefreshTokenFromDB(String UserId, String DeviceTackId)
        {
            UserRefreshToken UserToken = await _context.UserRefreshTokens.Where(x => x.UserId == UserId && x.DeviceTrackId == DeviceTackId).FirstOrDefaultAsync();

            if (UserToken != default)
                return UserToken.RefreshToken;
            else
                return "";
        }

        private async Task InsertRefreshToken(String UserId, String DeviceTrackId, String NewRefreshToken)
        {
            UserRefreshToken UserToken = new UserRefreshToken
            {
                UserId = UserId,
                DeviceTrackId = DeviceTrackId,
                RefreshToken = NewRefreshToken,
                TokenDate = DateTime.UtcNow
            };

            await _context.UserRefreshTokens.AddAsync(UserToken);
            await _context.SaveChangesAsync();
        }

        private String GenerateJwtToken(AppUser user, String DeviceTrackId, IList<Claim> Claims, out DateTime ExpiryDateTime)
        {
            Claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            Claims.Add(new Claim(ClaimTypes.Sid, user.Id));
            Claims.Add(new Claim(ClaimTypes.Role, _userManager.GetRolesAsync(user).Result.FirstOrDefault()));
            Claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.UserName));
            Claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            Claims.Add(new Claim("DeviceTrackId", DeviceTrackId));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            ExpiryDateTime = DateTime.UtcNow.AddDays(Convert.ToDouble(_config["JWT:JwtExpireDays"]));

            var token = new JwtSecurityToken(_config["JWT:ValidIssuer"], _config["JWT:ValidAudience"],
                                              Claims, expires: ExpiryDateTime, signingCredentials: creds);

            _logger.LogInformation("Token generated successfully for [" + user.Email + "]");

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(int Size = 32)
        {
            var randomNumber = new byte[Size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _config["JWT:ValidIssuer"],
                ValidAudience = _config["JWT:ValidAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                                                _config["JWT:Secret"])),
                ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                    StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
