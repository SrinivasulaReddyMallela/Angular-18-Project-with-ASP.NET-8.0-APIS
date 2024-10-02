using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options; 
using Microsoft.IdentityModel.Tokens;
using WebGYM.API.Models;
using WebGYM.API.Common;
using WebGYM.Interface;
using WebGYM.Models;
using WebGYM.ViewModels;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

namespace WebGYM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IUsers _users;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticateController> _logger;
        public AuthenticateController(IOptions<AppSettings> appSettings, IUsers users, IConfiguration configuration, ILogger<AuthenticateController> logger)
        {
            _users = users;
            _appSettings = appSettings.Value;
            _configuration = configuration;
            _logger = logger;
        }
        [HttpPost]
        [Route("PostSAMLResponse")]
        //[ActionName("PostSAMLResponse")]
        public async Task<IActionResult> Post([FromBody] PostSAMLResponseModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    var samlresp = new Response(_appSettings.X509Certificate);
                    samlresp.LoadXml(Encoding.UTF8.GetString(Convert.FromBase64String(value.PostSAMLResponse)));
                    if (samlresp.IsValid())
                    {
                        var userdetails = await _users.GetUserDetailsbyCredentials(samlresp.GetCustomAttribute("Email"));
                        value.UserName = userdetails.UserName;
                        if (userdetails != null)
                        {
                            var authClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, userdetails.UserId.ToString()),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            };
                            authClaims.Add(new Claim(ClaimTypes.Role, userdetails.RoleId.ToString()));

                            var token = CreateToken(authClaims);
                            var refreshToken = GenerateRefreshToken();

                            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                            Users users = new Users();
                            users.UserId = userdetails.UserId;
                            users.RefreshToken = refreshToken;
                            users.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
                            await _users.SaveTokenbyUser(users);
                            return Ok(new
                            {
                                Token = new JwtSecurityTokenHandler().WriteToken(token),
                                RefreshToken = refreshToken,
                                Expiration = token.ValidTo,
                                Usertype = userdetails.RoleId,
                                UserName = userdetails.UserName
                            });
                        }
                        else
                            return StatusCode(418);
                    }
                    return Unauthorized();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }

        // POST: api/Authenticate
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] LoginRequestViewModel value)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var loginstatus = await _users.AuthenticateUsers(value.UserName, EncryptionLibrary.EncryptText(value.Password));

                    if (loginstatus)
                    {
                        var userdetails = await _users.GetUserDetailsbyCredentials(value.UserName);

                        if (userdetails != null)
                        {
                            #region Commented Code
                            //var tokenHandler = new JwtSecurityTokenHandler();
                            //var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                            //var tokenDescriptor = new SecurityTokenDescriptor
                            //{
                            //    Subject = new ClaimsIdentity(new Claim[]
                            //    {
                            //            new Claim(ClaimTypes.Name, userdetails.UserId.ToString())
                            //    }),
                            //    Expires = DateTime.UtcNow.AddDays(1),
                            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                            //        SecurityAlgorithms.HmacSha256Signature)
                            //};
                            //var token = tokenHandler.CreateToken(tokenDescriptor);
                            //value.Token = tokenHandler.WriteToken(token);

                            //// remove password before returning
                            //value.Password = null;
                            //value.Usertype = userdetails.RoleId;

                            //return Ok(value);
                            #endregion
                            var authClaims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, userdetails.UserId.ToString()),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            };
                            authClaims.Add(new Claim(ClaimTypes.Role, userdetails.RoleId.ToString()));

                            var token = CreateToken(authClaims);
                            var refreshToken = GenerateRefreshToken();

                            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);
                            Users users = new Users();
                            users.UserId = userdetails.UserId;
                            users.RefreshToken = refreshToken;
                            users.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
                            await _users.SaveTokenbyUser(users);
                            return Ok(new
                            {
                                Token = new JwtSecurityTokenHandler().WriteToken(token),
                                RefreshToken = refreshToken,
                                Expiration = token.ValidTo,
                                Usertype = userdetails.RoleId
                            });
                        }
                        else
                            return StatusCode(418);
                    }
                    return Unauthorized();
                }
                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }
        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel)
        {
            try
            {
                if (tokenModel is null)
                    return BadRequest("Invalid client request");
                string? accessToken = tokenModel.AccessToken;
                string? refreshToken = tokenModel.RefreshToken;
                var principal = GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                    return BadRequest("Invalid access token or refresh token");
                var user = await _users.GetUsersbyId(Convert.ToInt32(principal.Identity.Name));
                if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                    return BadRequest("Invalid access token or refresh token");
                var newAccessToken = CreateToken(principal.Claims.ToList());
                var newRefreshToken = GenerateRefreshToken();
                Users users = new Users();
                users.UserId = user.UserId;
                users.RefreshToken = newRefreshToken;
                await _users.SaveTokenbyUser(users);
                return new ObjectResult(new
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                    refreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "This is an Error log, indicating a failure in the current operation.");
                return StatusCode(500, new { message = "An error occurred while creating the  crating Todo Item", error = ex.Message });
            }
        }
        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            //_ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int tokenValidityInMinutes);
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }

    }
}
