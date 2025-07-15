using System.ComponentModel.DataAnnotations;

namespace BlanchisserieBackend.Payload
{
    public class ClientOrderArticlePayload
    {
        [Required]
        public int ClientOrderId { get; set; }

        [Required]
        public int ArticleId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}