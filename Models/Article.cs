using System.ComponentModel.DataAnnotations;
using BlanchisserieBackend.Payload;

namespace BlanchisserieBackend.Models
{
    public class Article
    {
        public Article() { }

        public Article(ArticlePayload articlePayload)
        {
            Name = articlePayload.Name;
            Description = articlePayload.Description;
            Price = articlePayload.Price;
            ImagePath = articlePayload.ImagePath;
        }
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImagePath { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}