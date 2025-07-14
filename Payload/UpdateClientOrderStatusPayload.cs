using System.ComponentModel.DataAnnotations;

namespace BlanchisserieBackend.Payload
{
    public class UpdateClientOrderStatusPayload
    {
        [Required]
        public int Status { get; set; }
    }
}