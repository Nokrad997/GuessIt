using Backend.Controllers;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject1.ControllersTests;

[TestFixture]
public class ContinentControllerTests
{
    private Mock<IContinentService> _continentServiceMock;
    private ContinentController _controller;

    [SetUp]
    public void SetUp()
    {
        _continentServiceMock = new Mock<IContinentService>();
        _controller = new ContinentController(_continentServiceMock.Object);
    }

    [Test]
    public async Task GetContinentById_ShouldReturnOkResult_WhenContinentIsFound()
    {
        // Arrange
        int continentId = 1;
        var continentDto = new ContinentDto { ContinentId = continentId };
        _continentServiceMock.Setup(service => service.Retrieve(continentId)).ReturnsAsync(continentDto);

        // Act
        var result = await _controller.GetContinentById(continentId);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task GetContinentById_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        int continentId = 1;
        _continentServiceMock.Setup(service => service.Retrieve(continentId))
            .ThrowsAsync(new ArgumentException("Continent not found"));

        // Act
        var result = await _controller.GetContinentById(continentId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task GetAllContinents_ShouldReturnOkResult_WithListOfContinents()
    {
        // Arrange
        var continents = new List<ContinentDto> { new ContinentDto(), new ContinentDto() };
        _continentServiceMock.Setup(service => service.Retrieve()).ReturnsAsync(continents);

        // Act
        var result = await _controller.GetAllContinents();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task AddContinent_ShouldReturnOkResult_WhenContinentIsAdded()
    {
        // Arrange
        var continentDto = GetValidContinentDto();
        _continentServiceMock.Setup(service => service.AddContinent(continentDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AddContinent(continentDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task AddContinent_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var invalidDto = new ContinentDto();

        // Act
        var result = await _controller.AddContinent(invalidDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task AddContinent_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var continentDto = new ContinentDto();
        _continentServiceMock.Setup(service => service.AddContinent(continentDto))
            .ThrowsAsync(new ArgumentException("Continent already exists"));

        // Act
        var result = await _controller.AddContinent(continentDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task EditContinent_ShouldReturnOkResult_WhenContinentIsUpdated()
    {
        // Arrange
        int continentId = 1;
        var editContinentDto = GetValidContinentDto();
        var updatedContinentDto = new ContinentDto() { ContinentId = continentId };
        _continentServiceMock.Setup(service => service.EditContinent(continentId, editContinentDto))
            .ReturnsAsync(updatedContinentDto);

        // Act
        var result = await _controller.EditContinent(continentId, editContinentDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task EditContinent_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var invalidDto = new EditContinentDto();
        int continentId = 1;

        // Act
        var result = await _controller.EditContinent(continentId, invalidDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task EditContinent_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        int continentId = 1;
        var editContinentDto = new EditContinentDto();
        _continentServiceMock.Setup(service => service.EditContinent(continentId, editContinentDto))
            .ThrowsAsync(new ArgumentException("Continent not found"));

        // Act
        var result = await _controller.EditContinent(continentId, editContinentDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task DeleteContinent_ShouldReturnOkResult_WhenContinentIsDeleted()
    {
        // Arrange
        int continentId = 1;
        _continentServiceMock.Setup(service => service.DeleteContinent(continentId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteContinent(continentId);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task DeleteContinent_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        int continentId = 1;
        _continentServiceMock.Setup(service => service.DeleteContinent(continentId))
            .ThrowsAsync(new ArgumentException("Continent not found"));

        // Act
        var result = await _controller.DeleteContinent(continentId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    private ContinentDto GetValidContinentDto()
    {
        return new ContinentDto
        {
            ContinentId = 1,
            ContinentName = "test"
        };
    }
}