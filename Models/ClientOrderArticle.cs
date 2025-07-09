using System.ComponentModel.DataAnnotations;

namespace BlanchisserieBackend.Models
{
    public class ClientOrderArticle
    {
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