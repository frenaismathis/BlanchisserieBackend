using System.ComponentModel.DataAnnotations;

namespace BlanchisserieBackend.Payload
{
    public class ClientOrderArticleUpdatePayload
    {
        [Required]
        public int Quantity { get; set; }
    }
}