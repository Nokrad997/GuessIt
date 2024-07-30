using Backend.Entities;

namespace Backend.Repositories.Interfaces;

public interface IFriendsRepository
{
    Task<Friends?> GetFriendsById(int id);
    Task<IEnumerable<Friends>> GetFriendsByUserId(int userId);
    Task<IEnumerable<Friends>> GetFriendsByFriendId(int friendId);
    Task AddFriends(Friends friends);
    Task EditFriends(Friends friends);
    Task DeleteFriendsById(int id);
}