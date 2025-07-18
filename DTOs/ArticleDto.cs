namespace BlanchisserieBackend.DTOs
{
    public class ArticleDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }
        
        public string ImagePath { get; set; } = string.Empty;
    }
}