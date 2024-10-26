using Backend.Controllers;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject1.ControllersTests;

[TestFixture]
    public class StatisticsControllerTests
    {
        private Mock<IStatisticsService> _statisticsServiceMock;
        private StatisticsController _controller;

        [SetUp]
        public void Setup()
        {
            _statisticsServiceMock = new Mock<IStatisticsService>();
            _controller = new StatisticsController(_statisticsServiceMock.Object);
        }

        [Test]
        public async Task GetStatistics_ShouldReturnOkResult_WhenStatisticsAreRetrieved()
        {
            // Arrange
            _statisticsServiceMock.Setup(service => service.Retrieve())
                .ReturnsAsync(new List<StatisticsDto>{GetValidStatisticsDto()});

            // Act
            var result = await _controller.GetStatistics();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task GetAllStatistics_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            int id = 1;
            _statisticsServiceMock.Setup(service => service.Retrieve(id))
                .ThrowsAsync(new Exception("Error retrieving statistics"));

            // Act
            var result = await _controller.GetAllStatistics(id);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task AddStatistics_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var invalidDto = GetInvalidStatisticsDto();

            // Act
            var result = await _controller.AddStatistics(invalidDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task AddStatistics_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var statisticsDto = GetValidStatisticsDto();
            _statisticsServiceMock.Setup(service => service.AddStatistics(It.IsAny<StatisticsDto>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception("Error adding statistics"));

            // Act
            var result = await _controller.AddStatistics(statisticsDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task EditStatistics_ShouldReturnOkResult_WhenEditIsSuccessful()
        {
            // Arrange
            int id = 1;
            _controller.ControllerContext.HttpContext = new DefaultHttpContext() { Request = { Headers = { Authorization = "Bearer testtoken"}}};
            var editDto = GetValidEditStatisticsDto();
            _statisticsServiceMock.Setup(service => service.EditStatistics(id, editDto, It.IsAny<string>()))
                .ReturnsAsync(GetValidStatisticsDto());

            // Act
            var result = await _controller.EditStatistics(id, editDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task DeleteStatistics_ShouldReturnOkResult_WhenDeleteIsSuccessful()
        {
            // Arrange
            int id = 1;
            _statisticsServiceMock.Setup(service => service.DeleteStatistics(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteStatistics(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task DeleteStatistics_ShouldReturnBadRequest_WhenExceptionThrown()
        {
            // Arrange
            int id = 1;
            _statisticsServiceMock.Setup(service => service.DeleteStatistics(id))
                .ThrowsAsync(new Exception("Error deleting statistics"));

            // Act
            var result = await _controller.DeleteStatistics(id);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }
        
        private StatisticsDto GetValidStatisticsDto()
        {
            return new StatisticsDto
            {
                StatisticId = 1,
                UserIdFk = 1,
                TotalPoints = 1,
                TotalGames = 1,
                HighestScore = 1,
                LowestTimeInSeconds = 1,
                TotalTraveledDistanceInMeters = 2,
                AverageScore = 1
            };
        }

        private StatisticsDto GetInvalidStatisticsDto()
        {
            return new StatisticsDto()
            {
                StatisticId = 1,
                UserIdFk = 1,
                TotalPoints = 1,
                TotalGames = 1,
                HighestScore = 1,
                LowestTimeInSeconds = 1,
                TotalTraveledDistanceInMeters = 2,
                AverageScore = 1
            };
        }

        private EditStatisticsDto GetValidEditStatisticsDto()
        {
            return new EditStatisticsDto()
            {
                UserIdFk = 1,
                TotalPoints = 1,
                TotalGames = 1,
                HighestScore = 1,
                LowestTimeInSeconds = 1,
                TotalTraveledDistanceInMeters = 2,
                AverageScore = 1
            };
        }
    }