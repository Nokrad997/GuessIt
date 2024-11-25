using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services;
using FluentAssertions;
using Moq;
using NetTopologySuite.Geometries;

namespace TestProject1.ServicesTests;

[TestFixture]
public class GeolocationServiceTests
{
    private GeolocationService _service;
    private Mock<IGeolocationRepository> _geolocationRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _geolocationRepositoryMock = new Mock<IGeolocationRepository>();
        _service = new GeolocationService(_geolocationRepositoryMock.Object);
    }

    [Test]
    public void Retrieve_ShouldThrowException_WhenGeolocationNotFound()
    {
        // Arrange
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationById(It.IsAny<int>())).ReturnsAsync((Geolocation)null);

        // Act
        Func<Task> result = async () => await _service.Retrieve(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Geolocation with provided id not found");
    }

    [Test]
    public async Task Retrieve_ShouldReturnGeolocationDto_WhenGeolocationExists()
    {
        // Arrange
        var geolocation = CreateGeolocation();
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationById(It.IsAny<int>())).ReturnsAsync(geolocation);

        // Act
        var result = await _service.Retrieve(geolocation.GeolocationId);

        // Assert
        result.Should().BeEquivalentTo(geolocation.ConvertToDto());
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllGeolocations()
    {
        // Arrange
        var geolocations = CreateGeolocationList();
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocations()).ReturnsAsync(geolocations);

        // Act
        var result = await _service.Retrieve();

        // Assert
        result.Should().BeEquivalentTo(geolocations.Select(g => g.ConvertToDto()));
    }

    [Test]
    public void AddGeolocation_ShouldThrowException_WhenGeolocationWithAreaExists()
    {
        // Arrange
        var geolocationDto = CreateGeolocationDto();
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationByArea(geolocationDto.Area)).ReturnsAsync(new Geolocation());

        // Act
        Func<Task> result = async () => await _service.AddGeolocation(geolocationDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Geolocation with provided area already exists");
    }

    [Test]
    public async Task AddGeolocation_ShouldAddGeolocation_WhenGeolocationIsUnique()
    {
        // Arrange
        var geolocationDto = CreateGeolocationDto();
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationByArea(geolocationDto.Area)).ReturnsAsync((Geolocation)null);

        // Act
        await _service.AddGeolocation(geolocationDto);

        // Assert
        _geolocationRepositoryMock.Verify(repo => repo.AddGeolocation(It.Is<Geolocation>(g => 
            g.Area == geolocationDto.Area)), Times.Once);
    }

    [Test]
    public void EditGeolocation_ShouldThrowException_WhenGeolocationNotFound()
    {
        // Arrange
        var editGeolocationDto = CreateEditGeolocationDto();
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationById(It.IsAny<int>())).ReturnsAsync((Geolocation)null);

        // Act
        Func<Task> result = async () => await _service.EditGeolocation(1, editGeolocationDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Geolocation with provided id not found");
    }

    [Test]
    public async Task EditGeolocation_ShouldEditGeolocation_WhenGeolocationExists()
    {
        // Arrange
        var geolocation = CreateGeolocation();
        var editGeolocationDto = CreateEditGeolocationDto();
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationById(geolocation.GeolocationId)).ReturnsAsync(geolocation);

        // Act
        var result = await _service.EditGeolocation(geolocation.GeolocationId, editGeolocationDto);

        // Assert
        _geolocationRepositoryMock.Verify(repo => repo.EditGeolocation(It.Is<Geolocation>(g => 
            g.GeolocationId == geolocation.GeolocationId && g.Area == editGeolocationDto.Area)), Times.Once);
        result.Area.Should().Be(editGeolocationDto.Area);
    }

    [Test]
    public void DeleteGeolocation_ShouldThrowException_WhenGeolocationNotFound()
    {
        // Arrange
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationById(It.IsAny<int>())).ReturnsAsync((Geolocation)null);

        // Act
        Func<Task> result = async () => await _service.DeleteGeolocation(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Geolocation with provided id not found");
    }

    [Test]
    public async Task DeleteGeolocation_ShouldDeleteGeolocation_WhenGeolocationExists()
    {
        // Arrange
        var geolocation = CreateGeolocation();
        _geolocationRepositoryMock.Setup(repo => repo.GetGeolocationById(geolocation.GeolocationId)).ReturnsAsync(geolocation);

        // Act
        await _service.DeleteGeolocation(geolocation.GeolocationId);

        // Assert
        _geolocationRepositoryMock.Verify(repo => repo.DeleteGeolocation(geolocation), Times.Once);
    }

    // *** Prywatne metody pomocnicze ***

    private Geolocation CreateGeolocation() =>
        new Geolocation
        {
            GeolocationId = 1,
            Area = new Polygon(new LinearRing(new[]
            {
                new Coordinate(1, 2),
                new Coordinate(2, 3),
                new Coordinate(3, 2),
                new Coordinate(2, 1),
                new Coordinate(1, 2)
            }))
        };

    private GeolocationDto CreateGeolocationDto() =>
        new GeolocationDto()
        {
            Area = new Polygon(new LinearRing(new[]
            {
                new Coordinate(1, 2),
                new Coordinate(2, 3),
                new Coordinate(3, 2),
                new Coordinate(2, 1),
                new Coordinate(1, 2)
            }))
        };

    private EditGeolocationDto CreateEditGeolocationDto() =>
        new EditGeolocationDto()
        {
            Area = new Polygon(new LinearRing(new[]
            {
                new Coordinate(1, 2),
                new Coordinate(2, 3),
                new Coordinate(3, 2),
                new Coordinate(2, 1),
                new Coordinate(1, 2)
            }))
        };

    private List<Geolocation> CreateGeolocationList() =>
        new List<Geolocation>
        {
            new Geolocation { GeolocationId = 1, Area = new Polygon(new LinearRing(new[]
            {
                new Coordinate(1, 2),
                new Coordinate(2, 3),
                new Coordinate(3, 2),
                new Coordinate(2, 1),
                new Coordinate(1, 2)
            })) },
            new Geolocation() { GeolocationId = 2, Area = new Polygon(new LinearRing(new[]
            {
                new Coordinate(1, 2),
                new Coordinate(2, 3),
                new Coordinate(3, 2),
                new Coordinate(2, 1),
                new Coordinate(1, 2)
            })) }
        };
}