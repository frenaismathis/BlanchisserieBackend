using System.ComponentModel.DataAnnotations;
using BlanchisserieBackend.Payload;

namespace BlanchisserieBackend.Models
{
    public class ClientOrderArticle
    {
        public ClientOrderArticle(){}
        public ClientOrderArticle(ClientOrderArticlePayload clientOrderArticlePayload, int? clientOrderId = null)
        {
            ClientOrderId = clientOrderId ?? clientOrderArticlePayload.ClientOrderId;
            ArticleId = clientOrderArticlePayload.ArticleId;
            Quantity = clientOrderArticlePayload.Quantity;
        }

        [Required]
        public int ClientOrderId { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public ClientOrder? ClientOrder { get; set; }

        public Article? Article { get; set; }
        
        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}