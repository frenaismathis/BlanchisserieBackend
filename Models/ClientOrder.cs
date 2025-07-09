using System.ComponentModel.DataAnnotations;
using BlanchisserieBackend.Payload;

namespace BlanchisserieBackend.Models
{
    public class ClientOrder
    {
        public ClientOrder() { }

        public ClientOrder(ClientOrderPayload clientOrderPayload)
        {
            TotalPrice = clientOrderPayload.TotalPrice;
            Motif = clientOrderPayload.Motif;
            Commentary = clientOrderPayload.Commentary;
            UserId = clientOrderPayload.UserId;
        }

        public int Id { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }

        public string? Motif { get; set; }

        public string? Commentary { get; set; }

        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }

        public ICollection<ClientOrderArticle> ClientOrderArticles { get; set; } = new List<ClientOrderArticle>();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }  
    }
}
