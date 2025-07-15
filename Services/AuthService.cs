using BlanchisserieBackend.Data;
using Microsoft.EntityFrameworkCore;
using BlanchisserieBackend.Services;
using BlanchisserieBackend.Models;

public class AuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(ApplicationDbContext context, IJwtTokenService jwtTokenService)
    {
        _context = context;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<(string? token, User? user)> LoginAsync(string email, string password)
    {
        var user = await _context.Users
            .Include(user => user.Role)
            .FirstOrDefaultAsync(user => user.Email == email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            return (null, null);

        var token = _jwtTokenService.GenerateToken(user);
        return (token, user);
    }

    public async Task<User?> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal principal)
    {
        // Retrieve the user ID from the JWT claims (either "id" or NameIdentifier)
        // If the claim is missing or not a valid integer, return null
        var userIdClaim = principal.FindFirst("id") ?? principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
            return null;

        var user = await _context.Users
            .Include(user => user.Role)
            .FirstOrDefaultAsync(user => user.Id == userId);

        return user == null ? null : user;
    }
}