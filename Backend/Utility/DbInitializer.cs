using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Utility;

public class DbInitializer
{
    private readonly ModelBuilder _modelBuilder;
    private readonly PasswordHasher _passwordHasher;

    public DbInitializer(ModelBuilder modelBuilder, PasswordHasher passwordHasher)
    {
        _modelBuilder = modelBuilder;
        _passwordHasher = passwordHasher;
    }

    public void seed()
    {
        _modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = -1,
                Username = "admin",
                Email = "admin@admin.com",
                Password = _passwordHasher.HashPassword("admin"),
                IsAdmin = true,
                Verified = true
            },
            new User
            {
                UserId = -2,
                Username = "user",
                Email = "user@user.com",
                Password = _passwordHasher.HashPassword("user"),
                IsAdmin = false,
                Verified = true
            }
            );
    }
}