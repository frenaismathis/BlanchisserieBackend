using BlanchisserieBackend.DTOs;
using BlanchisserieBackend.Models;

namespace BlanchisserieBackend.Mappers
{
    public static class ArticleMapper
    {
        public static ArticleDto ToArticleDto(Article article)
        {
            return new ArticleDto
            {
                Id = article.Id,
                Name = article.Name,
                Description = article.Description,
                Price = article.Price,
            };
        }
    }
}
