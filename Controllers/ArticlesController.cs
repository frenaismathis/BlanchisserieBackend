using Microsoft.AspNetCore.Mvc;
using BlanchisserieBackend.Services;
using BlanchisserieBackend.Mappers;
using BlanchisserieBackend.Payload;
using BlanchisserieBackend.DTOs;

namespace BlanchisserieBackend.Controllers;

[ApiController]
[Route("api/articles")]
public class ArticlesController : ControllerBase
{
    private readonly ArticleService _articleService;
    public ArticlesController(ArticleService articleService)
    {
        _articleService = articleService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> Get()
    {
        var articles = await _articleService.GetAllAsync();
        return Ok(articles.Select(ArticleMapper.ToArticleDto));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ArticleDto>> Get(int id)
    {
        var article = await _articleService.GetByIdAsync(id);
        if (article is null) return NotFound();
        return Ok(ArticleMapper.ToArticleDto(article));
    }

    [HttpPost]
    public async Task<ActionResult<ArticleDto>> Post(ArticlePayload articlePayload)
    {
        var createdArticle = await _articleService.CreateAsync(articlePayload);
        return CreatedAtAction(nameof(Get), new { id = createdArticle.Id }, ArticleMapper.ToArticleDto(createdArticle));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ArticlePayload articlePayload)
    {
        var updatedArticle = await _articleService.UpdateAsync(id, articlePayload);
        if (!updatedArticle) return BadRequest();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deletedArticle = await _articleService.DeleteAsync(id);
        if (!deletedArticle) return NotFound();
        return NoContent();
    }
}