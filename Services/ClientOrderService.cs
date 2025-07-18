using BlanchisserieBackend.Data;
using BlanchisserieBackend.Models;
using BlanchisserieBackend.Payload;
using Microsoft.EntityFrameworkCore;

namespace BlanchisserieBackend.Services
{
    public class ClientOrderService
    {
        private readonly ApplicationDbContext _context;
        public ClientOrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientOrder>> GetAllAsync()
        {
            return await _context.ClientOrders
                .Include(clientOrder => clientOrder.User)
                .Include(clientOrder => clientOrder.ClientOrderArticles)
                    .ThenInclude(clientOrderArticle => clientOrderArticle.Article)
                .ToListAsync();
        }

        public async Task<ClientOrder> CreateAsync(ClientOrderPayload clientOrderPayload)
        {
            var clientOrder = new ClientOrder(clientOrderPayload);
            _context.ClientOrders.Add(clientOrder);
            await _context.SaveChangesAsync();

            foreach (var articlePayload in clientOrderPayload.ClientOrderArticles)
            {
                var clientOrderArticle = new ClientOrderArticle(articlePayload, clientOrder.Id);
                _context.ClientOrderArticles.Add(clientOrderArticle);
            }
            await _context.SaveChangesAsync();

            var createdClientOrder = await _context.ClientOrders
                .Include(clientOrderDb => clientOrderDb.User)
                .Include(clientOrderDb => clientOrderDb.ClientOrderArticles)
                    .ThenInclude(clientOrderArticle => clientOrderArticle.Article)
                .FirstOrDefaultAsync(clientOrderDb => clientOrderDb.Id == clientOrder.Id);

            return createdClientOrder!;
        }

        public async Task<List<ClientOrder>> GetByUserIdAsync(int userId)
        {
            return await _context.ClientOrders
                .Where(clientOrder => clientOrder.UserId == userId)
                .Include(clientOrder => clientOrder.User)
                .Include(clientOrder => clientOrder.ClientOrderArticles)
                    .ThenInclude(clientOrderArticle => clientOrderArticle.Article)
                .ToListAsync();
        }

        public async Task<ClientOrder?> UpdateOrderStatusAndReturnAsync(int orderId, int status)
        {
            var order = await _context.ClientOrders
                .Include(clientOrder => clientOrder.User)
                .Include(clientOrder => clientOrder.ClientOrderArticles)
                    .ThenInclude(clientOrderArticle => clientOrderArticle.Article)
                .FirstOrDefaultAsync(clientOrder => clientOrder.Id == orderId);

            if (order == null) return null;

            order.Status = (ClientOrderStatus)status;
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return order;
        }

    }
}
