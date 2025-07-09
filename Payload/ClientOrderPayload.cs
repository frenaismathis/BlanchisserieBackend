using System.ComponentModel.DataAnnotations;

namespace BlanchisserieBackend.Payload
{
    public class ClientOrderPayload
    {
        [Required]
        public decimal TotalPrice { get; set; }

        public string? Motif { get; set; }

        public string? Commentary { get; set; }

        [Required]
        public int UserId { get; set; }
    }
}
