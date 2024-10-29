using Backend.Context;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories;
using Backend.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace TestProject1.IntegrationTests;

[TestFixture]
public class ContinentServiceIntegrationTests : IDisposable
{
    private ContinentService _continentService;
    private ContinentRepository _continentRepository;
    private GeolocationRepository _geolocationRepository;
    private GuessItContext _context;
    private static int x = 1;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<GuessItContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unikalna baza dla każdego testu
            .Options;

        _context = new GuessItContext(options);
        _continentRepository = new ContinentRepository(_context);
        _geolocationRepository = new GeolocationRepository(_context);
        _continentService = new ContinentService(_continentRepository, _geolocationRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted(); // Usunięcie bazy po każdym teście
        _context.Dispose(); // Zwolnienie zasobów bazy danych
    }

    [Test]
    public async Task AddContinent_ShouldAddNewContinent_WhenGeolocationAndNameAreUnique()
    {
        // Arrange
        var geolocationId = await AddGeolocation();
        var continentDto = new ContinentDto { ContinentName = "New Continent", GeolocationIdFk = geolocationId };

        // Act
        await _continentService.AddContinent(continentDto);

        // Assert
        var continents = await _continentRepository.GetContinents();
        continents.Should().ContainSingle(c => c.ContinentName == "New Continent" && c.GeolocationIdFk == geolocationId);
    }

    [Test]
    public async Task AddContinent_ShouldThrowException_WhenContinentNameExists()
    {
        // Arrange
        var geolocationId = await AddGeolocation();
        var continentDto = new ContinentDto { ContinentName = "DuplicateName", GeolocationIdFk = geolocationId };
        await _continentService.AddContinent(continentDto);

        var duplicateContinentDto = new ContinentDto { ContinentName = "DuplicateName", GeolocationIdFk = geolocationId + 1 };

        // Act
        Func<Task> act = async () => await _continentService.AddContinent(duplicateContinentDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Continent with provided name already exists");
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllContinents()
    {
        // Arrange
        var geolocationId = await AddGeolocation();
        await AddContinent("Continent1", geolocationId);
        await AddContinent("Continent2", geolocationId);

        // Act
        var result = await _continentService.Retrieve();

        // Assert
        result.Should().HaveCount(2);
        result.Select(c => c.ContinentName).Should().Contain(new[] { "Continent1", "Continent2" });
    }

    [Test]
    public async Task EditContinent_ShouldUpdateContinent_WhenDataIsValid()
    {
        // Arrange
        var geolocationId = await AddGeolocation();
        var continentId = await AddContinent("OldName", geolocationId);

        var editDto = new EditContinentDto { ContinentName = "UpdatedName", GeolocationIdFk = geolocationId };

        // Act
        var result = await _continentService.EditContinent(continentId, editDto);

        // Assert
        result.ContinentName.Should().Be("UpdatedName");
        var updatedContinent = await _continentRepository.GetContinentById(continentId);
        updatedContinent.ContinentName.Should().Be("UpdatedName");
    }

    [Test]
    public async Task DeleteContinent_ShouldRemoveContinent_WhenContinentExists()
    {
        // Arrange
        var geolocationId = await AddGeolocation();
        var continentId = await AddContinent("ToBeDeleted", geolocationId);

        // Act
        await _continentService.DeleteContinent(continentId);

        // Assert
        var continent = await _continentRepository.GetContinentById(continentId);
        continent.Should().BeNull();
    }

    // Helper Methods

    private async Task<int> AddGeolocation()
    {
        var geolocation = new Geolocation { GeolocationId = x, Area = new Polygon(new LinearRing(new[]
        {
            new Coordinate(1, 2),
            new Coordinate(2, 3),
            new Coordinate(3, 2),
            new Coordinate(2, 1),
            new Coordinate(1, 2)
        }))};
        _context.Geolocation.Add(geolocation);
        await _context.SaveChangesAsync();
        x++;
        return geolocation.GeolocationId;
    }

    private async Task<int> AddContinent(string name, int geolocationId)
    {
        var continent = new Continent { ContinentName = name, GeolocationIdFk = geolocationId };
        _context.Continent.Add(continent);
        await _context.SaveChangesAsync();
        return continent.ContinentId;
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}