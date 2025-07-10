using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlanchisserieBackend.Data;
using BlanchisserieBackend.Services;
using BlanchisserieBackend.Models;

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
        var user = await _context.Users.FirstOrDefaultAsync(user => user.Email == loginRequest.Email);
        if (user == null)
            return Unauthorized("Invalid credentials");

        // Verify hashed password
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password);
        if (!isPasswordValid)
            return Unauthorized("Invalid credentials");

        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new { token });
    }

}
