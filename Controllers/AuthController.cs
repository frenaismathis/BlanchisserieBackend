using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlanchisserieBackend.Data;
using BlanchisserieBackend.Services;
using BlanchisserieBackend.Models;
using BlanchisserieBackend.Mappers;

namespace BlanchisserieBackend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;
    public AuthController(ApplicationDbContext context, IJwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var user = await _context.Users
            .Include(user => user.Role)
            .FirstOrDefaultAsync(user => user.Email == loginRequest.Email);

        if (user == null)
            return Unauthorized("Invalid credentials");

        // Verify hashed password
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);
        if (!isPasswordValid)
            return Unauthorized("Invalid credentials");

        var token = _jwtTokenService.GenerateToken(user);

        // Ajoute le cookie HTTP-only
        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None, // Important pour cross-origin
            Secure = true // Obligatoire si tu es en HTTPS
        });

        return Ok(new { user = UserMapper.ToUserDto(user) });
    }

}
