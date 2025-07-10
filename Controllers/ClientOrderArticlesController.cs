using Microsoft.AspNetCore.Mvc;
using BlanchisserieBackend.Services;
using BlanchisserieBackend.Payload;
using BlanchisserieBackend.DTOs;
using BlanchisserieBackend.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace BlanchisserieBackend.Controllers;

[ApiController]
[Route("api/clientOrderArticles")]
[Authorize]
public class ClientOrderArticlesController : ControllerBase
{
    private readonly ClientOrderArticleService _service;
    public ClientOrderArticlesController(ClientOrderArticleService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientOrderArticleDto>>> Get()
    {
        var listClientOrderArticles = await _service.GetAllAsync();
        return listClientOrderArticles.Select(ClientOrderArticleMapper.ToClientOrderDto).ToList();
    }

    [HttpGet("{clientOrderId}/{articleId}")]
    public async Task<ActionResult<ClientOrderArticleDto>> Get(int clientOrderId, int articleId)
    {
        var clientOrderArticle = await _service.GetByIdsAsync(clientOrderId, articleId);
        if (clientOrderArticle is null) return NotFound();
        return Ok(ClientOrderArticleMapper.ToClientOrderDto(clientOrderArticle));
    }

    [HttpPost]
    public async Task<ActionResult<ClientOrderArticleDto>> Post(ClientOrderArticleCreatePayload clientOrderArticleCreatePayload)
    {
        var createdClientOrderArticle = await _service.CreateAsync(clientOrderArticleCreatePayload);
        return CreatedAtAction(nameof(Get), new
        {
            clientOrderId = createdClientOrderArticle.ClientOrderId,
            articleId = createdClientOrderArticle.ArticleId
        }, ClientOrderArticleMapper.ToClientOrderDto(createdClientOrderArticle));
    }

    [HttpPut("{clientOrderId}/{articleId}")]
    public async Task<IActionResult> Put(int clientOrderId, int articleId, ClientOrderArticleUpdatePayload clientOrderArticleUpdatePayload)
    {
        var updatedClientOrderArticle = await _service.UpdateAsync(clientOrderId, articleId, clientOrderArticleUpdatePayload);
        if (!updatedClientOrderArticle) return BadRequest();
        return NoContent();
    }

    [HttpDelete("{clientOrderId}/{articleId}")]
    public async Task<IActionResult> Delete(int clientOrderId, int articleId)
    {
        var deletedClientOrderArticle = await _service.DeleteAsync(clientOrderId, articleId);
        if (!deletedClientOrderArticle) return NotFound();
        return NoContent();
    }
}
