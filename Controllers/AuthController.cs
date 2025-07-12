using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlanchisserieBackend.Data;
using BlanchisserieBackend.Services;
using BlanchisserieBackend.Models;
using BlanchisserieBackend.Mappers;
using Microsoft.AspNetCore.Authorization;
using BlanchisserieBackend.DTOs;

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
    public async Task<IActionResult> Login(LoginRequest loginRequest)
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

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Me()
    {
        // Récupère l'ID utilisateur depuis le token JWT, NameIdentifier = identifiant unique
        var userIdClaim = User.FindFirst("id") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
            return Unauthorized();

        // Vérifie si l'ID extrait du token JWT est un entier
        if (!int.TryParse(userIdClaim.Value, out var userId))
            return Unauthorized();

        // Charge l'utilisateur avec son rôle
        var user = await _context.Users
            .Include(user => user.Role)
            .FirstOrDefaultAsync(user => user.Id == userId);

        if (user == null)
            return Unauthorized();

        return Ok(new { user = UserMapper.ToUserDto(user) });
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        // Supprime le cookie HTTP-only
        Response.Cookies.Delete("access_token");
        return Ok(new { message = "Logged out successfully" });
    }

}
