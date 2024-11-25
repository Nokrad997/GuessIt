using Backend.Controllers;
using Backend.Dtos.EditDtos;
using Backend.Dtos.Interfaces;
using Backend.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject1.ControllersTests;

[TestFixture]
public class LeaderboardControllerTests
{
    private Mock<ILeaderboardService> _leaderboardServiceMock;
    private LeaderboardController _controller;

    [SetUp]
    public void SetUp()
    {
        _leaderboardServiceMock = new Mock<ILeaderboardService>();
        _controller = new LeaderboardController(_leaderboardServiceMock.Object);
    }

    [Test]
    public async Task GetWholeLeaderboard_ShouldReturnOkResult_WithLeaderboardData()
    {
        // Arrange
        var leaderboard = new List<LeaderboardDto> { CreateValidLeaderboardDto(), CreateValidLeaderboardDto() };
        _leaderboardServiceMock.Setup(service => service.Retrieve()).ReturnsAsync(leaderboard);

        // Act
        var result = await _controller.GetWholeLeaderboard();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task GetLeaderboardById_ShouldReturnOkResult_WhenLeaderboardEntryFound()
    {
        // Arrange
        int id = 1;
        var leaderboardEntry = CreateValidLeaderboardDto();
        _leaderboardServiceMock.Setup(service => service.Retrieve(id)).ReturnsAsync(leaderboardEntry);

        // Act
        var result = await _controller.GetLeaderboardById(id);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task GetLeaderboardById_ShouldReturnBadRequest_WhenExceptionThrown()
    {
        // Arrange
        int id = 1;
        _leaderboardServiceMock.Setup(service => service.Retrieve(id))
            .ThrowsAsync(new Exception("Leaderboard entry not found"));

        // Act
        var result = await _controller.GetLeaderboardById(id);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task AddLeaderboardEntry_ShouldReturnOkResult_WhenEntryAdded()
    {
        // Arrange
        var leaderboardDto = CreateValidLeaderboardDto();
        _leaderboardServiceMock.Setup(service => service.AddLeaderboardEntry(leaderboardDto))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddLeaderboardEntry(leaderboardDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task AddLeaderboardEntry_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var invalidDto = CreateInvalidLeaderboardDto();

        // Act
        var result = await _controller.AddLeaderboardEntry(invalidDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task EditLeaderboardEntry_ShouldReturnOkResult_WhenEntryUpdated()
    {
        // Arrange
        int id = 1;
        var editDto = CreateValidEditLeaderboardDto();
        var updatedDto = CreateValidLeaderboardDto();
        _leaderboardServiceMock.Setup(service => service.EditLeaderBoardEntry(id, editDto))
            .ReturnsAsync(updatedDto);

        // Act
        var result = await _controller.EditLeaderboardEntry(id, editDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task DeleteLeaderboardEntry_ShouldReturnOkResult_WhenEntryDeleted()
    {
        // Arrange
        int id = 1;
        _leaderboardServiceMock.Setup(service => service.DeleteLeaderboardEntry(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteLeaderboardEntry(id);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    #region Helper Methods

    private LeaderboardDto CreateValidLeaderboardDto()
    {
        return new LeaderboardDto
        {
            LeaderBoardId = 1,
            TotalPoints = 1
        };
    }

    private LeaderboardDto CreateInvalidLeaderboardDto()
    {
        return new LeaderboardDto()
        {
            LeaderBoardId = -1,
            TotalPoints = -1
        };
    }

    private EditLeaderboardDto CreateValidEditLeaderboardDto()
    {
        return new EditLeaderboardDto()
        {
            TotalPoints = 1
        };
    }

    #endregion
}