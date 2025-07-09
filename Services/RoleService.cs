using BlanchisserieBackend.Data;
using BlanchisserieBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BlanchisserieBackend.Services
{
    public class RoleService
    {
        private readonly ApplicationDbContext _context;
        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<Role> CreateAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task<bool> UpdateAsync(int id, Role role)
        {
            if (id != role.Id) return false;
            role.UpdatedAt = DateTime.UtcNow;
            _context.Entry(role).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Roles.FindAsync(id);
            if (entity is null) return false;
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
