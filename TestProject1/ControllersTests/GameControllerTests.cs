using System.Security.Claims;
using Backend.Controllers;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NetTopologySuite.Geometries;

namespace TestProject1.ControllersTests;

[TestFixture]
    public class GameControllerTests
    {
        private Mock<IGameService> _gameServiceMock;
        private GameController _controller;

        [SetUp]
        public void SetUp()
        {
            _gameServiceMock = new Mock<IGameService>();
            _controller = new GameController(_gameServiceMock.Object);
        }

        [Test]
        public async Task GetAllGames_ShouldReturnOkResult_WithListOfGames()
        {
            // Arrange
            var games = new List<GameDto> { new GameDto(), new GameDto() };
            _gameServiceMock.Setup(service => service.Retrieve()).ReturnsAsync(games);

            // Act
            var result = await _controller.GetAllGames();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task GetGame_ShouldReturnOkResult_WhenGameIsFound()
        {
            // Arrange
            int gameId = 1;
            var gameDto = new GameDto { GameId = gameId };
            _gameServiceMock.Setup(service => service.Retrieve(gameId)).ReturnsAsync(gameDto);

            // Act
            var result = await _controller.GetGame(gameId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task GetGame_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            int gameId = 1;
            _gameServiceMock.Setup(service => service.Retrieve(gameId))
                .ThrowsAsync(new ArgumentException("Game not found"));

            // Act
            var result = await _controller.GetGame(gameId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task AddGame_ShouldReturnOkResult_WhenGameIsAdded()
        {
            // Arrange
            var gameDto = CreateValidGameDto();
            _gameServiceMock.Setup(service => service.AddGame(gameDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddGame(gameDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task AddGame_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var invalidDto = CreateInvalidGameDto();

            // Act
            var result = await _controller.AddGame(invalidDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task AddAfterGameStatistics_ShouldReturnOkResult_WhenStatisticsAreAdded()
        {
            // Arrange
            var gameDto = CreateValidGameDto();
            string gameType = "Country";
            
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { Request = { Headers = { Authorization = "Bearer testtoken"}}};

            _gameServiceMock.Setup(service => service.AddAfterGameStatistics(gameDto, gameType, "testtoken")).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddAfterGameStatistics(gameDto, gameType);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task EditGame_ShouldReturnOkResult_WhenGameIsUpdated()
        {
            // Arrange
            int gameId = 1;
            var editGameDto = CreateValidEditGameDto();
            var updatedGameDto = CreateValidGameDto();
            updatedGameDto.GameId = gameId;
            _gameServiceMock.Setup(service => service.EditGame(gameId, editGameDto))
                .ReturnsAsync(updatedGameDto);

            // Act
            var result = await _controller.EditGame(gameId, editGameDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task DeleteGame_ShouldReturnOkResult_WhenGameIsDeleted()
        {
            // Arrange
            int gameId = 1;
            _gameServiceMock.Setup(service => service.DeleteGame(gameId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteGame(gameId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        #region Helper Methods

        private GameDto CreateValidGameDto()
        {
            return new GameDto
            {
                GameId = 1,
                UserIdFk = 1,
                StartLocation = new Point(new Coordinate(1,1)),
                GuessedLocation = new Point(new Coordinate(1,1)),
                DistanceToStartingLocation = 10,
                TraveledDistance = 10,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Score = 10
            };
        }

        private GameDto CreateInvalidGameDto()
        {
            return new GameDto
            {
                GameId = -1,
                UserIdFk = 1,
                StartLocation = new Point(new Coordinate(1,1)),
                GuessedLocation = new Point(new Coordinate(1,1)),
                DistanceToStartingLocation = 10,
                TraveledDistance = 10,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Score = -10
            };
        }

        private EditGameDto CreateValidEditGameDto()
        {
            return new EditGameDto
            {
                UserIdFk = 1,
                StartLocation = new Point(new Coordinate(1,1)),
                GuessedLocation = new Point(new Coordinate(1,1)),
                DistanceToStartingLocation = 10,
                TraveledDistance = 10,
                StartTime = DateTime.Now,
                EndTime = DateTime.Now,
                Score = 10
            };
        }

        #endregion
    }