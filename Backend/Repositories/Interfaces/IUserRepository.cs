using Backend.Entities;

namespace Backend.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserById(int id);
    Task<IEnumerable<User>> GetAllUsers();
    Task AddUser(User user);
    Task SaveChanges();
    Task DeleteUserById(int id);
}