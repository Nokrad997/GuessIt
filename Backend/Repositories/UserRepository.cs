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

    public async Task<User> GetUserByEmail(string email)
    {
        return await _context.User.FirstOrDefaultAsync(user => user.Email == email);
    }

    public async Task<User> GetUserById(int id)
    {
       return await _context.User.FirstOrDefaultAsync(user => user.UserId == id); 
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _context.User.ToListAsync();
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public async Task EditUser(User user)
    {
        var editedUser = await _context.User.FindAsync(user.UserId);
        if (editedUser is not null)
        {
            editedUser = user;
            await SaveChanges();    
        }
    }

    public async Task DeleteUserById(int id)
    {
        var user = await _context.User.FindAsync(id);

        if (user is not null)
        {
            _context.User.Remove(user);
            await SaveChanges();
        }
        else
        {
            throw new KeyNotFoundException("User not found");
        }
    }
}