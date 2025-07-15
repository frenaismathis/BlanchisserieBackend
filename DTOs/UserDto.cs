namespace BlanchisserieBackend.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; } = string.Empty;
        public string Lastname { get; set; } = string.Empty;
        public string? Email { get; set; }
        public int Civilite { get; set; }
        public int RoleId { get; set; }
        public RoleDto? Role { get; set; }
    }
}