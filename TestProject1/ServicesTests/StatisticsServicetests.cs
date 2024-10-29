using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services;
using Backend.Utility.Interfaces;
using FluentAssertions;
using Moq;

namespace TestProject1.ServicesTests;

[TestFixture]
public class StatisticsServiceTests
{
    private StatisticsService _service;
    private Mock<IStatisticsRepository> _statisticsRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ITokenUtil> _tokenUtilMock;

    [SetUp]
    public void Setup()
    {
        _statisticsRepositoryMock = new Mock<IStatisticsRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenUtilMock = new Mock<ITokenUtil>();
        _service = new StatisticsService(_statisticsRepositoryMock.Object, _userRepositoryMock.Object, _tokenUtilMock.Object);
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllStatistics()
    {
        // Arrange
        var statisticsList = CreateStatisticsList();
        _statisticsRepositoryMock.Setup(repo => repo.GetStatistics()).ReturnsAsync(statisticsList);

        // Act
        var result = await _service.Retrieve();

        // Assert
        result.Should().BeEquivalentTo(statisticsList.Select(s => s.ConvertToDto()));
    }

    [Test]
    public void Retrieve_ShouldThrowException_WhenStatisticsNotFound()
    {
        // Arrange
        _statisticsRepositoryMock.Setup(repo => repo.GetStatisticsById(It.IsAny<int>())).ReturnsAsync((Statistics)null);

        // Act
        Func<Task> result = async () => await _service.Retrieve(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Statistics not found");
    }

    [Test]
    public async Task GetUserStats_ShouldReturnEmptyStatistics_WhenUserStatsNotFound()
    {
        // Arrange
        string token = "validToken";
        int userId = 1;
        _tokenUtilMock.Setup(t => t.GetIdFromToken(token)).Returns(userId);
        _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(new User { UserId = userId });
        _statisticsRepositoryMock.Setup(repo => repo.GetStatisticsByUserId(userId)).ReturnsAsync((Statistics)null);

        // Act
        var result = await _service.GetUserStats(token);

        // Assert
        result.Should().BeEquivalentTo(new StatisticsDto
        {
            AverageScore = 0,
            HighestScore = 0,
            LowestTimeInSeconds = 0,
            TotalGames = 0,
            TotalPoints = 0,
            TotalTraveledDistanceInMeters = 0
        });
    }

    [Test]
    public async Task AddStatistics_ShouldAddStatistics_WhenValid()
    {
        // Arrange
        var statisticsDto = CreateStatisticsDto();
        string token = "adminToken";
        int userId = 1;
        _tokenUtilMock.Setup(t => t.GetIdFromToken(token)).Returns(userId);
        _tokenUtilMock.Setup(t => t.GetRoleFromToken(token)).Returns("Admin");
        _userRepositoryMock.Setup(repo => repo.GetUserById(statisticsDto.UserIdFk)).ReturnsAsync(new User { UserId = statisticsDto.UserIdFk });
        _statisticsRepositoryMock.Setup(repo => repo.GetStatisticsByUserId(statisticsDto.UserIdFk)).ReturnsAsync((Statistics)null);

        // Act
        await _service.AddStatistics(statisticsDto, token);

        // Assert
        _statisticsRepositoryMock.Verify(repo => repo.AddStatistics(It.Is<Statistics>(s =>
            s.UserIdFk == statisticsDto.UserIdFk)), Times.Once);
    }

    [Test]
    public void EditStatistics_ShouldThrowException_WhenStatisticsNotFound()
    {
        // Arrange
        var editStatisticsDto = CreateEditStatisticsDto();
        string token = "validToken";
        int userId = 1;
        _tokenUtilMock.Setup(t => t.GetIdFromToken(token)).Returns(userId);
        _statisticsRepositoryMock.Setup(repo => repo.GetStatisticsById(It.IsAny<int>())).ReturnsAsync((Statistics)null);

        // Act
        Func<Task> result = async () => await _service.EditStatistics(userId, editStatisticsDto, token);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Statistics not found");
    }

    [Test]
    public async Task EditStatistics_ShouldUpdateStatistics_WhenValid()
    {
        // Arrange
        var existingStatistics = CreateStatisticsEntry();
        var editStatisticsDto = CreateEditStatisticsDto();
        string token = "userToken";
        int userId = existingStatistics.UserIdFk;
        _tokenUtilMock.Setup(t => t.GetIdFromToken(token)).Returns(userId);
        _statisticsRepositoryMock.Setup(repo => repo.GetStatisticsById(existingStatistics.StatisticId)).ReturnsAsync(existingStatistics);
        _userRepositoryMock.Setup(repo => repo.GetUserById(editStatisticsDto.UserIdFk)).ReturnsAsync(new User { UserId = editStatisticsDto.UserIdFk });

        // Act
        var result = await _service.EditStatistics(existingStatistics.StatisticId, editStatisticsDto, token);

        // Assert
        _statisticsRepositoryMock.Verify(repo => repo.EditStatistics(It.Is<Statistics>(s =>
            s.StatisticId == existingStatistics.StatisticId && s.TotalPoints == editStatisticsDto.TotalPoints)), Times.Once);
        result.TotalPoints.Should().Be(editStatisticsDto.TotalPoints);
    }

    [Test]
    public async Task DeleteStatistics_ShouldDeleteStatistics_WhenStatisticsExist()
    {
        // Arrange
        var statistics = CreateStatisticsEntry();
        _statisticsRepositoryMock.Setup(repo => repo.GetStatisticsById(statistics.StatisticId)).ReturnsAsync(statistics);

        // Act
        await _service.DeleteStatistics(statistics.StatisticId);

        // Assert
        _statisticsRepositoryMock.Verify(repo => repo.DeleteStatistics(statistics), Times.Once);
    }

    [Test]
    public void DeleteStatistics_ShouldThrowException_WhenStatisticsNotFound()
    {
        // Arrange
        _statisticsRepositoryMock.Setup(repo => repo.GetStatisticsById(It.IsAny<int>())).ReturnsAsync((Statistics)null);

        // Act
        Func<Task> result = async () => await _service.DeleteStatistics(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Statistics not found");
    }

    #region Helper Methods

    private Statistics CreateStatisticsEntry() =>
        new Statistics
        {
            StatisticId = 1,
            UserIdFk = 1,
            TotalPoints = 1000,
            AverageScore = 500,
            HighestScore = 800,
            LowestTimeInSeconds = 30,
            TotalGames = 10,
            TotalTraveledDistanceInMeters = 15000
        };

    private StatisticsDto CreateStatisticsDto() =>
        new StatisticsDto()
        {
            UserIdFk = 1,
            TotalPoints = 1000,
            AverageScore = 500,
            HighestScore = 800,
            LowestTimeInSeconds = 30,
            TotalGames = 10,
            TotalTraveledDistanceInMeters = 15000
        };

    private EditStatisticsDto CreateEditStatisticsDto() =>
        new EditStatisticsDto()
        {
            UserIdFk = 1,
            TotalPoints = 2000
        };

    private List<Statistics> CreateStatisticsList() =>
        new List<Statistics>
        {
            new Statistics() { StatisticId = 1, UserIdFk = 1, TotalPoints = 1000, AverageScore = 500 },
            new Statistics { StatisticId = 2, UserIdFk = 2, TotalPoints = 800, AverageScore = 400 }
        };

    #endregion
}