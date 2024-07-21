using Backend.Context;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly GuessItContext _context;

    public UserRepository(GuessItContext context)
    {
        _context = context;
    }

    public async Task AddUser(User user)
    {
        await _context.User.AddAsync(user);
        await SaveChanges();
    }

    public async Task<User> GetUserByEmailOrUsername(string email, string username)
    {
        return await _context.User.FirstOrDefaultAsync(user => user.Username == username || user.Email == email);
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}