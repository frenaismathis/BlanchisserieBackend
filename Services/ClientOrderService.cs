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

        public async Task<ClientOrder?> GetByIdAsync(int id)
        {
            return await _context.ClientOrders.Include(clientOrder => clientOrder.User).FirstOrDefaultAsync(clientOrder => clientOrder.Id == id);
        }

        public async Task<ClientOrder> CreateAsync(ClientOrderPayload clientOrderPayload)
        {
            var createdClientOrder = _context.ClientOrders.Add(new ClientOrder(clientOrderPayload));
            await _context.SaveChangesAsync();
            return createdClientOrder.Entity;
        }

        public async Task<bool> UpdateAsync(int id, ClientOrderPayload clientOrderPayload)
        {
            var clientOrder = await _context.ClientOrders.FindAsync(id);
            if (clientOrder is null) return false;

            clientOrder.TotalPrice = clientOrderPayload.TotalPrice;
            clientOrder.UserId = clientOrderPayload.UserId;
            clientOrder.Motif = clientOrderPayload.Motif;
            clientOrder.Commentary = clientOrderPayload.Commentary;
            clientOrder.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var clientOrder = await _context.ClientOrders.FindAsync(id);
            if (clientOrder is null) return false;
            _context.ClientOrders.Remove(clientOrder);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
