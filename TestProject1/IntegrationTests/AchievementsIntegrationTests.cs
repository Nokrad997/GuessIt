using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Context;
using Backend.Dtos;
using Backend.Entities;
using Backend.Repositories;
using Backend.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace TestProject1.IntegrationTests;

[TestFixture]
public class AchievementServiceIntegrationTests
{
    private AchievementService _achievementService;
    private AchievementRepository _achievementRepository;
    private GuessItContext _context;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<GuessItContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unikalna baza dla każdego testu
            .Options;

        _context = new GuessItContext(options);
        _achievementRepository = new AchievementRepository(_context);
        _achievementService = new AchievementService(_achievementRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllAchievements()
    {
        // Arrange
        _context.Achievement.AddRange(
            new Achievement
            {
                AchievementName = "Achievement1",
                AchievementDescription = "test",
                AchievementCriteria = new Dictionary<string, object>
                {
                    ["test1"] = "test1",
                    ["test2"] = "test2"
                }
            },
            new Achievement
            {
                AchievementName = "Achievement2",
                AchievementDescription = "test",
                AchievementCriteria = new Dictionary<string, object>
                {
                    ["test1"] = "test1",
                    ["test2"] = "test2"
                }
            }
        );
        await _context.SaveChangesAsync();

        // Act
        var result = await _achievementService.Retrieve();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Test]
    public async Task Retrieve_ById_ShouldReturnAchievement_WhenExists()
    {
        // Arrange
        var achievement = new Achievement
        {
            AchievementName = "Achievement1",
            AchievementDescription = "test",
            AchievementCriteria = new Dictionary<string, object>
            {
                ["test1"] = "test1",
                ["test2"] = "test2"
            }
        };
        _context.Achievement.Add(achievement);
        await _context.SaveChangesAsync();

        // Act
        var result = await _achievementService.Retrieve(achievement.AchievementId);

        // Assert
        result.Should().NotBeNull();
        result.AchievementName.Should().Be("Achievement1");
    }

    [Test]
    public async Task AddAchievement_ShouldAddAchievement_WhenUnique()
    {
        // Arrange
        var dto = new AchievementDto
        {
            AchievementName = "UniqueAchievement",
            AchievementDescription = "test",
            AchievementCriteria = new Dictionary<string, object>
            {
                ["test1"] = "test1",
                ["test2"] = "test2"
            }
        };

        // Act
        await _achievementService.AddAchievement(dto);

        // Assert
        var achievement = await _context.Achievement.FirstOrDefaultAsync(a => a.AchievementName == "UniqueAchievement");
        achievement.Should().NotBeNull();
    }

    [Test]
    public async Task EditAchievement_ShouldUpdateAchievement_WhenUnique()
    {
        // Arrange
        var achievement = new Achievement
        {
            AchievementName = "OriginalName",
            AchievementDescription = "test",
            AchievementCriteria = new Dictionary<string, object>
            {
                ["test1"] = "test1",
                ["test2"] = "test2"
            }
        };
        _context.Achievement.Add(achievement);
        await _context.SaveChangesAsync();

        var editDto = new EditAchievementDto
        {
            AchievementName = "UpdatedName",
            AchievementDescription = "test",
            AchievementCriteria = new Dictionary<string, object>
            {
                ["test1"] = "test1",
                ["test2"] = "test2"
            }
        };

        // Act
        var result = await _achievementService.EditAchievement(achievement.AchievementId, editDto);

        // Assert
        result.AchievementName.Should().Be("UpdatedName");

        var updatedAchievement = await _context.Achievement.FindAsync(achievement.AchievementId);
        updatedAchievement.AchievementName.Should().Be("UpdatedName");
    }

    [Test]
    public async Task DeleteAchievement_ShouldRemoveAchievement_WhenExists()
    {
        // Arrange
        var achievement = new Achievement
        {
            AchievementName = "ToBeDeleted",
            AchievementDescription = "test",
            AchievementCriteria = new Dictionary<string, object>
            {
                ["test1"] = "test1",
                ["test2"] = "test2"
            }
        };
        _context.Achievement.Add(achievement);
        await _context.SaveChangesAsync();

        // Act
        await _achievementService.DeleteAchievement(achievement.AchievementId);

        // Assert
        var deletedAchievement = await _context.Achievement.FindAsync(achievement.AchievementId);
        deletedAchievement.Should().BeNull();
    }
}
