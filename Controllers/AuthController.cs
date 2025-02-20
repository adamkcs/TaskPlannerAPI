using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TaskPlannerAPI.DTOs;
using TaskPlannerAPI.Models;
using TaskPlannerAPI.Helpers;

namespace TaskPlannerAPI.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private static List<User> _users = new(); // Simulated user storage (db in production)

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    /// <summary>
    /// Authenticates the user and returns a JWT token.
    /// </summary>
    [HttpPost("login")]
    public IActionResult Login([FromBody] User user)
    {
        var existingUser = _users.FirstOrDefault(u => u.Username == user.Username);

        if (existingUser == null || !PasswordHasher.VerifyPassword(user.HashedPassword, existingUser.HashedPassword))
        {
            return Unauthorized(new { message = "Invalid credentials" });
        }

        var token = GenerateJwtToken(user.Username);
        return Ok(new { Token = token });
    }

    /// <summary>
    /// Registers a new user with a hashed password.
    /// </summary>
    [HttpPost("register")]
    public IActionResult Register([FromBody] User user)
    {
        if (_users.Any(u => u.Username == user.Username))
            return BadRequest(new { message = "Username already exists" });

        user.HashedPassword = PasswordHasher.HashPassword(user.HashedPassword);
        _users.Add(user);

        return Ok(new { message = "User registered successfully" });
    }

    private string GenerateJwtToken(string username)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var key = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"] 
            ?? throw new ArgumentNullException("SecretKey is missing in configuration."));

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, username) }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
