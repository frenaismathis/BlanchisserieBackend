using BlanchisserieBackend.DTOs;
using BlanchisserieBackend.Models;

namespace BlanchisserieBackend.Mappers
{
    public static class ClientOrderMapper
    {
        public static ClientOrderDto ToClientOrderDto(ClientOrder clientOrder)
        {
            return new ClientOrderDto
            {
                Id = clientOrder.Id,
                TotalPrice = clientOrder.TotalPrice,
                OrderDate = clientOrder.OrderDate,
                Motif = clientOrder.Motif,
                Commentary = clientOrder.Commentary,
                UserId = clientOrder.UserId,
                Username = clientOrder.User == null ? null : clientOrder.User.Firstname + " " + clientOrder.User.Lastname,
                Status = clientOrder.Status,
                ClientOrderArticles = clientOrder.ClientOrderArticles?.Select(clientOrderArticle => new ClientOrderArticleDto
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
                }).ToList() ?? new List<ClientOrderArticleDto>()
            };
        }
    }
}
