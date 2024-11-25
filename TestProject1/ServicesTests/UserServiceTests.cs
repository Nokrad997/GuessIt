using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Exceptions;
using Backend.Repositories.Interfaces;
using Backend.Services;
using Backend.Utility.Interfaces;
using FluentAssertions;
using Moq;

namespace TestProject1.ServicesTests;

[TestFixture]
public class UserServiceTests
{
    private UserService _service;
    private Mock<ITokenUtil> _tokenUtilMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IPasswordAndEmailHasher> _passwordHasherMock;

    [SetUp]
    public void Setup()
    {
        _tokenUtilMock = new Mock<ITokenUtil>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordAndEmailHasher>();
        _service = new UserService(_tokenUtilMock.Object, _userRepositoryMock.Object, _passwordHasherMock.Object);
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllUsers()
    {
        // Arrange
        var users = CreateUserList();
        _userRepositoryMock.Setup(repo => repo.GetAllUsers()).ReturnsAsync(users);

        // Act
        var result = await _service.Retrieve();

        // Assert
        result.Should().BeEquivalentTo(users.Select(user => user.ConvertToDto()));
    }

    [Test]
    public void Retrieve_ShouldThrowException_WhenUserNotFoundById()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetUserById(It.IsAny<int>())).ReturnsAsync((User)null);

        // Act
        Func<Task> result = async () => await _service.Retrieve(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("No user with provided id");
    }

    [Test]
    public async Task Retrieve_ShouldReturnUserDto_WhenUserExistsById()
    {
        // Arrange
        var user = CreateUser();
        _userRepositoryMock.Setup(repo => repo.GetUserById(user.UserId)).ReturnsAsync(user);

        // Act
        var result = await _service.Retrieve(user.UserId);

        // Assert
        result.Should().BeEquivalentTo(user.ConvertToDto());
    }

    [Test]
    public void AddUserAsAdmin_ShouldThrowException_WhenEmailAlreadyExists()
    {
        // Arrange
        var userDto = CreateUserDto();
        var existingUser = CreateUser();
        _userRepositoryMock.Setup(repo => repo.GetAllUsers()).ReturnsAsync(new List<User> { existingUser });
        _passwordHasherMock.Setup(hasher => hasher.VerifyEmail(userDto.Email, existingUser.Email)).Returns(true);

        // Act
        Func<Task> result = async () => await _service.AddUserAsAdmin(userDto);

        // Assert
        result.Should().ThrowAsync<BadCredentialsException>().WithMessage("User with provided email already exists");
    }

    [Test]
    public async Task AddUserAsAdmin_ShouldHashPasswordAndEmail_WhenAddingNewUser()
    {
        // Arrange
        var userDto = CreateUserDto();
        _userRepositoryMock.Setup(repo => repo.GetAllUsers()).ReturnsAsync(new List<User>());
        _passwordHasherMock.Setup(hasher => hasher.HashPassword(userDto.Password)).Returns("hashedPassword");
        _passwordHasherMock.Setup(hasher => hasher.HashEmail(userDto.Email)).Returns("hashedEmail");

        // Act
        await _service.AddUserAsAdmin(userDto);

        // Assert
        _userRepositoryMock.Verify(repo => repo.AddUser(It.Is<User>(u =>
            u.Password == "hashedPassword" && u.Email == "hashedEmail")), Times.Once);
    }

    [Test]
    public void EditUserAsUser_ShouldThrowException_WhenUserTriesToEditAnotherUser()
    {
        // Arrange
        int userId = 1;
        string token = "userToken";
        _tokenUtilMock.Setup(t => t.GetIdFromToken(token)).Returns(2); // Different user ID than `userId`

        // Act
        Func<Task> result = async () => await _service.EditUserAsUser(userId, new EditUserDto(), token);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Denied, cannot edit other user data");
    }

    [Test]
    public async Task EditUserAsUser_ShouldUpdateUserProperties_WhenValid()
    {
        // Arrange
        int userId = 1;
        var user = CreateUser();
        var userDto = CreateEditUserDto();
        string token = "userToken";
        _tokenUtilMock.Setup(t => t.GetIdFromToken(token)).Returns(userId);
        _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
        _userRepositoryMock.Setup(repo => repo.GetAllUsers()).ReturnsAsync(new List<User> { user });
        _passwordHasherMock.Setup(phm => phm.HashEmail(It.IsAny<string>())).Returns(userDto.Email);

        // Act
        var result = await _service.EditUserAsUser(userId, userDto, token);

        // Assert
        _userRepositoryMock.Verify(repo => repo.EditUser(user), Times.Once);
        result.Email.Should().Be(userDto.Email);
    }

    [Test]
    public void DeleteUser_ShouldThrowException_WhenUserToDeleteNotFound()
    {
        // Arrange
        _userRepositoryMock.Setup(repo => repo.GetUserById(It.IsAny<int>())).ReturnsAsync((User)null);

        // Act
        Func<Task> result = async () => await _service.DeleteUser(1, "validToken");

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("User doesn't exist");
    }

    [Test]
    public async Task DeleteUser_ShouldAllowSelfDeleteOrAdminDelete()
    {
        // Arrange
        int userId = 1;
        var user = CreateUser();
        string token = "adminToken";

        _tokenUtilMock.Setup(t => t.GetIdFromToken(token)).Returns(userId);
        _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
        _userRepositoryMock.Setup(repo => repo.DeleteUserById(userId)).Returns(Task.CompletedTask);

        // Act
        await _service.DeleteUser(userId, token);

        // Assert
        _userRepositoryMock.Verify(repo => repo.DeleteUserById(userId), Times.Once);
    }

    #region Helper Methods

    private User CreateUser() =>
        new User
        {
            UserId = 1,
            Email = "user@example.com",
            Password = "password",
            IsAdmin = false,
            Verified = true
        };

    private UserDto CreateUserDto() =>
        new UserDto
        {
            Email = "user@example.com",
            Password = "password"
        };

    private EditUserDto CreateEditUserDto() =>
        new EditUserDto
        {
            Email = "new_email@example.com"
        };

    private List<User> CreateUserList() =>
        new List<User>
        {
            new User { UserId = 1, Email = "user1@example.com" },
            new User { UserId = 2, Email = "user2@example.com" }
        };

    #endregion
}