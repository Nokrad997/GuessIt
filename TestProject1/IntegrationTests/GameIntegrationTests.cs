using Backend.Context;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories;
using Backend.Services;
using Backend.Utility.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NetTopologySuite.Geometries;


namespace TestProject1.IntegrationTests;

[TestFixture]
public class GameServiceIntegrationTests
{
    private GameService _gameService;
    private GameRepository _gameRepository;
    private UserRepository _userRepository;
    private Mock<ITokenUtil> _tokenUtilMock;
    private GuessItContext _context;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<GuessItContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) 
            .Options;

        _context = new GuessItContext(options);
        _gameRepository = new GameRepository(_context);
        _userRepository = new UserRepository(_context);
        _tokenUtilMock = new Mock<ITokenUtil>();

        _gameService = new GameService(_gameRepository, _userRepository, _tokenUtilMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddGame_ShouldAddGame_WhenUserExists()
    {
        // Arrange
        var userId = await AddUser();
        var gameDto = new GameDto { UserIdFk = userId, Score = 100, DistanceToStartingLocation = 50, StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddMinutes(5), StartLocation = new Point(new Coordinate(1,1)),
            GuessedLocation = new Point(new Coordinate(1,1)), };

        // Act
        await _gameService.AddGame(gameDto);

        // Assert
        var games = await _gameRepository.GetGames();
        games.Should().ContainSingle(g => g.UserIdFk == userId && g.Score == 100);
    }

    [Test]
    public async Task AddAfterGameStatistics_ShouldCalculateAndAddGame_WhenTokenAndUserAreValid()
    {
        // Arrange
        var userId = await AddUser();
        var gameDto = new GameDto { Score = 0, DistanceToStartingLocation = 100, StartTime = DateTime.UtcNow, EndTime = DateTime.UtcNow.AddMinutes(2), StartLocation = new Point(new Coordinate(1,1)),
            GuessedLocation = new Point(new Coordinate(1,1)), };
        
        _tokenUtilMock.Setup(t => t.GetIdFromToken(It.IsAny<string>())).Returns(userId);

        // Act
        await _gameService.AddAfterGameStatistics(gameDto, "country", "validToken");

        // Assert
        var games = await _gameRepository.GetGames();
        games.Should().ContainSingle(g => g.UserIdFk == userId && g.Score > 0); // Sprawdzamy, czy wynik został policzony
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllGames()
    {
        // Arrange
        var userId = await AddUser();
        await AddGame(userId, 150);
        await AddGame(userId, 200);

        // Act
        var result = await _gameService.Retrieve();

        // Assert
        result.Should().HaveCount(2);
        result.Select(g => g.Score).Should().Contain(new[] { 150, 200 });
    }

    [Test]
    public async Task EditGame_ShouldUpdateGame_WhenGameAndUserExist()
    {
        // Arrange
        var userId = await AddUser();
        var gameId = await AddGame(userId, 100);

        var editDto = new EditGameDto { UserIdFk = userId, Score = 300 };

        // Act
        var result = await _gameService.EditGame(gameId, editDto);

        // Assert
        result.Score.Should().Be(300);
        var updatedGame = await _gameRepository.GetGameById(gameId);
        updatedGame.Score.Should().Be(300);
    }

    [Test]
    public async Task DeleteGame_ShouldRemoveGame_WhenGameExists()
    {
        // Arrange
        var userId = await AddUser();
        var gameId = await AddGame(userId, 100);

        // Act
        await _gameService.DeleteGame(gameId);

        // Assert
        var game = await _gameRepository.GetGameById(gameId);
        game.Should().BeNull();
    }

    // Helper Methods

    private async Task<int> AddUser()
    {
        var user = new User { Username = "TestUser", Password = "testtest", Email = "test@test.com"};
        _context.User.Add(user);
        await _context.SaveChangesAsync();
        return user.UserId;
    }

    private async Task<int> AddGame(int userId, int score)
    {
        var game = new Game
        {
            UserIdFk = userId,
            Score = score,
            DistanceToStartingLocation = 100,
            StartTime = DateTime.UtcNow,
            EndTime = DateTime.UtcNow.AddMinutes(5),
            StartLocation = new Point(new Coordinate(1,1)),
            GuessedLocation = new Point(new Coordinate(1,1))
        };
        _context.Game.Add(game);
        await _context.SaveChangesAsync();
        return game.GameId;
    }
}
