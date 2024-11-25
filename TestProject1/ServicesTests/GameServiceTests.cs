using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services;
using Backend.Utility.Interfaces;
using FluentAssertions;
using Moq;

namespace TestProject1.ServicesTests;

[TestFixture]
public class GameServiceTests
{
    private GameService _service;
    private Mock<IGameRepository> _gameRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ITokenUtil> _tokenUtilMock;

    [SetUp]
    public void Setup()
    {
        _gameRepositoryMock = new Mock<IGameRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _tokenUtilMock = new Mock<ITokenUtil>();
        _service = new GameService(_gameRepositoryMock.Object, _userRepositoryMock.Object, _tokenUtilMock.Object);
    }

    [Test]
    public async Task Retrieve_ShouldReturnAllGames()
    {
        // Arrange
        var games = CreateGameList();
        _gameRepositoryMock.Setup(repo => repo.GetGames()).ReturnsAsync(games);

        // Act
        var result = await _service.Retrieve();

        // Assert
        result.Should().BeEquivalentTo(games.Select(g => g.ConvertToDto()));
    }

    [Test]
    public void Retrieve_ShouldThrowException_WhenGameNotFound()
    {
        // Arrange
        _gameRepositoryMock.Setup(repo => repo.GetGameById(It.IsAny<int>())).ReturnsAsync((Game)null);

        // Act
        Func<Task> result = async () => await _service.Retrieve(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Game not found");
    }

    [Test]
    public async Task Retrieve_ShouldReturnGameDto_WhenGameExists()
    {
        // Arrange
        var game = CreateGame();
        _gameRepositoryMock.Setup(repo => repo.GetGameById(It.IsAny<int>())).ReturnsAsync(game);

        // Act
        var result = await _service.Retrieve(game.GameId);

        // Assert
        result.Should().BeEquivalentTo(game.ConvertToDto());
    }

    [Test]
    public void AddGame_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var gameDto = CreateGameDto();
        _userRepositoryMock.Setup(repo => repo.GetUserById(gameDto.UserIdFk)).ReturnsAsync((User)null);

        // Act
        Func<Task> result = async () => await _service.AddGame(gameDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("User not found");
    }

    [Test]
    public async Task AddGame_ShouldAddGame_WhenUserExists()
    {
        // Arrange
        var gameDto = CreateGameDto();
        SetupUserExists(gameDto.UserIdFk);

        // Act
        await _service.AddGame(gameDto);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.AddGame(It.Is<Game>(g =>
            g.UserIdFk == gameDto.UserIdFk)), Times.Once);
    }

    [Test]
    public void AddAfterGameStatistics_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        var gameDto = CreateGameDto();
        _tokenUtilMock.Setup(t => t.GetIdFromToken(It.IsAny<string>())).Returns(1);
        _userRepositoryMock.Setup(repo => repo.GetUserById(1)).ReturnsAsync((User)null);

        // Act
        Func<Task> result = async () => await _service.AddAfterGameStatistics(gameDto, "country", "token");

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("User not found");
    }

    [Test]
    public void EditGame_ShouldThrowException_WhenGameNotFound()
    {
        // Arrange
        var editGameDto = CreateEditGameDto();
        _gameRepositoryMock.Setup(repo => repo.GetGameById(It.IsAny<int>())).ReturnsAsync((Game)null);

        // Act
        Func<Task> result = async () => await _service.EditGame(1, editGameDto);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Game not found");
    }

    [Test]
    public async Task DeleteGame_ShouldDeleteGame_WhenGameExists()
    {
        // Arrange
        var game = CreateGame();
        _gameRepositoryMock.Setup(repo => repo.GetGameById(It.IsAny<int>())).ReturnsAsync(game);

        // Act
        await _service.DeleteGame(game.GameId);

        // Assert
        _gameRepositoryMock.Verify(repo => repo.DeleteGame(game), Times.Once);
    }

    [Test]
    public void DeleteGame_ShouldThrowException_WhenGameNotFound()
    {
        // Arrange
        _gameRepositoryMock.Setup(repo => repo.GetGameById(It.IsAny<int>())).ReturnsAsync((Game)null);

        // Act
        Func<Task> result = async () => await _service.DeleteGame(1);

        // Assert
        result.Should().ThrowAsync<ArgumentException>().WithMessage("Game not found");
    }

    // *** Prywatne metody pomocnicze ***

    private Game CreateGame() =>
        new Game
        {
            GameId = 1,
            UserIdFk = 1,
            // Other properties as needed
        };

    private GameDto CreateGameDto() =>
        new GameDto
        {
            UserIdFk = 1,
            // Other properties as needed
        };

    private GameDto CreateGameDtoWithTimeAndDistance() =>
        new GameDto()
        {
            UserIdFk = 1,
            StartTime = DateTime.Now.AddMinutes(-30),
            EndTime = DateTime.Now,
            DistanceToStartingLocation = 5.0
        };

    private EditGameDto CreateEditGameDto() =>
        new EditGameDto()
        {
            UserIdFk = 1,
            // Other properties as needed
        };

    private List<Game> CreateGameList() =>
        new List<Game>
        {
            new Game { GameId = 1, UserIdFk = 1 },
            new Game { GameId = 2, UserIdFk = 2 }
        };

    private void SetupUserExists(int userId)
    {
        _userRepositoryMock.Setup(repo => repo.GetUserById(userId)).ReturnsAsync(new User() { UserId = userId });
    }
}