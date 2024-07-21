using Backend.Entities;

namespace Backend.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByEmailOrUsername(string email, string username);
    Task AddUser(User user);
    Task SaveChanges();
}