using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services;
using FluentAssertions;
using Moq;

namespace TestProject1.ServicesTests;

[TestFixture]
public class CountryServiceTests
{
    private CountryService _service;
    private Mock<ICountryRepository> _countryRepositoryMock;
    private Mock<IGeolocationRepository> _geolocationRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _countryRepositoryMock = new Mock<ICountryRepository>();
        _geolocationRepositoryMock = new Mock<IGeolocationRepository>();
        _service = new CountryService(_countryRepositoryMock.Object, _geolocationRepositoryMock.Object);
    }

    [Test]
    public void Retrieve_ShouldThrowException_WhenCountryNotFound()
    {
        // Arrange
        _countryRepositoryMock.Setup(repo => repo.GetCountryById(It.IsAny<int>())).ReturnsAsync((Country)null);

        // Act
        Func<Task> result = async () => await _service.Retrieve(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Country with provided id not found");
    }

    [Test]
    public async Task Retrieve_ShouldReturnCountryDto_WhenCountryExists()
    {
        // Arrange
        var country = CreateCountry();
        _countryRepositoryMock.Setup(repo => repo.GetCountryById(It.IsAny<int>())).ReturnsAsync(country);

        // Act
        var result = await _service.Retrieve(country.CountryId);

        // Assert
        result.Should().BeEquivalentTo(country.ConvertToDto());
    }

    [Test]
    public async Task RetrieveByContinentId_ShouldReturnCountriesDtoList_WhenCountriesExist()
    {
        // Arrange
        var countries = CreateCountryList();
        _countryRepositoryMock.Setup(repo => repo.GetCountriesByContinentId(It.IsAny<int>())).ReturnsAsync(countries);

        // Act
        var result = await _service.RetrieveByContinentId(1);

        // Assert
        result.Should().BeEquivalentTo(countries.Select(c => c.ConvertToDto()));
    }

    [Test]
    public void AddCountry_ShouldThrowException_WhenCountryWithGeolocationIdExists()
    {
        // Arrange
        var countryDto = CreateCountryDto();
        _countryRepositoryMock.Setup(repo => repo.GetCountryByGeolocationId(countryDto.GeolocationIdFk))
            .ReturnsAsync(new Country());

        // Act
        Func<Task> result = async () => await _service.AddCountry(countryDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Country with provided geolocation id already exists");
    }

    [Test]
    public void AddCountry_ShouldThrowException_WhenCountryWithNameExists()
    {
        // Arrange
        var countryDto = CreateCountryDto();
        _countryRepositoryMock.Setup(repo => repo.GetCountryByName(countryDto.CountryName))
            .ReturnsAsync(new Country());

        // Act
        Func<Task> result = async () => await _service.AddCountry(countryDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Country with provided name already exists");
    }

    [Test]
    public void AddCountry_ShouldThrowException_WhenGeolocationNotFound()
    {
        // Arrange
        var countryDto = CreateCountryDto();
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationById(countryDto.GeolocationIdFk))
            .ReturnsAsync((Geolocation)null);

        // Act
        Func<Task> result = async () => await _service.AddCountry(countryDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Geolocation with provided id not found");
    }

    [Test]
    public async Task AddCountry_ShouldAddCountry_WhenCountryIsValid()
    {
        // Arrange
        var countryDto = CreateCountryDto();
        SetupValidAddCountry(countryDto);

        // Act
        await _service.AddCountry(countryDto);

        // Assert
        _countryRepositoryMock.Verify(repo => repo.AddCountry(It.Is<Country>(c =>
            c.CountryName == countryDto.CountryName && c.GeolocationIdFk == countryDto.GeolocationIdFk)), Times.Once);
    }

    [Test]
    public void EditCountry_ShouldThrowException_WhenCountryNotFound()
    {
        // Arrange
        var editCountryDto = CreateEditCountryDto();
        _countryRepositoryMock.Setup(repo => repo.GetCountryById(It.IsAny<int>())).ReturnsAsync((Country)null);

        // Act
        Func<Task> result = async () => await _service.EditCountry(1, editCountryDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Country with provided id not found");
    }

    [Test]
    public async Task DeleteCountry_ShouldDeleteCountry_WhenCountryExists()
    {
        // Arrange
        var country = CreateCountry();
        _countryRepositoryMock.Setup(repo => repo.GetCountryById(It.IsAny<int>())).ReturnsAsync(country);

        // Act
        await _service.DeleteCountry(country.CountryId);

        // Assert
        _countryRepositoryMock.Verify(repo => repo.DeleteCountry(country), Times.Once);
    }

    [Test]
    public void DeleteCountry_ShouldThrowException_WhenCountryNotFound()
    {
        // Arrange
        _countryRepositoryMock.Setup(repo => repo.GetCountryById(It.IsAny<int>())).ReturnsAsync((Country)null);

        // Act
        Func<Task> result = async () => await _service.DeleteCountry(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Country with provided id not found");
    }

    // *** Prywatne metody pomocnicze ***

    private Country CreateCountry() =>
        new Country
        {
            CountryId = 1,
            CountryName = "TestCountry",
            GeolocationIdFk = 1
        };

    private CountryDto CreateCountryDto() =>
        new CountryDto
        {
            CountryName = "TestCountry",
            GeolocationIdFk = 1
        };

    private EditCountryDto CreateEditCountryDto() =>
        new EditCountryDto()
        {
            CountryName = "UpdatedCountry",
            GeolocationIdFk = 2
        };

    private List<Country> CreateCountryList() =>
        new List<Country>
        {
            new Country { CountryId = 1, CountryName = "Country1", GeolocationIdFk = 1 },
            new Country { CountryId = 2, CountryName = "Country2", GeolocationIdFk = 2 }
        };

    private void SetupValidAddCountry(CountryDto countryDto)
    {
        _countryRepositoryMock.Setup(repo => repo.GetCountryByGeolocationId(countryDto.GeolocationIdFk)).ReturnsAsync((Country)null);
        _countryRepositoryMock.Setup(repo => repo.GetCountryByName(countryDto.CountryName)).ReturnsAsync((Country)null);
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationById(countryDto.GeolocationIdFk)).ReturnsAsync(new Geolocation());
    }
}