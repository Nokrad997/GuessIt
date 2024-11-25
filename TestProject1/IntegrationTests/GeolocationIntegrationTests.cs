using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Context;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories;
using Backend.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NUnit.Framework;

namespace TestProject1.IntegrationTests;

[TestFixture]
public class GeolocationServiceIntegrationTests
{
    private GeolocationService _geolocationService;
    private GeolocationRepository _geolocationRepository;
    private GuessItContext _context;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<GuessItContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new GuessItContext(options);
        _geolocationRepository = new GeolocationRepository(_context);
        _geolocationService = new GeolocationService(_geolocationRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddGeolocation_ShouldAddGeolocation_WhenAreaIsUnique()
    {
        // Arrange
        var area = CreatePolygon();
        var geolocationDto = new GeolocationDto { Area = area };

        // Act
        await _geolocationService.AddGeolocation(geolocationDto);

        // Assert
        var geolocations = await _geolocationRepository.GetGeolocations();
        geolocations.Should().ContainSingle(g => g.Area.Equals(area));
    }

    [Test]
    public async Task AddGeolocation_ShouldThrowException_WhenAreaAlreadyExists()
    {
        // Arrange
        var area = CreatePolygon();
        var geolocationDto = new GeolocationDto { Area = area };
        await _geolocationService.AddGeolocation(geolocationDto);

        var duplicateGeolocationDto = new GeolocationDto { Area = area };

        // Act
        Func<Task> act = async () => await _geolocationService.AddGeolocation(duplicateGeolocationDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Geolocation with provided area already exists");
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllGeolocations()
    {
        // Arrange
        await AddGeolocation(CreatePolygon());
        await AddGeolocation(CreatePolygon(10, 10));

        // Act
        var result = await _geolocationService.Retrieve();

        // Assert
        result.Should().HaveCount(2);
    }

    [Test]
    public async Task EditGeolocation_ShouldUpdateGeolocation_WhenGeolocationExists()
    {
        // Arrange
        var initialArea = CreatePolygon();
        var updatedArea = CreatePolygon(20, 20);
        var geolocationId = await AddGeolocation(initialArea);
        var editDto = new EditGeolocationDto { Area = updatedArea };

        // Act
        var result = await _geolocationService.EditGeolocation(geolocationId, editDto);

        // Assert
        result.Area.Equals(updatedArea).Should().BeTrue();
        var updatedGeolocation = await _geolocationRepository.GetGeolocationById(geolocationId);
        updatedGeolocation.Area.Equals(updatedArea).Should().BeTrue();
    }

    [Test]
    public async Task DeleteGeolocation_ShouldRemoveGeolocation_WhenGeolocationExists()
    {
        // Arrange
        var geolocationId = await AddGeolocation(CreatePolygon());

        // Act
        await _geolocationService.DeleteGeolocation(geolocationId);

        // Assert
        var geolocation = await _geolocationRepository.GetGeolocationById(geolocationId);
        geolocation.Should().BeNull();
    }

    [Test]
    public async Task Retrieve_ShouldReturnGeolocation_WhenIdExists()
    {
        // Arrange
        var area = CreatePolygon();
        var geolocationId = await AddGeolocation(area);

        // Act
        var result = await _geolocationService.Retrieve(geolocationId);

        // Assert
        result.Should().NotBeNull();
        result.Area.Equals(area).Should().BeTrue();
    }

    private async Task<int> AddGeolocation(Geometry area)
    {
        var geolocation = new Geolocation { Area = area };
        _context.Geolocation.Add(geolocation);
        await _context.SaveChangesAsync();
        return geolocation.GeolocationId;
    }

    private Geometry CreatePolygon(double offsetX = 0, double offsetY = 0)
    {
        var coordinates = new[]
        {
            new Coordinate(offsetX, offsetY),
            new Coordinate(offsetX + 1, offsetY),
            new Coordinate(offsetX + 1, offsetY + 1),
            new Coordinate(offsetX, offsetY + 1),
            new Coordinate(offsetX, offsetY)
        };

        return new Polygon(new LinearRing(coordinates));
    }
}
