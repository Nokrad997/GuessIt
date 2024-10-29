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
public class CountryServiceIntegrationTests
{
    private CountryService _countryService;
    private CountryRepository _countryRepository;
    private GeolocationRepository _geolocationRepository;
    private GuessItContext _context;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<GuessItContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unikalna baza dla każdego testu
            .Options;

        _context = new GuessItContext(options);
        _countryRepository = new CountryRepository(_context);
        _geolocationRepository = new GeolocationRepository(_context);
        _countryService = new CountryService(_countryRepository, _geolocationRepository);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddCountry_ShouldAddNewCountry_WhenGeolocationAndNameAreUnique()
    {
        // Arrange
        var geolocationId = await AddGeolocation();
        var countryDto = new CountryDto { CountryName = "New Country", GeolocationIdFk = geolocationId };

        // Act
        await _countryService.AddCountry(countryDto);

        // Assert
        var countries = await _countryRepository.GetCountries();
        countries.Should().ContainSingle(c => c.CountryName == "New Country" && c.GeolocationIdFk == geolocationId);
    }

    [Test]
    public async Task AddCountry_ShouldThrowException_WhenCountryNameExists()
    {
        // Arrange
        var geolocationId = await AddGeolocation();
        var countryDto = new CountryDto { CountryName = "DuplicateName", GeolocationIdFk = geolocationId };
        await _countryService.AddCountry(countryDto);

        var duplicateCountryDto = new CountryDto { CountryName = "DuplicateName", GeolocationIdFk = geolocationId + 1 };

        // Act
        Func<Task> act = async () => await _countryService.AddCountry(duplicateCountryDto);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Country with provided name already exists");
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllCountries()
    {
        // Arrange
        var geolocationId = await AddGeolocation();
        await AddCountry("Country1", geolocationId);
        await AddCountry("Country2", geolocationId);

        // Act
        var result = await _countryService.Retrieve();

        // Assert
        result.Should().HaveCount(2);
        result.Select(c => c.CountryName).Should().Contain(new[] { "Country1", "Country2" });
    }

    [Test]
    public async Task RetrieveByContinentId_ShouldReturnCountriesForSpecificContinent()
    {
        // Arrange
        var continentId = 1; // Zakładamy kontynent o identyfikatorze 1
        var geolocationId = await AddGeolocation();
        await AddCountry("Country1", geolocationId, continentId);
        await AddCountry("Country2", geolocationId, continentId);

        // Act
        var result = await _countryService.RetrieveByContinentId(continentId);

        // Assert
        result.Should().HaveCount(2);
        result.Select(c => c.CountryName).Should().Contain(new[] { "Country1", "Country2" });
    }

    [Test]
    public async Task EditCountry_ShouldUpdateCountry_WhenDataIsValid()
    {
        // Arrange
        var geolocationId = await AddGeolocation();
        var countryId = await AddCountry("OldName", geolocationId);

        var editDto = new EditCountryDto { CountryName = "UpdatedName", GeolocationIdFk = geolocationId };

        // Act
        var result = await _countryService.EditCountry(countryId, editDto);

        // Assert
        result.CountryName.Should().Be("UpdatedName");
        var updatedCountry = await _countryRepository.GetCountryById(countryId);
        updatedCountry.CountryName.Should().Be("UpdatedName");
    }

    [Test]
    public async Task DeleteCountry_ShouldRemoveCountry_WhenCountryExists()
    {
        // Arrange
        var geolocationId = await AddGeolocation();
        var countryId = await AddCountry("ToBeDeleted", geolocationId);

        // Act
        await _countryService.DeleteCountry(countryId);

        // Assert
        var country = await _countryRepository.GetCountryById(countryId);
        country.Should().BeNull();
    }

    // Helper Methods

    private async Task<int> AddGeolocation()
    {
        var geolocation = new Geolocation { GeolocationId = 1, Area = new Polygon(new LinearRing(new[]
        {
            new Coordinate(1, 2),
            new Coordinate(2, 3),
            new Coordinate(3, 2),
            new Coordinate(2, 1),
            new Coordinate(1, 2)
        }))};
        _context.Geolocation.Add(geolocation);
        await _context.SaveChangesAsync();
        return geolocation.GeolocationId;
    }

    private async Task<int> AddCountry(string name, int geolocationId, int continentId = 1)
    {
        var country = new Country { CountryName = name, GeolocationIdFk = geolocationId, ContinentIdFk = continentId };
        _context.Country.Add(country);
        await _context.SaveChangesAsync();
        return country.CountryId;
    }
}
