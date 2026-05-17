using Microsoft.AspNetCore.Mvc;
using MovieQuotesAPI.Data;
using MovieQuotesAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MovieQuotesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == dto.Email);

            if (admin == null)
                return Unauthorized("Invalid email");

            // ⚠️ დროებით plain password comparison (შემდეგ დავამატებთ hashing-ს)
            if (admin.PasswordHash != dto.Password)
                return Unauthorized("Invalid password");

            var token = GenerateJwtToken(admin);

            return Ok(new { token });
        }

        private string GenerateJwtToken(Models.Admin admin)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, admin.Email)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
