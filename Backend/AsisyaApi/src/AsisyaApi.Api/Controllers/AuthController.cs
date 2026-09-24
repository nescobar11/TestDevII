using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AsisyaApi.Application.DTOs.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AsisyaApi.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto dto)
        {
            var username = _config["User:Username"];
            var password = _config["User:Password"];

            if (dto.Username != username || dto.Password != password)
                return Unauthorized(new { message = "Credenciales inválidas." });

            var expires = DateTime.UtcNow.AddHours(2);
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username!),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new LoginResponseDto(new JwtSecurityTokenHandler().WriteToken(token), expires, username!));
        }
    }
}