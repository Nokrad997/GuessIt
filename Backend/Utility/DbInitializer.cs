using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Utility;

public class DbInitializer
{
    private readonly ModelBuilder _modelBuilder;
    private readonly PasswordAndEmailHasher _passwordAndEmailHasher;

    public DbInitializer(ModelBuilder modelBuilder, PasswordAndEmailHasher passwordAndEmailHasher)
    {
        _modelBuilder = modelBuilder;
        _passwordAndEmailHasher = passwordAndEmailHasher;
    }

    public void seed()
    {
        _modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = -1,
                Username = "admin",
                Email = _passwordAndEmailHasher.HashEmail("admin@admin.com"),
                Password = _passwordAndEmailHasher.HashPassword("admin"),
                IsAdmin = true,
                Verified = true
            },
            new User
            {
                UserId = -2,
                Username = "user",
                Email = _passwordAndEmailHasher.HashEmail("user@user.com"),
                Password = _passwordAndEmailHasher.HashPassword("user"),
                IsAdmin = false,
                Verified = true
            }
            );
    }
}