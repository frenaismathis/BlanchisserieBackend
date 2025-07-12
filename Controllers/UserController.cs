using Microsoft.AspNetCore.Mvc;
using BlanchisserieBackend.Services;
using BlanchisserieBackend.Mappers;
using BlanchisserieBackend.DTOs;
using BlanchisserieBackend.Payload;
using Microsoft.AspNetCore.Authorization;
using BlanchisserieBackend.Models;

namespace BlanchisserieBackend.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly UserService _service;
    public UsersController(UserService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get()
    {
        var userList = await _service.GetAllAsync();
        return userList.Select(UserMapper.ToUserDto).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(int id)
    {
        var user = await _service.GetByIdAsync(id);
        if (user is null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> Post(UserPayload userPayload)
    {
        var createdUser = await _service.CreateAsync(userPayload);
        return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, UserMapper.ToUserDto(createdUser));
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, UserPayload userPayload)
    {
        var updatedUser = await _service.UpdateAsync(id, userPayload);
        if (!updatedUser) return BadRequest();
        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedUser = await _service.DeleteAsync(id);
        if (!deletedUser) return NotFound();
        return NoContent();
    }

}
