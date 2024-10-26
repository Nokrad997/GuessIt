using Backend.Controllers;
using Backend.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject1.ControllersTests;

[TestFixture]
public class MonthlyUsageControllerTests
{
    private Mock<IMonthlyUsageService> _monthlyUsageServiceMock;
    private MonthlyUsageController _controller;

    [SetUp]
    public void SetUp()
    {
        _monthlyUsageServiceMock = new Mock<IMonthlyUsageService>();
        _controller = new MonthlyUsageController(_monthlyUsageServiceMock.Object);
    }

    [Test]
    public async Task UpdateMonthlyUsage_ShouldReturnOkResult_WhenUsageUpdatedSuccessfully()
    {
        // Arrange
        int userUsage = 100;
        var updatedUsage = "200";
        _monthlyUsageServiceMock.Setup(service => service.UpdateMonthlyUsage(userUsage)).ReturnsAsync(updatedUsage);

        // Act
        var result = await _controller.UpdateMonthlyUsage(userUsage);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task UpdateMonthlyUsage_ShouldReturnBadRequest_WhenExceptionThrown()
    {
        // Arrange
        int userUsage = 100;
        _monthlyUsageServiceMock.Setup(service => service.UpdateMonthlyUsage(userUsage))
            .ThrowsAsync(new Exception("Update failed"));

        // Act
        var result = await _controller.UpdateMonthlyUsage(userUsage);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task GetMonthlyUsage_ShouldReturnOkResult_WithMonthlyUsageData()
    {
        // Arrange
        var monthlyUsage = 150;
        _monthlyUsageServiceMock.Setup(service => service.GetMonthlyUsage()).ReturnsAsync(monthlyUsage);

        // Act
        var result = await _controller.GetMonthlyUsage();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task GetMonthlyUsage_ShouldReturnBadRequest_WhenExceptionThrown()
    {
        // Arrange
        _monthlyUsageServiceMock.Setup(service => service.GetMonthlyUsage())
            .ThrowsAsync(new Exception("Retrieval failed"));

        // Act
        var result = await _controller.GetMonthlyUsage();

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }
}