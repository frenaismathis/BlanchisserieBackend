namespace BlanchisserieBackend.DTOs
{
    public class ClientOrderDto
    {
        public int Id { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Motif { get; set; }
        public string? Commentary { get; set; }
        public int UserId { get; set; } 
        public UserDto? User { get; set; }
        public string? Username { get; set; }
        public ClientOrderStatus Status { get; set; }
        public List<ClientOrderArticleDto> ClientOrderArticles { get; set; } = new List<ClientOrderArticleDto>();
    }
}
