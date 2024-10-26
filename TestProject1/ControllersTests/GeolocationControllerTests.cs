using Backend.Controllers;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NetTopologySuite.Geometries;

namespace TestProject1.ControllersTests;

[TestFixture]
    public class GeolocationControllerTests
    {
        private Mock<IGeolocationService> _geolocationServiceMock;
        private GeolocationController _controller;

        [SetUp]
        public void SetUp()
        {
            _geolocationServiceMock = new Mock<IGeolocationService>();
            _controller = new GeolocationController(_geolocationServiceMock.Object);
        }

        [Test]
        public async Task GetGeolocationById_ShouldReturnOkResult_WhenGeolocationIsFound()
        {
            // Arrange
            int geolocationId = 1;
            var geolocationDto = CreateValidGeolocationDto();
            _geolocationServiceMock.Setup(service => service.Retrieve(geolocationId))
                .ReturnsAsync(geolocationDto);

            // Act
            var result = await _controller.GetGeolocationById(geolocationId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task GetGeolocationById_ShouldReturnBadRequest_WhenGeolocationNotFound()
        {
            // Arrange
            int geolocationId = 1;
            _geolocationServiceMock.Setup(service => service.Retrieve(geolocationId))
                .ThrowsAsync(new ArgumentException("Geolocation not found"));

            // Act
            var result = await _controller.GetGeolocationById(geolocationId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task GetAllGeolocations_ShouldReturnOkResult_WithListOfGeolocations()
        {
            // Arrange
            var geolocations = new List<GeolocationDto> { CreateValidGeolocationDto(), CreateValidGeolocationDto() };
            _geolocationServiceMock.Setup(service => service.Retrieve()).ReturnsAsync(geolocations);

            // Act
            var result = await _controller.GetAllGeolocations();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task CreateGeolocation_ShouldReturnOkResult_WhenGeolocationIsCreated()
        {
            // Arrange
            var geolocationDto = CreateValidGeolocationDto();
            _geolocationServiceMock.Setup(service => service.AddGeolocation(geolocationDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateGeolocation(geolocationDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task CreateGeolocation_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var invalidGeolocationDto = CreateInvalidGeolocationDto();

            // Act
            var result = await _controller.CreateGeolocation(invalidGeolocationDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task EditGeolocation_ShouldReturnOkResult_WhenGeolocationIsUpdated()
        {
            // Arrange
            int geolocationId = 1;
            var editGeolocationDto = CreateValidEditGeolocationDto();
            var updatedGeolocationDto = CreateValidGeolocationDto();
            _geolocationServiceMock.Setup(service => service.EditGeolocation(geolocationId, editGeolocationDto))
                .ReturnsAsync(updatedGeolocationDto);

            // Act
            var result = await _controller.EditGeolocation(geolocationId, editGeolocationDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task DeleteGeolocation_ShouldReturnOkResult_WhenGeolocationIsDeleted()
        {
            // Arrange
            int geolocationId = 1;
            _geolocationServiceMock.Setup(service => service.DeleteGeolocation(geolocationId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteGeolocation(geolocationId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        #region Helper Methods

        private GeolocationDto CreateValidGeolocationDto()
        {
            return new GeolocationDto
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
        }

        private GeolocationDto CreateInvalidGeolocationDto()
        {
            return new GeolocationDto()
            {
                GeolocationId = -2,
                Area = null
            };
        }

        private EditGeolocationDto CreateValidEditGeolocationDto()
        {
            return new EditGeolocationDto()
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
        }

        #endregion
    }