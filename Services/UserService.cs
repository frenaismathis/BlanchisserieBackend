using BlanchisserieBackend.Data;
using BlanchisserieBackend.Models;
using Microsoft.EntityFrameworkCore;
using BlanchisserieBackend.Payload;

namespace BlanchisserieBackend.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.Include(user => user.Role).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.Include(user => user.Role).FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> CreateAsync(UserPayload userPayload)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(userPayload.Password);
            var user = new User(userPayload)
            {
                Password = passwordHash
            };
            var createdUser = _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return createdUser.Entity;
        }

        public async Task<bool> UpdateAsync(int id, UserPayload userPayload)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return false;
            
            user.Firstname = userPayload.Firstname;
            user.Lastname = userPayload.Lastname;
            user.Email = userPayload.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userPayload.Password);
            user.Civilite = userPayload.Civilite;
            user.RoleId = userPayload.RoleId;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity is null) return false;
            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
