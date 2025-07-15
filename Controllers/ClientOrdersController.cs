using Microsoft.AspNetCore.Mvc;
using BlanchisserieBackend.Services;
using BlanchisserieBackend.DTOs;
using BlanchisserieBackend.Payload;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace BlanchisserieBackend.Controllers;

[ApiController]
[Route("api/clientOrders")]
[Authorize]
public class ClientOrdersController : ControllerBase
{
    private readonly ClientOrderService _clientOrderService;
    private readonly IMapper _mapper;
    public ClientOrdersController(ClientOrderService clientOrderService, IMapper mapper)
    {
        _clientOrderService = clientOrderService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientOrderDto>>> Get()
    {
        var clientOrderList = await _clientOrderService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<ClientOrderDto>>(clientOrderList));
    }

    [HttpPost]
    public async Task<ActionResult<ClientOrderDto>> Post(ClientOrderPayload clientOrderPayload)
    {
        var createdClientOrder = await _clientOrderService.CreateAsync(clientOrderPayload);
        return CreatedAtAction(nameof(Get), new { id = createdClientOrder.Id }, _mapper.Map<ClientOrderDto>(createdClientOrder));
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<ClientOrderDto>>> GetClientOrdersByUserId(int userId)
    {
        var clientOrders = await _clientOrderService.GetByUserIdAsync(userId);
        return Ok(_mapper.Map<IEnumerable<ClientOrderDto>>(clientOrders));
    }

    [Authorize(Roles = "Admin")]
    [HttpPatch("{id}")]
    public async Task<ActionResult<ClientOrderDto>> UpdateOrderStatus(int id, [FromBody] UpdateClientOrderStatusPayload payload)
    {
        var updatedOrder = await _clientOrderService.UpdateOrderStatusAndReturnAsync(id, payload.Status);
        if (updatedOrder == null) return NotFound();
        return Ok(_mapper.Map<ClientOrderDto>(updatedOrder));
    }

}