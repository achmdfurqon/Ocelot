using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UsersAPI.Models;

namespace UsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        List<User> userList = new List<User>();
        private IOptions<Audience> _settings;
        string token;

        public UsersController(IOptions<Audience> settings)
        {
            _settings = settings;
            var user1 = new User("16151", "Achmad Furqon");
            var user2 = new User("16160", "Marzota Dwi R.");
            userList = new List<User>() { user1, user2 };
        }

        [HttpGet]
        public IActionResult Get(string name, string pwd)
        {
            //just hard code here.  
            if (name == "admin" && pwd == "123")
            {
                var now = DateTime.UtcNow;

                var claims = new Claim[]
                {
            new Claim(JwtRegisteredClaimNames.Sub, name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
                };

                var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.Value.Secret));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateIssuer = true,
                    ValidIssuer = _settings.Value.Iss,
                    ValidateAudience = true,
                    ValidAudience = _settings.Value.Aud,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,

                };

                var jwt = new JwtSecurityToken(
                    issuer: _settings.Value.Iss,
                    audience: _settings.Value.Aud,
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                );
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                var responseJson = new
                {
                    access_token = encodedJwt,
                    expires_in = (int)TimeSpan.FromMinutes(2).TotalSeconds
                };
               
                HttpContext.Session.SetString("Token", "Bearer " + encodedJwt);
                return Ok(responseJson);
            }
            else
            {
                token = HttpContext.Session.GetString("Token");
                if (name == null && pwd == null && token != null)
                {                    
                    return Ok(token);
                }
                else if (name != null || pwd != null)
                {
                    return Unauthorized("Login Gagal");
                }
                return Unauthorized("Anda belum Login");
            }
        }

        [HttpGet("data")]
        public IActionResult GetUsers()
        {
            return Ok(userList);
        }

        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            var user = userList.Where(p => p.NIK == id);
            return Ok(user);
        }
    }
}
