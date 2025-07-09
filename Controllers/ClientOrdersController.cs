using Microsoft.AspNetCore.Mvc;
using BlanchisserieBackend.Services;
using BlanchisserieBackend.DTOs;
using BlanchisserieBackend.Mappers;
using BlanchisserieBackend.Payload;

namespace BlanchisserieBackend.Controllers;

[ApiController]
[Route("api/clientOrders")]
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

}
