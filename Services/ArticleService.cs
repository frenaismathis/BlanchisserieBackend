using BlanchisserieBackend.Data;
using BlanchisserieBackend.Models;
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
    }
}
