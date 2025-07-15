using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BlanchisserieBackend.DTOs;
using AutoMapper;
using BlanchisserieBackend.Payload;

namespace BlanchisserieBackend.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    private readonly IMapper _mapper;
    public AuthController(AuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginPayload LoginPayload)
    {
        var (token, user) = await _authService.LoginAsync(LoginPayload.Email, LoginPayload.Password);
        if (token == null || user == null)
            return Unauthorized("Invalid credentials");

        Response.Cookies.Append("access_token", token, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true
        });

        // If the request comes from Swagger, also return the token in the response body
        var referer = Request.Headers["Referer"].ToString();
        var isSwaggerWeb = referer.Contains("/swagger", StringComparison.OrdinalIgnoreCase);
        
        if (isSwaggerWeb)
        {
            return Ok(new
            {
                user = _mapper.Map<UserDto>(user),
                token
            });
        }

        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> Me()
    {
        var user = await _authService.GetCurrentUserAsync(User);
        if (user == null)
            return Unauthorized();
        return Ok(_mapper.Map<UserDto>(user));
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("access_token");
        return Ok(new { message = "Logged out successfully" });
    }
}
