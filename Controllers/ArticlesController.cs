using Microsoft.AspNetCore.Mvc;
using BlanchisserieBackend.Services;
using BlanchisserieBackend.DTOs;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;

namespace BlanchisserieBackend.Controllers;

[ApiController]
[Route("api/articles")]
[Authorize]
public class ArticlesController : ControllerBase
{
    private readonly ArticleService _articleService;
    private readonly IMapper _mapper;
    public ArticlesController(ArticleService articleService, IMapper mapper)
    {
        _articleService = articleService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ArticleDto>>> Get()
    {
        var articles = await _articleService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<ArticleDto>>(articles));
    }

}