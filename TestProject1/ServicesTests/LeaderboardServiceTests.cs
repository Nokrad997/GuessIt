using Backend.Dtos.EditDtos;
using Backend.Dtos.Interfaces;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services;
using FluentAssertions;
using Moq;

namespace TestProject1.ServicesTests;

[TestFixture]
public class LeaderboardServiceTests
{
    private LeaderboardService _service;
    private Mock<ILeaderboardRepository> _leaderboardRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _leaderboardRepositoryMock = new Mock<ILeaderboardRepository>();
        _service = new LeaderboardService(_leaderboardRepositoryMock.Object);
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllLeaderboardEntries()
    {
        // Arrange
        var leaderboardEntries = CreateLeaderboardList();
        _leaderboardRepositoryMock.Setup(repo => repo.GetWholeLeaderboard()).ReturnsAsync(leaderboardEntries);

        // Act
        var result = await _service.Retrieve();

        // Assert
        result.Should().BeEquivalentTo(leaderboardEntries.Select(x => x.ConvertToDto()));
    }

    [Test]
    public void Retrieve_ShouldThrowException_WhenLeaderboardEntryNotFound()
    {
        // Arrange
        _leaderboardRepositoryMock.Setup(repo => repo.GetLeaderboardById(It.IsAny<int>())).ReturnsAsync((Leaderboard)null);

        // Act
        Func<Task> result = async () => await _service.Retrieve(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Leaderboard entry does not exist");
    }

    [Test]
    public async Task Retrieve_ShouldReturnLeaderboardDto_WhenLeaderboardEntryExists()
    {
        // Arrange
        var leaderboardEntry = CreateLeaderboardEntry();
        _leaderboardRepositoryMock.Setup(repo => repo.GetLeaderboardById(It.IsAny<int>())).ReturnsAsync(leaderboardEntry);

        // Act
        var result = await _service.Retrieve(leaderboardEntry.LeaderBoardId);

        // Assert
        result.Should().BeEquivalentTo(leaderboardEntry.ConvertToDto());
    }

    [Test]
    public void AddLeaderboardEntry_ShouldThrowException_WhenEntryAlreadyExistsForUser()
    {
        // Arrange
        var leaderboardDto = CreateLeaderboardDto();
        _leaderboardRepositoryMock.Setup(repo => repo.GetLeaderboardByUserId(leaderboardDto.UserIdFk))
            .ReturnsAsync(new Leaderboard());

        // Act
        Func<Task> result = async () => await _service.AddLeaderboardEntry(leaderboardDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Leaderboard entry already exists for this user");
    }

    [Test]
    public async Task AddLeaderboardEntry_ShouldAddEntry_WhenEntryIsNewForUser()
    {
        // Arrange
        var leaderboardDto = CreateLeaderboardDto();
        _leaderboardRepositoryMock.Setup(repo => repo.GetLeaderboardByUserId(leaderboardDto.UserIdFk)).ReturnsAsync((Leaderboard)null);

        // Act
        await _service.AddLeaderboardEntry(leaderboardDto);

        // Assert
        _leaderboardRepositoryMock.Verify(repo => repo.CreateLeaderboardEntry(It.Is<Leaderboard>(l =>
            l.UserIdFk == leaderboardDto.UserIdFk)), Times.Once);
    }

    [Test]
    public void EditLeaderboardEntry_ShouldThrowException_WhenLeaderboardEntryNotFound()
    {
        // Arrange
        var editLeaderboardDto = CreateEditLeaderboardDto();
        _leaderboardRepositoryMock.Setup(repo => repo.GetLeaderboardById(It.IsAny<int>())).ReturnsAsync((Leaderboard)null);

        // Act
        Func<Task> result = async () => await _service.EditLeaderBoardEntry(1, editLeaderboardDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Leaderboard entry does not exist");
    }

    [Test]
    public async Task EditLeaderboardEntry_ShouldUpdateEntry_WhenLeaderboardEntryExists()
    {
        // Arrange
        var leaderboardEntry = CreateLeaderboardEntry();
        var editLeaderboardDto = CreateEditLeaderboardDto();
        _leaderboardRepositoryMock.Setup(repo => repo.GetLeaderboardById(leaderboardEntry.LeaderBoardId)).ReturnsAsync(leaderboardEntry);

        // Act
        var result = await _service.EditLeaderBoardEntry(leaderboardEntry.LeaderBoardId, editLeaderboardDto);

        // Assert
        _leaderboardRepositoryMock.Verify(repo => repo.EditLeaderboardEntry(It.Is<Leaderboard>(l => 
            l.LeaderBoardId == leaderboardEntry.LeaderBoardId && l.TotalPoints == editLeaderboardDto.TotalPoints)), Times.Once);
        result.TotalPoints.Should().Be(editLeaderboardDto.TotalPoints);
    }

    [Test]
    public void DeleteLeaderboardEntry_ShouldThrowException_WhenLeaderboardEntryNotFound()
    {
        // Arrange
        _leaderboardRepositoryMock.Setup(repo => repo.GetLeaderboardById(It.IsAny<int>())).ReturnsAsync((Leaderboard)null);

        // Act
        Func<Task> result = async () => await _service.DeleteLeaderboardEntry(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Leaderboard entry does not exist");
    }

    [Test]
    public async Task DeleteLeaderboardEntry_ShouldDeleteEntry_WhenLeaderboardEntryExists()
    {
        // Arrange
        var leaderboardEntry = CreateLeaderboardEntry();
        _leaderboardRepositoryMock.Setup(repo => repo.GetLeaderboardById(leaderboardEntry.LeaderBoardId)).ReturnsAsync(leaderboardEntry);

        // Act
        await _service.DeleteLeaderboardEntry(leaderboardEntry.LeaderBoardId);

        // Assert
        _leaderboardRepositoryMock.Verify(repo => repo.DeleteLeaderboardEntry(leaderboardEntry.LeaderBoardId), Times.Once);
    }

    #region Helper Methods

    private Leaderboard CreateLeaderboardEntry() =>
        new Leaderboard
        {
            LeaderBoardId = 1,
            UserIdFk = 1,
            TotalPoints = 100
        };

    private LeaderboardDto CreateLeaderboardDto() =>
        new LeaderboardDto()
        {
            UserIdFk = 1,
            TotalPoints = 100
        };

    private EditLeaderboardDto CreateEditLeaderboardDto() =>
        new EditLeaderboardDto()
        {
            TotalPoints = 200
        };

    private List<Leaderboard> CreateLeaderboardList() =>
        new List<Leaderboard>
        {
            new Leaderboard() { LeaderBoardId = 1, UserIdFk = 1, TotalPoints = 100 },
            new Leaderboard { LeaderBoardId = 2, UserIdFk = 2, TotalPoints = 150 }
        };

    #endregion
}