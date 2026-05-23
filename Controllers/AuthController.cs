using Microsoft.AspNetCore.Mvc;
using MovieQuotesAPI.Data;
using MovieQuotesAPI.DTOs;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using MovieQuotesAPI.Models;

namespace MovieQuotesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<Models.Admin> _passwordHasher = new();

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var existingUser = await _context.Admins
                .FirstOrDefaultAsync(x => x.Email == dto.Email);

            if (existingUser != null)
                return BadRequest("User already exists!");

            var admin = new Admin
            {
                Name = dto.Name,
                Email = dto.Email
            };

            admin.PasswordHash = _passwordHasher.HashPassword(admin, dto.Password);

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();

            return Ok("User created successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Email == dto.Email);

            if (admin == null)
                return Unauthorized("Invalid email");

            var result = _passwordHasher.VerifyHashedPassword(
                admin,
                admin.PasswordHash,
                dto.Password
            );

            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid password");

            var token = GenerateJwtToken(admin);

            return Ok(new { token });
        }

        private string GenerateJwtToken(Models.Admin admin)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, admin.Email),
                new Claim(ClaimTypes.Role, "Admin")
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
