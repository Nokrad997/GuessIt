using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface IFriendsRepository
{
    Task<IEnumerable<Friends>> GetAllFriends();
    Task<Friends?> GetFriendsById(int id);
    Task<IEnumerable<Friends>> GetFriendsByUserId(int userId);
    Task<IEnumerable<Friends>> GetFriendsByFriendId(int friendId);
    Task<Friends?> GetFriendsByUserAndFriendId(int userId, int friendId);
    Task AddFriends(Friends friends);
    Task EditFriends(Friends friends);
    Task DeleteFriendsById(int id);
}