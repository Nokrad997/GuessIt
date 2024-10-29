using System.Text.Json;
using Backend.Services;
using Backend.Utility;
using FluentAssertions;

namespace TestProject1.ServicesTests;

[TestFixture]
public class MonthlyUsageServiceTests
{
    private MonthlyUsageService _service;
    private string _testFilePath = "Utility/MonthlyUsageTest.json";

    [SetUp]
    public void Setup()
    {

        _service = new MonthlyUsageService(_testFilePath);
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [TearDown]
    public void Teardown()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Test]
    public async Task UpdateMonthlyUsage_ShouldInitializeFile_WhenFileDoesNotExist()
    {
        // Arrange
        var usage = 100;

        // Act
        var result = await _service.UpdateMonthlyUsage(usage);

        // Assert
        result.Should().Be($"Monthly usage updated to: {usage}");

        var fileContent = await File.ReadAllTextAsync(_testFilePath);
        var monthlyUsageData = JsonSerializer.Deserialize<MonthlyUsageData>(fileContent);

        monthlyUsageData.Should().NotBeNull();
        monthlyUsageData.MonthlyUsage.Should().Be(usage);
    }

    [Test]
    public async Task UpdateMonthlyUsage_ShouldResetUsage_WhenNewMonthBegins()
    {
        // Arrange
        var monthlyUsageData = new MonthlyUsageData
        {
            MonthlyUsage = 500,
            LastUpdated = DateTime.UtcNow.AddMonths(-1)
        };

        await File.WriteAllTextAsync(_testFilePath, JsonSerializer.Serialize(monthlyUsageData));

        // Act
        var result = await _service.UpdateMonthlyUsage(100);

        // Assert
        result.Should().Be("Monthly usage updated to: 100");

        var fileContent = await File.ReadAllTextAsync(_testFilePath);
        var updatedData = JsonSerializer.Deserialize<MonthlyUsageData>(fileContent);

        updatedData.Should().NotBeNull();
        updatedData.MonthlyUsage.Should().Be(100);
    }

    [Test]
    public async Task UpdateMonthlyUsage_ShouldAccumulateUsage_WhenInSameMonth()
    {
        // Arrange
        var monthlyUsageData = new MonthlyUsageData
        {
            MonthlyUsage = 200,
            LastUpdated = DateTime.UtcNow
        };

        await File.WriteAllTextAsync(_testFilePath, JsonSerializer.Serialize(monthlyUsageData));

        // Act
        var result = await _service.UpdateMonthlyUsage(150);

        // Assert
        result.Should().Be("Monthly usage updated to: 350");

        var fileContent = await File.ReadAllTextAsync(_testFilePath);
        var updatedData = JsonSerializer.Deserialize<MonthlyUsageData>(fileContent);

        updatedData.Should().NotBeNull();
        updatedData.MonthlyUsage.Should().Be(350);
    }

    [Test]
    public async Task GetMonthlyUsage_ShouldReturnCorrectUsage_WhenFileExists()
    {
        // Arrange
        var monthlyUsageData = new MonthlyUsageData
        {
            MonthlyUsage = 300,
            LastUpdated = DateTime.UtcNow
        };

        await File.WriteAllTextAsync(_testFilePath, JsonSerializer.Serialize(monthlyUsageData));

        // Act
        var usage = await _service.GetMonthlyUsage();

        // Assert
        usage.Should().Be(300);
    }

    [Test]
    public async Task GetMonthlyUsage_ShouldReturnZero_WhenFileDoesNotExist()
    {
        // Act
        var usage = await _service.GetMonthlyUsage();

        // Assert
        usage.Should().Be(0);

        var fileExists = File.Exists(_testFilePath);
        fileExists.Should().BeTrue("file should be created if it does not exist initially");
    }

    [Test]
    public async Task EnsureMonthlyUsageData_ShouldCreateNewFile_WhenFileIsCorrupted()
    {
        // Arrange
        await File.WriteAllTextAsync(_testFilePath, "Invalid JSON");

        // Act
        var usage = await _service.GetMonthlyUsage();

        // Assert
        usage.Should().Be(0);

        var fileContent = await File.ReadAllTextAsync(_testFilePath);
        var monthlyUsageData = JsonSerializer.Deserialize<MonthlyUsageData>(fileContent);

        monthlyUsageData.Should().NotBeNull();
        monthlyUsageData.MonthlyUsage.Should().Be(0);
    }
}