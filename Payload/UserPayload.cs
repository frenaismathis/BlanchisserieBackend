using System.ComponentModel.DataAnnotations;

namespace BlanchisserieBackend.Payload

{
    public class UserPayload
    {
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

        [Required]
        public int RoleId { get; set; }

    }
}
