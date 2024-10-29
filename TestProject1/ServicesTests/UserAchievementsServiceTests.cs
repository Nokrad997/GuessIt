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
public class UserAchievementsServiceTests
{
    private UserAchievementsService _service;
    private Mock<IUserAchievementsRepository> _userAchievementsRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IAchievementRepository> _achievementRepositoryMock;
    private Mock<ITokenUtil> _tokenUtilMock;

    [SetUp]
    public void Setup()
    {
        _userAchievementsRepositoryMock = new Mock<IUserAchievementsRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _achievementRepositoryMock = new Mock<IAchievementRepository>();
        _tokenUtilMock = new Mock<ITokenUtil>();
        _service = new UserAchievementsService(
            _userAchievementsRepositoryMock.Object,
            _userRepositoryMock.Object,
            _achievementRepositoryMock.Object,
            _tokenUtilMock.Object
        );
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllUserAchievements()
    {
        // Arrange
        var userAchievements = CreateUserAchievementsList();
        _userAchievementsRepositoryMock.Setup(repo => repo.GetUserAchievements()).ReturnsAsync(userAchievements);

        // Act
        var result = await _service.Retrieve();

        // Assert
        result.Should().BeEquivalentTo(userAchievements.Select(ua => ua.ConvertToDto()));
    }

    [Test]
    public void Retrieve_ShouldThrowException_WhenUserAchievementNotFound()
    {
        // Arrange
        _userAchievementsRepositoryMock.Setup(repo => repo.GetUserAchievementsById(It.IsAny<int>())).ReturnsAsync((UserAchievements)null);

        // Act
        Func<Task> result = async () => await _service.Retrieve(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("UserAchievements not found");
    }

    [Test]
    public async Task RetrieveUserAchievements_ShouldReturnAchievementsForUser_WhenTokenIsValid()
    {
        // Arrange
        string token = "validToken";
        int userId = 1;
        var user = new User { UserId = userId };
        var userAchievements = CreateUserAchievementsList();
        var achievementIds = userAchievements.Select(ua => ua.AchievementIdFk).ToList();
        var achievements = CreateAchievementsList();

        _tokenUtilMock.Setup(t => t.GetIdFromToken(token)).Returns(userId);
        _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(user);
        _userAchievementsRepositoryMock.Setup(repo => repo.GetUserAchievementsByUserId(user)).ReturnsAsync(userAchievements);
        _achievementRepositoryMock.Setup(repo => repo.GetAchievementsByIds(achievementIds)).ReturnsAsync(achievements);

        // Act
        var result = await _service.RetrieveUserAchievements(token);

        // Assert
        result.Should().BeEquivalentTo(achievements.Select(a => a.ConvertToDto()));
    }

    [Test]
    public void AddUserAchievement_ShouldThrowException_WhenAchievementAlreadyExistsForUser()
    {
        // Arrange
        var userAchievementDto = CreateUserAchievementDto();
        _userAchievementsRepositoryMock.Setup(repo => repo.GetUserAchievementsById(userAchievementDto.UserAchievementId))
            .ReturnsAsync(new UserAchievements());

        // Act
        Func<Task> result = async () => await _service.AddUserAchievement(userAchievementDto);

        // Assert
        result.Should().ThrowAsync<EntryAlreadyExistsException>();
    }

    [Test]
    public async Task AddUserAchievement_ShouldAddUserAchievement_WhenValid()
    {
        // Arrange
        var userAchievementDto = CreateUserAchievementDto();
        _userAchievementsRepositoryMock.Setup(repo => repo.GetUserAchievementsById(userAchievementDto.UserAchievementId)).ReturnsAsync((UserAchievements)null);
        _userRepositoryMock.Setup(repo => repo.GetUserById(userAchievementDto.UserIdFk)).ReturnsAsync(new User { UserId = userAchievementDto.UserIdFk });
        _achievementRepositoryMock.Setup(repo => repo.GetAchievementById(userAchievementDto.AchievementIdFk)).ReturnsAsync(new Achievement { AchievementId = userAchievementDto.AchievementIdFk });

        // Act
        await _service.AddUserAchievement(userAchievementDto);

        // Assert
        _userAchievementsRepositoryMock.Verify(repo => repo.AddUserAchievements(It.Is<UserAchievements>(ua =>
            ua.UserIdFk == userAchievementDto.UserIdFk && ua.AchievementIdFk == userAchievementDto.AchievementIdFk)), Times.Once);
    }

    [Test]
    public void EditUserAchievement_ShouldThrowException_WhenUserAchievementNotFound()
    {
        // Arrange
        var editUserAchievementDto = CreateEditUserAchievementDto();
        _userAchievementsRepositoryMock.Setup(repo => repo.GetUserAchievementsById(It.IsAny<int>())).ReturnsAsync((UserAchievements)null);

        // Act
        Func<Task> result = async () => await _service.EditUserAchievement(1, editUserAchievementDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("UserAchievement not found");
    }

    [Test]
    public async Task EditUserAchievement_ShouldUpdateUserAchievement_WhenValid()
    {
        // Arrange
        var existingUserAchievement = CreateUserAchievement();
        var editUserAchievementDto = CreateEditUserAchievementDto();
        _userAchievementsRepositoryMock.Setup(repo => repo.GetUserAchievementsById(existingUserAchievement.UserAchievementId)).ReturnsAsync(existingUserAchievement);
        _userRepositoryMock.Setup(repo => repo.GetUserById(editUserAchievementDto.UserIdFk)).ReturnsAsync(new User { UserId = editUserAchievementDto.UserIdFk });
        _achievementRepositoryMock.Setup(repo => repo.GetAchievementById(editUserAchievementDto.AchievementIdFk)).ReturnsAsync(new Achievement { AchievementId = editUserAchievementDto.AchievementIdFk });

        // Act
        var result = await _service.EditUserAchievement(existingUserAchievement.UserAchievementId, editUserAchievementDto);

        // Assert
        _userAchievementsRepositoryMock.Verify(repo => repo.EditUserAchievements(It.Is<UserAchievements>(ua =>
            ua.UserAchievementId == existingUserAchievement.UserAchievementId && ua.AchievementIdFk == editUserAchievementDto.AchievementIdFk)), Times.Once);
        result.AchievementIdFk.Should().Be(editUserAchievementDto.AchievementIdFk);
    }

    [Test]
    public async Task DeleteUserAchievement_ShouldDeleteUserAchievement_WhenItExists()
    {
        // Arrange
        var userAchievement = CreateUserAchievement();
        _userAchievementsRepositoryMock.Setup(repo => repo.GetUserAchievementsById(userAchievement.UserAchievementId)).ReturnsAsync(userAchievement);

        // Act
        await _service.DeleteUserAchievement(userAchievement.UserAchievementId);

        // Assert
        _userAchievementsRepositoryMock.Verify(repo => repo.DeleteUserAchievements(userAchievement), Times.Once);
    }

    [Test]
    public void DeleteUserAchievement_ShouldThrowException_WhenUserAchievementNotFound()
    {
        // Arrange
        _userAchievementsRepositoryMock.Setup(repo => repo.GetUserAchievementsById(It.IsAny<int>())).ReturnsAsync((UserAchievements)null);

        // Act
        Func<Task> result = async () => await _service.DeleteUserAchievement(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("UserAchievement not found");
    }

    #region Helper Methods

    private UserAchievements CreateUserAchievement() =>
        new UserAchievements
        {
            UserAchievementId = 1,
            UserIdFk = 1,
            AchievementIdFk = 1
        };

    private UserAchievementsDtos CreateUserAchievementDto() =>
        new UserAchievementsDtos()
        {
            UserAchievementId = 1,
            UserIdFk = 1,
            AchievementIdFk = 1
        };

    private EditUserAchievementDtos CreateEditUserAchievementDto() =>
        new EditUserAchievementDtos()
        {
            UserIdFk = 1,
            AchievementIdFk = 2
        };

    private List<UserAchievements> CreateUserAchievementsList() =>
        new List<UserAchievements>
        {
            new UserAchievements { UserAchievementId = 1, UserIdFk = 1, AchievementIdFk = 1 },
            new UserAchievements { UserAchievementId = 2, UserIdFk = 2, AchievementIdFk = 2 }
        };

    private List<Achievement> CreateAchievementsList() =>
        new List<Achievement>
        {
            new Achievement { AchievementId = 1 },
            new Achievement() { AchievementId = 2 }
        };

    #endregion
}