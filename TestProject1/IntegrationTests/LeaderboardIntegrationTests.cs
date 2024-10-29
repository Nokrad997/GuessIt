using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Context;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Dtos.Interfaces;
using Backend.Entities;
using Backend.Repositories;
using Backend.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace TestProject1.IntegrationTests;

[TestFixture]
public class LeaderboardServiceIntegrationTests
{
    private LeaderboardService _leaderboardService;
    private LeaderboardRepository _leaderboardRepository;
    private GuessItContext _context;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<GuessItContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new GuessItContext(options);
        _leaderboardRepository = new LeaderboardRepository(_context);
        _leaderboardService = new LeaderboardService(_leaderboardRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddLeaderboardEntry_ShouldAddEntry_WhenUserDoesNotExistInLeaderboard()
    {
        // Arrange
        var leaderboardDto = new LeaderboardDto { UserIdFk = 1, TotalPoints = 100 };

        // Act
        await _leaderboardService.AddLeaderboardEntry(leaderboardDto);

        // Assert
        var leaderboard = await _leaderboardRepository.GetWholeLeaderboard();
        leaderboard.Should().ContainSingle(x => x.UserIdFk == 1 && x.TotalPoints == 100);
    }

    [Test]
    public async Task AddLeaderboardEntry_ShouldThrowException_WhenUserAlreadyExistsInLeaderboard()
    {
        // Arrange
        var leaderboardDto = new LeaderboardDto { UserIdFk = 1, TotalPoints = 100 };
        await _leaderboardService.AddLeaderboardEntry(leaderboardDto);

        var duplicateEntryDto = new LeaderboardDto() { UserIdFk = 1, TotalPoints = 150 };

        // Act
        Func<Task> act = async () => await _leaderboardService.AddLeaderboardEntry(duplicateEntryDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Leaderboard entry already exists for this user");
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllLeaderboardEntries()
    {
        // Arrange
        await AddLeaderboardEntry(1, 100);
        await AddLeaderboardEntry(2, 200);

        // Act
        var result = await _leaderboardService.Retrieve();

        // Assert
        result.Should().HaveCount(2);
        result.Select(x => x.TotalPoints).Should().Contain(new[] { 100, 200 });
    }

    [Test]
    public async Task Retrieve_ShouldReturnLeaderboardEntry_WhenEntryExists()
    {
        // Arrange
        var entryId = await AddLeaderboardEntry(1, 100);

        // Act
        var result = await _leaderboardService.Retrieve(entryId);

        // Assert
        result.Should().NotBeNull();
        result.TotalPoints.Should().Be(100);
    }

    [Test]
    public async Task EditLeaderBoardEntry_ShouldUpdatePoints_WhenEntryExists()
    {
        // Arrange
        var entryId = await AddLeaderboardEntry(1, 100);
        var editDto = new EditLeaderboardDto { TotalPoints = 200 };

        // Act
        var result = await _leaderboardService.EditLeaderBoardEntry(entryId, editDto);

        // Assert
        result.TotalPoints.Should().Be(200);
        var updatedEntry = await _leaderboardRepository.GetLeaderboardById(entryId);
        updatedEntry.TotalPoints.Should().Be(200);
    }

    [Test]
    public async Task DeleteLeaderboardEntry_ShouldRemoveEntry_WhenEntryExists()
    {
        // Arrange
        var entryId = await AddLeaderboardEntry(1, 100);

        // Act
        await _leaderboardService.DeleteLeaderboardEntry(entryId);

        // Assert
        var entry = await _leaderboardRepository.GetLeaderboardById(entryId);
        entry.Should().BeNull();
    }

    private async Task<int> AddLeaderboardEntry(int userId, int totalPoints)
    {
        var entry = new Leaderboard { UserIdFk = userId, TotalPoints = totalPoints };
        _context.Leaderboard.Add(entry);
        await _context.SaveChangesAsync();
        return entry.LeaderBoardId;
    }
}
