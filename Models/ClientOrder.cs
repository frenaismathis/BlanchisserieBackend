using System.ComponentModel.DataAnnotations;
using BlanchisserieBackend.Payload;
public enum ClientOrderStatus
{
    PendingValidation = 0,
    Validated = 1,
    Rejected = 2,
    Delivered = 3
}

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

        public ClientOrderStatus Status { get; set; } = ClientOrderStatus.PendingValidation;

        public DateTime OrderDate { get; set; }

        [Required]
        public int UserId { get; set; }

        public User? User { get; set; }

        public ICollection<ClientOrderArticle> ClientOrderArticles { get; set; } = new List<ClientOrderArticle>();

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
