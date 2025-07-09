using BlanchisserieBackend.DTOs;
using BlanchisserieBackend.Models;

namespace BlanchisserieBackend.Mappers
{
    public static class ClientOrderArticleMapper
    {
        public static ClientOrderArticleDto ToClientOrderDto(ClientOrderArticle clientOrderArticle)
        {
            return new ClientOrderArticleDto
            {
                ClientOrderId = clientOrderArticle.ClientOrderId,
                ArticleId = clientOrderArticle.ArticleId,
                Quantity = clientOrderArticle.Quantity,
                Article = clientOrderArticle.Article == null ? null : new ArticleDto
                {
                    Id = clientOrderArticle.Article.Id,
                    Name = clientOrderArticle.Article.Name,
                    Price = clientOrderArticle.Article.Price
                }
            };
        }
    }
}
