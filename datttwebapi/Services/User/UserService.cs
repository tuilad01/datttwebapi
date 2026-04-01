using datttwebapi.Data;
using Microsoft.EntityFrameworkCore;

namespace datttwebapi.Services
{
    public class UserService : IUserService
    {
        private readonly ApiVersioningDbContext _context;

        public UserService(ApiVersioningDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<User>> GetAll()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }
    }
}
