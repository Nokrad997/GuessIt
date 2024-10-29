using Backend.Controllers;
using Backend.Dtos;
using Backend.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject1.ControllersTests;

[TestFixture]
public class AchievementControllerTests
{
    private Mock<IAchievementService> _achievementServiceMock;
    private AchievementController _controller;

    [SetUp]
    public void Setup()
    {
        // Tworzymy mock dla AchievementService
        _achievementServiceMock = new Mock<IAchievementService>();

        // Tworzymy AchievementController z zamockowanym AchievementService
        _controller = new AchievementController(_achievementServiceMock.Object);
    }

    [Test]
    public async Task GetAllAchievements_ShouldReturnOkResult_WhenAchievementsAreRetrieved()
    {
        // Arrange
        var mockAchievements = new List<AchievementDto>
        {
            new AchievementDto { AchievementId = 1, AchievementName = "Test Achievement 1" },
            new AchievementDto { AchievementId = 2, AchievementName = "Test Achievement 2" }
        };
        _achievementServiceMock.Setup(service => service.Retrieve()).ReturnsAsync(mockAchievements);

        // Act
        var result = await _controller.GetAllAchievements();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new 
            { 
                message = "Achievements retrieved successfully", 
                achievements = mockAchievements
            });
    }

    [Test]
    public async Task GetAchievement_ShouldReturnOkResult_WhenAchievementIsRetrieved()
    {
        // Arrange
        int achievementId = 1;
        var mockAchievement = new AchievementDto { AchievementId = achievementId, AchievementName = "Test Achievement" };
        _achievementServiceMock.Setup(service => service.Retrieve(achievementId)).ReturnsAsync(mockAchievement);

        // Act
        var result = await _controller.GetAchievement(achievementId);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new
            {
                message = "Achievement retrieved successfully",
                achievement = mockAchievement
            });
    }

    [Test]
    public async Task GetAchievement_ShouldReturnBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        int achievementId = 1;
        _achievementServiceMock.Setup(service => service.Retrieve(achievementId))
            .ThrowsAsync(new ArgumentException("No achievement with provided id"));

        // Act
        var result = await _controller.GetAchievement(achievementId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task AddAchievement_ShouldReturnOkResult_WhenAchievementIsAdded()
    {
        // Arrange
        var dto = GetValidDto();
        _achievementServiceMock.Setup(service => service.AddAchievement(dto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddAchievement(dto);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task AddAchievement_ShouldReturnBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var dto = GetValidDto();
        _achievementServiceMock.Setup(service => service.AddAchievement(dto))
            .ThrowsAsync(new ArgumentException("Achievement with provided name already exists"));

        // Act
        var result = await _controller.AddAchievement(dto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task EditAchievement_ShouldReturnOkResult_WhenAchievementIsEdited()
    {
        // Arrange
        int id = 1;
        var editDto = GetValidDto();
        var mockAchievement = new AchievementDto { AchievementId = id, AchievementName = "Updated Achievement" };
        _achievementServiceMock.Setup(service => service.EditAchievement(id, editDto)).ReturnsAsync(mockAchievement);

        // Act
        var result = await _controller.EditAchievement(id, editDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new
            {
                message = "Achievement edited successfully",
                achievement = mockAchievement
            });
    }

    [Test]
    public async Task DeleteAchievement_ShouldReturnOkResult_WhenAchievementIsDeleted()
    {
        // Arrange
        int id = 1;
        _achievementServiceMock.Setup(service => service.DeleteAchievement(id)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteAchievement(id);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task DeleteAchievement_ShouldReturnBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        int id = 1;
        _achievementServiceMock.Setup(service => service.DeleteAchievement(id))
            .ThrowsAsync(new ArgumentException("Achievement doesn't exist"));

        // Act
        var result = await _controller.DeleteAchievement(id);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    private AchievementDto GetValidDto()
    {
        return new AchievementDto
        {
            AchievementId = 1,
            AchievementName = "Test Achievement",
            AchievementDescription = "This is a test description",
            AchievementCriteria = new Dictionary<string, object>
            {
                ["test"] = "test"
            }
        };
    }
}

