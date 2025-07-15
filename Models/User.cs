using System.ComponentModel.DataAnnotations;

namespace BlanchisserieBackend.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        public string Lastname { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public int Civilite { get; set; }

        public ICollection<ClientOrder> ClientOrders { get; set; } = new List<ClientOrder>();
        
        [Required]
        public int RoleId { get; set; }

        public Role? Role { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
