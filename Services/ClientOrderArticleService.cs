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

        public async Task<ClientOrderArticle> CreateAsync(ClientOrderArticleCreatePayload clientOrderArticleCreatePayload)
        {
            var clientOrderArticle = new ClientOrderArticle(clientOrderArticleCreatePayload);

            _context.ClientOrderArticles.Add(clientOrderArticle);
            await _context.SaveChangesAsync();  
            
            return (await _context.ClientOrderArticles
                .Include(clientOrderArticle => clientOrderArticle.Article)
                .FirstOrDefaultAsync(clientOrderArticleDb => clientOrderArticleDb.ClientOrderId == clientOrderArticle.ClientOrderId && clientOrderArticleDb.ArticleId == clientOrderArticle.ArticleId))!;
        }

        public async Task<bool> UpdateAsync(int clientOrderId, int articleId, ClientOrderArticleUpdatePayload clientOrderArticleUpdatePayload)
        {
            var clientOrderArticle = await _context.ClientOrderArticles.FindAsync(clientOrderId, articleId);
            if (clientOrderArticle is null) return false;

            clientOrderArticle.Quantity = clientOrderArticleUpdatePayload.Quantity;
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
