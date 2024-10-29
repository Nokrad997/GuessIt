using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services;
using FluentAssertions;
using Moq;

namespace TestProject1.ServicesTests;

[TestFixture]
public class ContinentServiceTests
{
    private ContinentService _service;
    private Mock<IContinentRepository> _continentRepositoryMock;
    private Mock<IGeolocationRepository> _geolocationRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _continentRepositoryMock = new Mock<IContinentRepository>();
        _geolocationRepositoryMock = new Mock<IGeolocationRepository>();
        _service = new ContinentService(_continentRepositoryMock.Object, _geolocationRepositoryMock.Object);
    }

    [Test]
    public void Retrieve_ShouldThrowException_WhenContinentNotFound()
    {
        // Arrange
        _continentRepositoryMock.Setup(repo => repo.GetContinentById(It.IsAny<int>())).ReturnsAsync((Continent)null);

        // Act
        Func<Task> result = async () => await _service.Retrieve(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Continent with provided id not found");
    }

    [Test]
    public async Task Retrieve_ShouldReturnContinentDto_WhenContinentExists()
    {
        // Arrange
        var continent = CreateContinent();
        _continentRepositoryMock.Setup(repo => repo.GetContinentById(It.IsAny<int>())).ReturnsAsync(continent);

        // Act
        var result = await _service.Retrieve(continent.ContinentId);

        // Assert
        result.Should().BeEquivalentTo(continent.ConvertToDto());
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllContinents_WhenContinentsArePresent()
    {
        // Arrange
        var continents = CreateContinentList();
        _continentRepositoryMock.Setup(repo => repo.GetContinents()).ReturnsAsync(continents);

        // Act
        var result = await _service.Retrieve();

        // Assert
        result.Should().BeEquivalentTo(continents.Select(c => c.ConvertToDto()));
    }

    [Test]
    public void AddContinent_ShouldThrowException_WhenContinentWithGeolocationIdExists()
    {
        // Arrange
        var continentDto = CreateContinentDto();
        _continentRepositoryMock.Setup(repo => repo.GetContinentByGeolocationId(continentDto.GeolocationIdFk))
            .ReturnsAsync(new Continent());

        // Act
        Func<Task> result = async () => await _service.AddContinent(continentDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Continent with provided geolocation id already exists");
    }

    [Test]
    public void AddContinent_ShouldThrowException_WhenContinentWithNameExists()
    {
        // Arrange
        var continentDto = CreateContinentDto();
        _continentRepositoryMock.Setup(repo => repo.GetContinentByName(continentDto.ContinentName))
            .ReturnsAsync(new Continent());

        // Act
        Func<Task> result = async () => await _service.AddContinent(continentDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Continent with provided name already exists");
    }

    [Test]
    public void AddContinent_ShouldThrowException_WhenGeolocationNotFound()
    {
        // Arrange
        var continentDto = CreateContinentDto();
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationById(continentDto.GeolocationIdFk))
            .ReturnsAsync((Geolocation)null);

        // Act
        Func<Task> result = async () => await _service.AddContinent(continentDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Geolocation with provided id not found");
    }

    [Test]
    public async Task AddContinent_ShouldAddContinent_WhenContinentIsValid()
    {
        // Arrange
        var continentDto = CreateContinentDto();
        SetupValidAddContinent(continentDto);

        // Act
        await _service.AddContinent(continentDto);

        // Assert
        _continentRepositoryMock.Verify(repo => repo.AddContinent(It.Is<Continent>(c =>
            c.ContinentName == continentDto.ContinentName && c.GeolocationIdFk == continentDto.GeolocationIdFk)), Times.Once);
    }

    [Test]
    public void EditContinent_ShouldThrowException_WhenContinentNotFound()
    {
        // Arrange
        var editContinentDto = CreateEditContinentDto();
        _continentRepositoryMock.Setup(repo => repo.GetContinentById(It.IsAny<int>())).ReturnsAsync((Continent)null);

        // Act
        Func<Task> result = async () => await _service.EditContinent(1, editContinentDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Continent with provided id not found");
    }

    [Test]
    public async Task DeleteContinent_ShouldDeleteContinent_WhenContinentExists()
    {
        // Arrange
        var continent = CreateContinent();
        _continentRepositoryMock.Setup(repo => repo.GetContinentById(It.IsAny<int>())).ReturnsAsync(continent);

        // Act
        await _service.DeleteContinent(continent.ContinentId);

        // Assert
        _continentRepositoryMock.Verify(repo => repo.DeleteContinent(continent), Times.Once);
    }

    [Test]
    public void DeleteContinent_ShouldThrowException_WhenContinentNotFound()
    {
        // Arrange
        _continentRepositoryMock.Setup(repo => repo.GetContinentById(It.IsAny<int>())).ReturnsAsync((Continent)null);

        // Act
        Func<Task> result = async () => await _service.DeleteContinent(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Continent with provided id not found");
    }

    // *** Prywatne metody pomocnicze ***

    private Continent CreateContinent() =>
        new Continent
        {
            ContinentId = 1,
            ContinentName = "TestContinent",
            GeolocationIdFk = 1
        };

    private ContinentDto CreateContinentDto() =>
        new ContinentDto()
        {
            ContinentName = "TestContinent",
            GeolocationIdFk = 1
        };

    private EditContinentDto CreateEditContinentDto() =>
        new EditContinentDto
        {
            ContinentName = "UpdatedContinent",
            GeolocationIdFk = 2
        };

    private List<Continent> CreateContinentList() =>
        new List<Continent>
        {
            new Continent() { ContinentId = 1, ContinentName = "Continent1", GeolocationIdFk = 1 },
            new Continent { ContinentId = 2, ContinentName = "Continent2", GeolocationIdFk = 2 }
        };

    private void SetupValidAddContinent(ContinentDto continentDto)
    {
        _continentRepositoryMock.Setup(repo => repo.GetContinentByGeolocationId(continentDto.GeolocationIdFk)).ReturnsAsync((Continent)null);
        _continentRepositoryMock.Setup(repo => repo.GetContinentByName(continentDto.ContinentName)).ReturnsAsync((Continent)null);
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationById(continentDto.GeolocationIdFk)).ReturnsAsync(new Geolocation());
    }
}