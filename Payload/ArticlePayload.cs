using System.ComponentModel.DataAnnotations;

namespace BlanchisserieBackend.Payload
{
    public class ArticlePayload
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string ImagePath { get; set; } = string.Empty;
    }
}