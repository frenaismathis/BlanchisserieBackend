using Microsoft.AspNetCore.Mvc;
using BlanchisserieBackend.Services;
using BlanchisserieBackend.DTOs;
using BlanchisserieBackend.Mappers;
using BlanchisserieBackend.Payload;
using Microsoft.AspNetCore.Authorization;

namespace BlanchisserieBackend.Controllers;

[ApiController]
[Route("api/clientOrders")]
[Authorize]
public class ClientOrdersController : ControllerBase
{
    private readonly ClientOrderService _service;
    public ClientOrdersController(ClientOrderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientOrderDto>>> Get()
    {
        var clientOrderList = await _service.GetAllAsync();
        return clientOrderList.Select(ClientOrderMapper.ToClientOrderDto).ToList();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ClientOrderDto>> Get(int id)
    {
        var clientOrder = await _service.GetByIdAsync(id);
        return clientOrder is null ? NotFound() : Ok(ClientOrderMapper.ToClientOrderDto(clientOrder));
    }

    [HttpPost]
    public async Task<ActionResult<ClientOrderDto>> Post(ClientOrderPayload clientOrderPayload)
    {
        var createdClientOrder = await _service.CreateAsync(clientOrderPayload);
        return CreatedAtAction(nameof(Get), new { id = createdClientOrder.Id }, ClientOrderMapper.ToClientOrderDto(createdClientOrder));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ClientOrderPayload clientOrderPayload)
    {
        var updatedClientOrder = await _service.UpdateAsync(id, clientOrderPayload);
        if (!updatedClientOrder) return BadRequest();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedClientOrder = await _service.DeleteAsync(id);
        if (!deletedClientOrder) return NotFound();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ClientOrderDto>>> GetClientOrdersByUserId(int userId)
    {
        var clientOrders = await _service.GetByUserIdAsync(userId);
        return clientOrders.Select(ClientOrderMapper.ToClientOrderDto).ToList();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}")]
    public async Task<ActionResult<ClientOrderDto>> UpdateOrderStatus(int id, [FromBody] UpdateClientOrderStatusPayload payload)
    {
        var updatedOrder = await _service.UpdateOrderStatusAndReturnAsync(id, payload.Status);
        if (updatedOrder == null) return NotFound();
        return Ok(ClientOrderMapper.ToClientOrderDto(updatedOrder));
    }

}