using BlanchisserieBackend.Data;
using BlanchisserieBackend.Models;
using BlanchisserieBackend.Payload;
using Microsoft.EntityFrameworkCore;

namespace BlanchisserieBackend.Services
{
    public class ArticleService
    {
        private readonly ApplicationDbContext _context;
        public ArticleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Article>> GetAllAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article?> GetByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task<Article> CreateAsync(ArticlePayload articlePayload)
        {
            var createdArticle = _context.Articles.Add(new Article(articlePayload));
            await _context.SaveChangesAsync();
            return createdArticle.Entity;
        }

        public async Task<bool> UpdateAsync(int id, ArticlePayload articlePayload)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article is null) return false;

            article.Name = articlePayload.Name;
            article.Description = articlePayload.Description;
            article.Price = articlePayload.Price;
            article.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article is null) return false;
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
