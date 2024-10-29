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

namespace TestProject1.IntegrationTests;

[TestFixture]
public class AuthServiceIntegrationTests
{
    private AuthService _authService;
    private UserRepository _userRepository;
    private Mock<IPasswordAndEmailHasher> _passwordAndEmailHasherMock;
    private Mock<ITokenUtil> _tokenUtilMock;
    private GuessItContext _context;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<GuessItContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new GuessItContext(options);
        _userRepository = new UserRepository(_context);
        _passwordAndEmailHasherMock = new Mock<IPasswordAndEmailHasher>();
        _tokenUtilMock = new Mock<ITokenUtil>();

        _authService = new AuthService(_userRepository, _passwordAndEmailHasherMock.Object, _tokenUtilMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task RegisterUser_ShouldRegisterUser_WhenEmailIsUnique()
    {
        // Arrange
        var registerUserDto = new EditUserDto { Email = "unique@example.com", Username = "test", Password = "password123" };
        
        _passwordAndEmailHasherMock.Setup(hasher => hasher.HashPassword(It.IsAny<string>())).Returns("hashedPassword");
        _passwordAndEmailHasherMock.Setup(hasher => hasher.HashEmail(It.IsAny<string>())).Returns("hashedEmail");

        // Act
        await _authService.RegisterUser(registerUserDto);

        // Assert
        var users = await _userRepository.GetAllUsers();
        users.Should().ContainSingle(user =>
            user.Email == "hashedEmail" && user.Password == "hashedPassword");
    }

    [Test]
    public async Task RegisterUser_ShouldThrowBadCredentialsException_WhenEmailAlreadyExists()
    {
        // Arrange
        var existingUser = new User { Email = "hashedEmail", Username = "test", Password = "testtest"};
        await _userRepository.AddUser(existingUser);

        var registerUserDto = new EditUserDto() { Email = "duplicate@example.com", Password = "password123" };
        _passwordAndEmailHasherMock.Setup(hasher => hasher.VerifyEmail(registerUserDto.Email, existingUser.Email)).Returns(true);

        // Act
        Func<Task> act = async () => await _authService.RegisterUser(registerUserDto);

        // Assert
        await act.Should().ThrowAsync<BadCredentialsException>().WithMessage("User with provided email already exists");
    }

    [Test]
    public async Task LoginUser_ShouldReturnTokens_WhenCredentialsAreCorrect()
    {
        // Arrange
        var existingUser = new User { Email = "hashedEmail", Username = "test", Password = "hashedPassword" };
        await _userRepository.AddUser(existingUser);

        var authUserDto = new AuthUserDto { Email = "user@example.com",  Password = "password123" };

        _passwordAndEmailHasherMock.Setup(hasher => hasher.VerifyEmail(authUserDto.Email, existingUser.Email)).Returns(true);
        _passwordAndEmailHasherMock.Setup(hasher => hasher.VerifyPassword(authUserDto.Password, existingUser.Password)).Returns(true);
        _tokenUtilMock.Setup(tokenUtil => tokenUtil.CreateTokenPair(existingUser)).Returns(new Dictionary<string, string>
        {
            { "access", "accessToken" },
            { "refresh", "refreshToken" }
        });

        // Act
        var result = await _authService.LoginUser(authUserDto);

        // Assert
        result.Should().NotBeNull();
        result.AccessToken.Should().Be("accessToken");
        result.RefreshToken.Should().Be("refreshToken");
    }

    [Test]
    public async Task LoginUser_ShouldThrowBadCredentialsException_WhenEmailIsIncorrect()
    {
        // Arrange
        var authUserDto = new AuthUserDto { Email = "wrong@example.com", Password = "password123" };

        // Act
        Func<Task> act = async () => await _authService.LoginUser(authUserDto);

        // Assert
        await act.Should().ThrowAsync<BadCredentialsException>().WithMessage("Wrong email or password");
    }

    [Test]
    public async Task LoginUser_ShouldThrowBadCredentialsException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var existingUser = new User() { Email = "hashedEmail", Username = "test", Password = "hashedPassword" };
        await _userRepository.AddUser(existingUser);

        var authUserDto = new AuthUserDto() { Email = "user@example.com", Password = "wrongPassword" };

        _passwordAndEmailHasherMock.Setup(hasher => hasher.VerifyEmail(authUserDto.Email, existingUser.Email)).Returns(true);
        _passwordAndEmailHasherMock.Setup(hasher => hasher.VerifyPassword(authUserDto.Password, existingUser.Password)).Returns(false);

        // Act
        Func<Task> act = async () => await _authService.LoginUser(authUserDto);

        // Assert
        await act.Should().ThrowAsync<BadCredentialsException>().WithMessage("Wrong email or password");
    }
}