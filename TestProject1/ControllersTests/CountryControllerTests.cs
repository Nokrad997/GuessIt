using Backend.Controllers;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject1.ControllersTests;

[TestFixture]
public class CountryControllerTests
{
    private Mock<ICountryService> _countryServiceMock;
    private CountryController _controller;

    [SetUp]
    public void SetUp()
    {
        _countryServiceMock = new Mock<ICountryService>();
        _controller = new CountryController(_countryServiceMock.Object);
    }

    [Test]
    public async Task GetCountryById_ShouldReturnOkResult_WhenCountryIsFound()
    {
        // Arrange
        int countryId = 1;
        var countryDto = CreateValidCountryDto();
        countryDto.CountryId = countryId;
        _countryServiceMock.Setup(service => service.Retrieve(countryId)).ReturnsAsync(countryDto);

        // Act
        var result = await _controller.GetCountryById(countryId);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task GetCountryById_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        int countryId = 1;
        _countryServiceMock.Setup(service => service.Retrieve(countryId))
            .ThrowsAsync(new ArgumentException("Country not found"));

        // Act
        var result = await _controller.GetCountryById(countryId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task GetAllCountries_ShouldReturnOkResult_WithListOfCountries()
    {
        // Arrange
        var countries = new List<CountryDto> { CreateValidCountryDto(), CreateValidCountryDto() };
        _countryServiceMock.Setup(service => service.Retrieve()).ReturnsAsync(countries);

        // Act
        var result = await _controller.GetAllCountries();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task AddCountry_ShouldReturnOkResult_WhenCountryIsAdded()
    {
        // Arrange
        var countryDto = CreateValidCountryDto();
        _countryServiceMock.Setup(service => service.AddCountry(countryDto)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CreateCountry(countryDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task AddCountry_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var invalidDto = CreateInvalidCountryDto();

        // Act
        var result = await _controller.CreateCountry(invalidDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task AddCountry_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        var countryDto = CreateValidCountryDto();
        _countryServiceMock.Setup(service => service.AddCountry(countryDto))
            .ThrowsAsync(new ArgumentException("Country already exists"));

        // Act
        var result = await _controller.CreateCountry(countryDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task EditCountry_ShouldReturnOkResult_WhenCountryIsUpdated()
    {
        // Arrange
        int countryId = 1;
        var editCountryDto = CreateValidEditCountryDto();
        var updatedCountryDto = CreateValidCountryDto();
        updatedCountryDto.CountryId = countryId;
        _countryServiceMock.Setup(service => service.EditCountry(countryId, editCountryDto))
            .ReturnsAsync(updatedCountryDto);

        // Act
        var result = await _controller.UpdateCountry(countryId, editCountryDto);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task EditCountry_ShouldReturnBadRequest_WhenValidationFails()
    {
        // Arrange
        var invalidDto = CreateInvalidEditCountryDto();
        int countryId = 1;

        // Act
        var result = await _controller.UpdateCountry(countryId, invalidDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task EditCountry_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        int countryId = 1;
        var editCountryDto = CreateValidEditCountryDto();
        _countryServiceMock.Setup(service => service.EditCountry(countryId, editCountryDto))
            .ThrowsAsync(new ArgumentException("Country not found"));

        // Act
        var result = await _controller.UpdateCountry(countryId, editCountryDto);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    [Test]
    public async Task DeleteCountry_ShouldReturnOkResult_WhenCountryIsDeleted()
    {
        // Arrange
        int countryId = 1;
        _countryServiceMock.Setup(service => service.DeleteCountry(countryId)).Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteCountry(countryId);

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.StatusCode.Should().Be(200);
    }

    [Test]
    public async Task DeleteCountry_ShouldReturnBadRequest_WhenArgumentExceptionIsThrown()
    {
        // Arrange
        int countryId = 1;
        _countryServiceMock.Setup(service => service.DeleteCountry(countryId))
            .ThrowsAsync(new ArgumentException("Country not found"));

        // Act
        var result = await _controller.DeleteCountry(countryId);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.StatusCode.Should().Be(400);
    }

    #region Helper Methods

    private CountryDto CreateValidCountryDto()
    {
        return new CountryDto
        {
            CountryId = 1,
            CountryName = "Valid Country",
            GeolocationIdFk = 2,
            ContinentIdFk = 2
        };
    }

    private CountryDto CreateInvalidCountryDto()
    {
        return new CountryDto()
        {
            CountryName = "",  
            ContinentIdFk = -1,
            GeolocationIdFk = -1
        };
    }

    private EditCountryDto CreateValidEditCountryDto()
    {
        return new EditCountryDto
        {
            CountryName = "Updated Country",
            ContinentIdFk = 1,
            GeolocationIdFk = 1
        };
    }

    private EditCountryDto CreateInvalidEditCountryDto()
    {
        return new EditCountryDto()
        {
            CountryName = "", 
            ContinentIdFk = -1,
            GeolocationIdFk = -1
        };
    }

    #endregion
}