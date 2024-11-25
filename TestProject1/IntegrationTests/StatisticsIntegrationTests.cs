using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Context;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories;
using Backend.Services;
using Backend.Utility.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace TestProject1.IntegrationTests;

[TestFixture]
public class StatisticsServiceIntegrationTests
{
    private StatisticsService _statisticsService;
    private StatisticsRepository _statisticsRepository;
    private UserRepository _userRepository;
    private Mock<ITokenUtil> _tokenUtilMock;
    private GuessItContext _context;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<GuessItContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new GuessItContext(options);
        _statisticsRepository = new StatisticsRepository(_context);
        _userRepository = new UserRepository(_context);
        _tokenUtilMock = new Mock<ITokenUtil>();

        _statisticsService = new StatisticsService(_statisticsRepository, _userRepository, _tokenUtilMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddStatistics_ShouldAddStatistics_WhenUserExists()
    {
        // Arrange
        var userId = await AddUser("User1");
        var statisticsDto = new StatisticsDto { UserIdFk = userId, TotalPoints = 100, TotalGames = 5 };
        _tokenUtilMock.Setup(t => t.GetIdFromToken(It.IsAny<string>())).Returns(userId);

        // Act
        await _statisticsService.AddStatistics(statisticsDto, "validToken");

        // Assert
        var statistics = await _statisticsRepository.GetStatisticsByUserId(userId);
        statistics.Should().NotBeNull();
        statistics.TotalPoints.Should().Be(100);
    }

    [Test]
    public async Task AddStatistics_ShouldThrowException_WhenStatisticsAlreadyExist()
    {
        // Arrange
        var userId = await AddUser("User1");
        var statisticsDto = new StatisticsDto { UserIdFk = userId, TotalPoints = 100, TotalGames = 5 };
        _tokenUtilMock.Setup(tum => tum.GetIdFromToken(It.IsAny<string>())).Returns(userId);
        await _statisticsService.AddStatistics(statisticsDto, "validToken");

        // Act
        Func<Task> act = async () => await _statisticsService.AddStatistics(statisticsDto, "validToken");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Statistics for user already exists");
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllStatistics()
    {
        // Arrange
        var userId1 = await AddUser("User1");
        var userId2 = await AddUser("User2");
        await AddStatistics(userId1, 100, 5);
        await AddStatistics(userId2, 200, 10);

        // Act
        var result = await _statisticsService.Retrieve();

        // Assert
        result.Should().HaveCount(2);
        result.Select(s => s.TotalPoints).Should().Contain(new[] { 100, 200 });
    }

    [Test]
    public async Task Retrieve_ShouldReturnStatistics_WhenStatisticsExist()
    {
        // Arrange
        var userId = await AddUser("User1");
        var statisticsId = await AddStatistics(userId, 100, 5);

        // Act
        var result = await _statisticsService.Retrieve(statisticsId);

        // Assert
        result.Should().NotBeNull();
        result.TotalPoints.Should().Be(100);
    }

    [Test]
    public async Task EditStatistics_ShouldUpdateStatistics_WhenUserAndStatisticsExist()
    {
        // Arrange
        var userId = await AddUser("User1");
        var statisticsId = await AddStatistics(userId, 100, 5);
        var editDto = new EditStatisticsDto { UserIdFk = userId, TotalPoints = 200, TotalGames = 10 };

        _tokenUtilMock.Setup(t => t.GetIdFromToken(It.IsAny<string>())).Returns(userId);

        // Act
        var result = await _statisticsService.EditStatistics(statisticsId, editDto, "validToken");

        // Assert
        result.TotalPoints.Should().Be(200);
        var updatedStatistics = await _statisticsRepository.GetStatisticsById(statisticsId);
        updatedStatistics.TotalPoints.Should().Be(200);
    }

    [Test]
    public async Task DeleteStatistics_ShouldRemoveStatistics_WhenStatisticsExist()
    {
        // Arrange
        var userId = await AddUser("User1");
        var statisticsId = await AddStatistics(userId, 100, 5);

        // Act
        await _statisticsService.DeleteStatistics(statisticsId);

        // Assert
        var statistics = await _statisticsRepository.GetStatisticsById(statisticsId);
        statistics.Should().BeNull();
    }

    [Test]
    public async Task GetUserStats_ShouldReturnEmptyStats_WhenNoStatisticsExistForUser()
    {
        // Arrange
        var userId = await AddUser("UserWithoutStats");
        _tokenUtilMock.Setup(t => t.GetIdFromToken(It.IsAny<string>())).Returns(userId);

        // Act
        var result = await _statisticsService.GetUserStats("validToken");

        // Assert
        result.Should().NotBeNull();
        result.TotalPoints.Should().Be(0);
        result.TotalGames.Should().Be(0);
    }

    // Helper Methods

    private async Task<int> AddUser(string username)
    {
        var user = new User { Username = username, Email = "test@test.com", Password = "testtest"};
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return user.UserId;
    }

    private async Task<int> AddStatistics(int userId, int totalPoints, int totalGames)
    {
        var statistics = new Statistics
        {
            UserIdFk = userId,
            TotalPoints = totalPoints,
            TotalGames = totalGames,
            AverageScore = totalPoints / totalGames,
            HighestScore = totalPoints,
            LowestTimeInSeconds = 0,
            TotalTraveledDistanceInMeters = 1000
        };
        _context.Statistics.Add(statistics);
        await _context.SaveChangesAsync();
        return statistics.StatisticId;
    }
}
