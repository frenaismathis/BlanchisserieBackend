using BlanchisserieBackend.Data;
using BlanchisserieBackend.Models;
using BlanchisserieBackend.Payload;
using Microsoft.EntityFrameworkCore;

namespace BlanchisserieBackend.Services
{
    public class ClientOrderArticleService
    {
        private readonly ApplicationDbContext _context;
        public ClientOrderArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientOrderArticle>> GetAllAsync()
        {
            return await _context.ClientOrderArticles.Include(clientOrderArticle => clientOrderArticle.Article).ToListAsync();
        }

        public async Task<ClientOrderArticle?> GetByIdsAsync(int clientOrderId, int articleId)
        {
            return await _context.ClientOrderArticles
                .Include(clientOrderArticle => clientOrderArticle.Article)
                .FirstOrDefaultAsync(clientOrderArticle => clientOrderArticle.ClientOrderId == clientOrderId && clientOrderArticle.ArticleId == articleId);
        }

        public async Task<ClientOrderArticle> CreateAsync(ClientOrderArticlePayload clientOrderArticlePayload)
        {
            var createdClientOrderArticle = _context.ClientOrderArticles.Add(new ClientOrderArticle(clientOrderArticlePayload));
            await _context.SaveChangesAsync();
            return (await _context.ClientOrderArticles
                .Include(clientOrderArticle => clientOrderArticle.Article)
                .FirstOrDefaultAsync(clientOrderArticle => clientOrderArticle.ClientOrderId == clientOrderArticle.ClientOrderId && clientOrderArticle.ArticleId == clientOrderArticle.ArticleId))!;
        }

        public async Task<bool> UpdateAsync(int clientOrderId, int articleId, ClientOrderArticlePayload clientOrderArticlePayload)
        {
            var clientOrderArticle = await _context.ClientOrderArticles.FindAsync(clientOrderId, articleId);
            if (clientOrderArticle is null) return false;

            clientOrderArticle.ClientOrderId = clientOrderArticlePayload.ClientOrderId;
            clientOrderArticle.ArticleId = clientOrderArticlePayload.ArticleId;
            clientOrderArticle.Quantity = clientOrderArticlePayload.Quantity;
            clientOrderArticle.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int clientOrderId, int articleId)
        {
            var entity = await _context.ClientOrderArticles.FindAsync(clientOrderId, articleId);
            if (entity is null) return false;
            _context.ClientOrderArticles.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
