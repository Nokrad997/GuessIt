using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Context;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Exceptions;
using Backend.Repositories;
using Backend.Services;
using Backend.Utility.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace TestProject1.IntegrationTests;

[TestFixture]
public class UserServiceIntegrationTests
{
    private UserService _userService;
    private UserRepository _userRepository;
    private Mock<ITokenUtil> _tokenUtilMock;
    private Mock<IPasswordAndEmailHasher> _passwordAndEmailHasherMock;
    private GuessItContext _context;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<GuessItContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unikalna baza dla każdego testu
            .Options;

        _context = new GuessItContext(options);
        _userRepository = new UserRepository(_context);
        _tokenUtilMock = new Mock<ITokenUtil>();
        _passwordAndEmailHasherMock = new Mock<IPasswordAndEmailHasher>();

        _userService = new UserService(_tokenUtilMock.Object, _userRepository, _passwordAndEmailHasherMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddUserAsAdmin_ShouldAddUser_WhenDataIsValid()
    {
        // Arrange
        var userDto = new UserDto { Username = "NewUser", Email = "test@example.com", Password = "password123" };
        _passwordAndEmailHasherMock.Setup(p => p.HashPassword(userDto.Password)).Returns("hashedPassword");
        _passwordAndEmailHasherMock.Setup(p => p.HashEmail(userDto.Email)).Returns("hashedEmail");

        // Act
        await _userService.AddUserAsAdmin(userDto);

        // Assert
        var users = await _userRepository.GetAllUsers();
        users.Should().ContainSingle(u => u.Username == "NewUser" && u.Email == "hashedEmail");
    }

    [Test]
    public async Task AddUserAsAdmin_ShouldThrowException_WhenEmailAlreadyExists()
    {
        // Arrange
        var existingUser = await AddUser("ExistingUser", "existing@example.com", "password123");
        var userDto = new UserDto { Username = "NewUser", Email = "existing@example.com", Password = "password123" };
        _passwordAndEmailHasherMock.Setup(p => p.VerifyEmail(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        // Act
        Func<Task> act = async () => await _userService.AddUserAsAdmin(userDto);

        // Assert
        await act.Should().ThrowAsync<BadCredentialsException>().WithMessage("User with provided email already exists");
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllUsers()
    {
        // Arrange
        await AddUser("User1", "user1@example.com", "password1");
        await AddUser("User2", "user2@example.com", "password2");

        // Act
        var result = await _userService.Retrieve();

        // Assert
        result.Should().HaveCount(2);
        result.Select(u => u.Username).Should().Contain(new[] { "User1", "User2" });
    }

    [Test]
    public async Task Retrieve_ShouldReturnUser_WhenIdIsValid()
    {
        // Arrange
        var userId = await AddUser("TestUser", "test@example.com", "password123");

        // Act
        var result = await _userService.Retrieve(userId);

        // Assert
        result.Should().NotBeNull();
        result.Username.Should().Be("TestUser");
    }

    [Test]
    public async Task EditUserAsUser_ShouldUpdateUser_WhenDataIsValidAndUserOwnsAccount()
    {
        // Arrange
        var userId = await AddUser("TestUser", "test@example.com", "password123");
        var editDto = new EditUserDto { Username = "UpdatedUser", Email = "updated@example.com" };

        _tokenUtilMock.Setup(t => t.GetIdFromToken(It.IsAny<string>())).Returns(userId);
        _passwordAndEmailHasherMock.Setup(p => p.HashEmail(editDto.Email)).Returns("hashedEmail");

        // Act
        var result = await _userService.EditUserAsUser(userId, editDto, "validToken");

        // Assert
        result.Username.Should().Be("UpdatedUser");
        var updatedUser = await _userRepository.GetUserById(userId);
        updatedUser.Email.Should().Be("hashedEmail");
    }

    [Test]
    public async Task EditUserAsUser_ShouldThrowException_WhenUserTriesToEditAnotherUser()
    {
        // Arrange
        var userId = await AddUser("TestUser", "test@example.com", "password123");
        var otherUserId = await AddUser("OtherUser", "other@example.com", "password456");
        var editDto = new EditUserDto { Username = "UpdatedUser", Email = "updated@example.com" };

        _tokenUtilMock.Setup(t => t.GetIdFromToken(It.IsAny<string>())).Returns(otherUserId);

        // Act
        Func<Task> act = async () => await _userService.EditUserAsUser(userId, editDto, "validToken");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Denied, cannot edit other user data");
    }

    [Test]
    public async Task DeleteUser_ShouldRemoveUser_WhenUserIsAdmin()
    {
        // Arrange
        var adminId = await AddUser("AdminUser", "admin@example.com", "password123", isAdmin: true);
        var userId = await AddUser("TestUser", "test@example.com", "password123");

        _tokenUtilMock.Setup(t => t.GetIdFromToken(It.IsAny<string>())).Returns(adminId);

        // Act
        await _userService.DeleteUser(userId, "adminToken");

        // Assert
        var user = await _userRepository.GetUserById(userId);
        user.Should().BeNull();
    }

    [Test]
    public async Task DeleteUser_ShouldThrowException_WhenUserTriesToDeleteAnotherUserWithoutPermission()
    {
        // Arrange
        var userId = await AddUser("TestUser", "test@example.com", "password123");
        var otherUserId = await AddUser("OtherUser", "other@example.com", "password456");

        _tokenUtilMock.Setup(t => t.GetIdFromToken(It.IsAny<string>())).Returns(otherUserId);

        // Act
        Func<Task> act = async () => await _userService.DeleteUser(userId, "userToken");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Denied, cannot delete other user accounts");
    }

    // Helper Methods

    private async Task<int> AddUser(string username, string email, string password, bool isAdmin = false)
    {
        var user = new User { Username = username, Email = email, Password = password, IsAdmin = isAdmin };
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return user.UserId;
    }
}
