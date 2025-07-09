namespace BlanchisserieBackend.DTOs
{
    public class ClientOrderArticleDto
    {
        public int ClientOrderId { get; set; }
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public ArticleDto? Article { get; set; }

    }
}
