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
public class UserAchievementsServiceIntegrationTests
{
    private UserAchievementsService _userAchievementsService;
    private UserAchievementsRepository _userAchievementsRepository;
    private UserRepository _userRepository;
    private AchievementRepository _achievementRepository;
    private Mock<ITokenUtil> _tokenUtilMock;
    private GuessItContext _context;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<GuessItContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new GuessItContext(options);
        _userAchievementsRepository = new UserAchievementsRepository(_context);
        _userRepository = new UserRepository(_context);
        _achievementRepository = new AchievementRepository(_context);
        _tokenUtilMock = new Mock<ITokenUtil>();

        _userAchievementsService = new UserAchievementsService(
            _userAchievementsRepository,
            _userRepository,
            _achievementRepository,
            _tokenUtilMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddUserAchievement_ShouldAddAchievement_WhenDataIsValid()
    {
        // Arrange
        var userId = await AddUser("TestUser");
        var achievementId = await AddAchievement("TestAchievement");
        var userAchievementsDto = new UserAchievementsDtos { UserIdFk = userId, AchievementIdFk = achievementId };

        // Act
        await _userAchievementsService.AddUserAchievement(userAchievementsDto);

        // Assert
        var userAchievements = await _userAchievementsRepository.GetUserAchievementsByUserId(_userRepository.GetUserById(userId).GetAwaiter().GetResult());
        userAchievements.Should().ContainSingle(ua => ua.AchievementIdFk == achievementId && ua.UserIdFk == userId);
    }

    [Test]
    public async Task RetrieveUserAchievements_ShouldReturnAchievements_WhenTokenIsValid()
    {
        // Arrange
        var userId = await AddUser("TestUser");
        var achievementId = await AddAchievement("TestAchievement");
        await AddUserAchievement(userId, achievementId);
        _tokenUtilMock.Setup(t => t.GetIdFromToken(It.IsAny<string>())).Returns(userId);

        // Act
        var result = await _userAchievementsService.RetrieveUserAchievements("validToken");

        // Assert
        result.Should().HaveCount(1);
        result.First().AchievementId.Should().Be(achievementId);
    }

    [Test]
    public async Task EditUserAchievement_ShouldUpdateAchievement_WhenDataIsValid()
    {
        // Arrange
        var userId = await AddUser("TestUser");
        var achievementId1 = await AddAchievement("TestAchievement1");
        var achievementId2 = await AddAchievement("TestAchievement2");
        var userAchievementId = await AddUserAchievement(userId, achievementId1);

        var editDto = new EditUserAchievementDtos { UserIdFk = userId, AchievementIdFk = achievementId2 };

        // Act
        var result = await _userAchievementsService.EditUserAchievement(userAchievementId, editDto);

        // Assert
        result.AchievementIdFk.Should().Be(achievementId2);
        var updatedUserAchievement = await _userAchievementsRepository.GetUserAchievementsById(userAchievementId);
        updatedUserAchievement.AchievementIdFk.Should().Be(achievementId2);
    }

    [Test]
    public async Task DeleteUserAchievement_ShouldRemoveAchievement_WhenAchievementExists()
    {
        // Arrange
        var userId = await AddUser("TestUser");
        var achievementId = await AddAchievement("TestAchievement");
        var userAchievementId = await AddUserAchievement(userId, achievementId);

        // Act
        await _userAchievementsService.DeleteUserAchievement(userAchievementId);

        // Assert
        var userAchievement = await _userAchievementsRepository.GetUserAchievementsById(userAchievementId);
        userAchievement.Should().BeNull();
    }

    // Helper Methods

    private async Task<int> AddUser(string username)
    {
        var user = new User { Username = username, Email = "test@test.com", Password = "testtest"};
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return user.UserId;
    }

    private async Task<int> AddAchievement(string achievementName)
    {
        var achievement = new Achievement { AchievementName = "Achievement1", AchievementDescription = "test", AchievementCriteria = new Dictionary<string, object>
        {
            ["test1"] = "test1",
            ["test2"] = "test2"
        } };
        _context.Achievement.Add(achievement);
        await _context.SaveChangesAsync();
        return achievement.AchievementId;
    }

    private async Task<int> AddUserAchievement(int userId, int achievementId)
    {
        var userAchievement = new UserAchievements { UserIdFk = userId, AchievementIdFk = achievementId };
        _context.UserAchievements.Add(userAchievement);
        await _context.SaveChangesAsync();
        return userAchievement.UserAchievementId;
    }
}
