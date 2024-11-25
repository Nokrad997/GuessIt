using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Backend.Services;
using Backend.Utility;
using FluentAssertions;
using NUnit.Framework;

namespace TestProject1.IntegrationTests;

[TestFixture]
public class MonthlyUsageServiceIntegrationTests
{
    private string _testFilePath;
    private MonthlyUsageService _monthlyUsageService;

    [SetUp]
    public void SetUp()
    {
        _testFilePath = $"TestFiles/MonthlyUsage_{Guid.NewGuid()}.json";
        _monthlyUsageService = new MonthlyUsageService(_testFilePath);
    }

    [TearDown]
    public void TearDown()
    {
        if (File.Exists(_testFilePath))
        {
            File.Delete(_testFilePath);
        }
    }

    [Test]
    public async Task UpdateMonthlyUsage_ShouldIncrementUsage_WhenCalledWithinSameMonth()
    {
        // Arrange
        await _monthlyUsageService.UpdateMonthlyUsage(100);

        // Act
        var result = await _monthlyUsageService.UpdateMonthlyUsage(50);

        // Assert
        result.Should().Be("Monthly usage updated to: 150");
        var monthlyUsage = await _monthlyUsageService.GetMonthlyUsage();
        monthlyUsage.Should().Be(150);
    }

    [Test]
    public async Task UpdateMonthlyUsage_ShouldResetUsage_WhenCalledInNewMonth()
    {
        // Arrange
        var initialData = new MonthlyUsageData
        {
            MonthlyUsage = 200,
            LastUpdated = DateTime.UtcNow.AddMonths(-1)
        };
        await SaveTestMonthlyUsageData(initialData);

        // Act
        var result = await _monthlyUsageService.UpdateMonthlyUsage(100);

        // Assert
        result.Should().Be("Monthly usage updated to: 100");
        var monthlyUsage = await _monthlyUsageService.GetMonthlyUsage();
        monthlyUsage.Should().Be(100);
    }

    [Test]
    public async Task GetMonthlyUsage_ShouldReturnZero_WhenFileDoesNotExist()
    {
        // Act
        var monthlyUsage = await _monthlyUsageService.GetMonthlyUsage();

        // Assert
        monthlyUsage.Should().Be(0);
    }

    [Test]
    public async Task GetMonthlyUsage_ShouldReturnStoredUsage_WhenFileExists()
    {
        // Arrange
        var initialData = new MonthlyUsageData
        {
            MonthlyUsage = 300,
            LastUpdated = DateTime.UtcNow
        };
        await SaveTestMonthlyUsageData(initialData);

        // Act
        var monthlyUsage = await _monthlyUsageService.GetMonthlyUsage();

        // Assert
        monthlyUsage.Should().Be(300);
    }

    [Test]
    public async Task EnsureMonthlyUsageData_ShouldHandleInvalidJsonFile_AndResetUsage()
    {
        // Arrange
        await File.WriteAllTextAsync(_testFilePath, "Invalid JSON Content");

        // Act
        var monthlyUsage = await _monthlyUsageService.GetMonthlyUsage();

        // Assert
        monthlyUsage.Should().Be(0);
    }

    // Helper Methods

    private async Task SaveTestMonthlyUsageData(MonthlyUsageData data)
    {
        var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        var directory = Path.GetDirectoryName(_testFilePath);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        await File.WriteAllTextAsync(_testFilePath, jsonData);
    }
}
