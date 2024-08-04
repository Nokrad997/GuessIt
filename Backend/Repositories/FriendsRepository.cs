using Backend.Context;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Interfaces;

public class FriendsRepository : IFriendsRepository
{
    private readonly GuessItContext _context;
    
    public FriendsRepository(GuessItContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Friends>> GetAllFriends()
    {
        return await _context.Friends.ToListAsync();
    }

    public async Task<Friends?> GetFriendsById(int id)
    {
        return await _context.Friends.FindAsync(id);
    }
    
    public async Task<IEnumerable<Friends>> GetFriendsByUserId(int userId)
    {
        return await _context.Friends.Where(friends => friends.UserIdFk == userId).ToListAsync();
    }
    
    public async Task<IEnumerable<Friends>> GetFriendsByFriendId(int friendId)
    {
        return await _context.Friends.Where(friends => friends.FriendIdFk == friendId).ToListAsync();
    }
    
    public async Task<Friends?> GetFriendsByUserAndFriendId(int userId, int friendId)
    {
        return await _context.Friends.FirstOrDefaultAsync(friends => friends.UserIdFk == userId && friends.FriendIdFk == friendId) ??
               await _context.Friends.FirstOrDefaultAsync(friends => friends.UserIdFk == friendId && friends.FriendIdFk == userId);
    }
    
    public async Task AddFriends(Friends friends)
    {
        await _context.Friends.AddAsync(friends);
        await SaveChanges();
    }
    
    public async Task EditFriends(Friends friends)
    {
        var editedFriends = await _context.Friends.FindAsync(friends.FriendsId);
        if (editedFriends is not null)
        {
            editedFriends = friends;
            await SaveChanges();
        }
    }
    
    public async Task DeleteFriendsById(int id)
    {
        var friends = await _context.Friends.FindAsync(id);
        if (friends is not null)
        {
            _context.Friends.Remove(friends);
            await SaveChanges();
        }
    }
    
    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }
}