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
public class AuthServiceTests
{
    private AuthService _service;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IPasswordAndEmailHasher> _passwordAndEmailHasherMock;
    private Mock<ITokenUtil> _tokenUtilMock;

    [SetUp]
    public void Setup()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordAndEmailHasherMock = new Mock<IPasswordAndEmailHasher>();
        _tokenUtilMock = new Mock<ITokenUtil>();
        _service = new AuthService(_userRepositoryMock.Object, _passwordAndEmailHasherMock.Object, _tokenUtilMock.Object);
    }

    [Test]
    public void RegisterUser_ShouldThrowException_WhenUserWithEmailAlreadyExists()
    {
        // Arrange
        var registerUserDto = CreateRegisterUserDto();
        var existingUser = CreateExistingUser();
        SetupUserExistsByEmail(registerUserDto.Email, existingUser);

        // Act
        Func<Task> result = async () => await _service.RegisterUser(registerUserDto);

        // Assert
        result.Should().ThrowAsync<BadCredentialsException>().WithMessage("User with provided email already exists");
    }

    [Test]
    public async Task RegisterUser_ShouldRegisterUser_WhenEmailIsUnique()
    {
        // Arrange
        var registerUserDto = CreateRegisterUserDto();
        SetupUserNotExistsByEmail(registerUserDto.Email);
        SetupHashingForRegisterUser(registerUserDto);

        // Act
        await _service.RegisterUser(registerUserDto);

        // Assert
        _userRepositoryMock.Verify(repo => repo.AddUser(It.Is<User>(user => 
            user.Email == "hashedEmail" && user.Password == "hashedPassword")), Times.Once);
    }

    [Test]
    public void LoginUser_ShouldThrowException_WhenEmailDoesNotExist()
    {
        // Arrange
        var authUserDto = CreateAuthUserDto();
        SetupUserNotExistsByEmail(authUserDto.Email);

        // Act
        Func<Task> result = async () => await _service.LoginUser(authUserDto);

        // Assert
        result.Should().ThrowAsync<BadCredentialsException>().WithMessage("Wrong email or password");
    }

    [Test]
    public void LoginUser_ShouldThrowException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var authUserDto = CreateAuthUserDto();
        var existingUser = CreateExistingUser();
        SetupUserExistsByEmail(authUserDto.Email, existingUser);
        SetupPasswordVerification(authUserDto.Password, existingUser.Password, false);

        // Act
        Func<Task> result = async () => await _service.LoginUser(authUserDto);

        // Assert
        result.Should().ThrowAsync<BadCredentialsException>().WithMessage("Wrong email or password");
    }

    [Test]
    public async Task LoginUser_ShouldReturnTokens_WhenCredentialsAreCorrect()
    {
        // Arrange
        var authUserDto = CreateAuthUserDto();
        var existingUser = CreateExistingUser();
        SetupUserExistsByEmail(authUserDto.Email, existingUser);
        SetupPasswordVerification(authUserDto.Password, existingUser.Password, true);
        SetupTokenCreation(existingUser);

        // Act
        var result = await _service.LoginUser(authUserDto);

        // Assert
        result.AccessToken.Should().Be("accessToken");
        result.RefreshToken.Should().Be("refreshToken");
    }

    private EditUserDto CreateRegisterUserDto() =>
        new EditUserDto { Email = "test@example.com", Password = "password" };

    private AuthUserDto CreateAuthUserDto() =>
        new AuthUserDto { Email = "test@example.com", Password = "correctPassword" };

    private User CreateExistingUser() =>
        new User { Email = "hashedEmail", Password = "correctPassword" };

    private void SetupUserExistsByEmail(string email, User user)
    {
        _userRepositoryMock.Setup(repo => repo.GetAllUsers())
            .ReturnsAsync(new List<User> { user });
        _passwordAndEmailHasherMock.Setup(hasher => hasher.VerifyEmail(email, user.Email))
            .Returns(true);
    }

    private void SetupUserNotExistsByEmail(string email)
    {
        _userRepositoryMock.Setup(repo => repo.GetAllUsers())
            .ReturnsAsync(new List<User>());
    }

    private void SetupHashingForRegisterUser(EditUserDto registerUserDto)
    {
        _passwordAndEmailHasherMock.Setup(hasher => hasher.HashEmail(registerUserDto.Email))
            .Returns("hashedEmail");
        _passwordAndEmailHasherMock.Setup(hasher => hasher.HashPassword(registerUserDto.Password))
            .Returns("hashedPassword");
    }

    private void SetupPasswordVerification(string providedPassword, string existingPassword, bool result)
    {
        _passwordAndEmailHasherMock.Setup(hasher => hasher.VerifyPassword(providedPassword, existingPassword))
            .Returns(result);
    }

    private void SetupTokenCreation(User user)
    {
        _tokenUtilMock.Setup(tokenUtil => tokenUtil.CreateTokenPair(user))
            .Returns(new Dictionary<string, string> { { "access", "accessToken" }, { "refresh", "refreshToken" } });
    }
}
