using System.ComponentModel.DataAnnotations;

namespace BlanchisserieBackend.Payload
{
    public class ClientOrderArticleCreatePayload
    {
        [Required]
        public int ClientOrderId { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}