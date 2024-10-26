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
    public class UserAchievementsControllerTests
    {
        private Mock<IUserAchievementService> _userAchievementsServiceMock;
        private UserAchievementsController _controller;

        [SetUp]
        public void Setup()
        {
            _userAchievementsServiceMock = new Mock<IUserAchievementService>();
            _controller = new UserAchievementsController(_userAchievementsServiceMock.Object);
        }

        [Test]
        public async Task GetAllUserAchievements_ShouldReturnOkResult_WhenUserAchievementsAreRetrieved()
        {
            // Arrange
            _userAchievementsServiceMock.Setup(service => service.Retrieve())
                .ReturnsAsync(new List<UserAchievementsDtos>());

            // Act
            var result = await _controller.GetAllUserAchievements();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task GetUserAchievements_ShouldReturnOkResult_WhenUserAchievementIsRetrieved()
        {
            // Arrange
            int achievementId = 1;
            _userAchievementsServiceMock.Setup(service => service.Retrieve(achievementId))
                .ReturnsAsync(new UserAchievementsDtos());

            // Act
            var result = await _controller.GetUserAchievements(achievementId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task GetUserAchievements_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            int achievementId = 1;
            _userAchievementsServiceMock.Setup(service => service.Retrieve(achievementId))
                .ThrowsAsync(new ArgumentException("Achievement not found"));

            // Act
            var result = await _controller.GetUserAchievements(achievementId);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task GetUserAssociatedAchievements_ShouldReturnOkResult_WhenUserAchievementsAreRetrieved()
        {
            // Arrange
            var token = "valid_token";
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { Request = { Headers = { Authorization = "Bearer testtoken"}}};
            _controller.Request.Headers.Authorization = $"Bearer {token}";

            _userAchievementsServiceMock.Setup(service => service.RetrieveUserAchievements(token))
                .ReturnsAsync(new List<AchievementDto>());

            // Act
            var result = await _controller.GetUserAssociatedAchievements();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task AddUserAchievements_ShouldReturnOkResult_WhenUserAchievementIsAdded()
        {
            // Arrange
            var userAchievementDto = GetValidUserAchievementsDto();

            _userAchievementsServiceMock.Setup(service => service.AddUserAchievement(userAchievementDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddUserAchievements(userAchievementDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task EditUserAchievements_ShouldReturnOkResult_WhenUserAchievementIsEdited()
        {
            // Arrange
            int id = 1;
            var editUserAchievementDto = GetValidEditUserAchievementDto();

            _userAchievementsServiceMock.Setup(service => service.EditUserAchievement(id, editUserAchievementDto))
                .ReturnsAsync(new UserAchievementsDtos());

            // Act
            var result = await _controller.EditUserAchievements(id, editUserAchievementDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task DeleteUserAchievements_ShouldReturnOkResult_WhenUserAchievementIsDeleted()
        {
            // Arrange
            int id = 1;

            _userAchievementsServiceMock.Setup(service => service.DeleteUserAchievement(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUserAchievements(id);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        private UserAchievementsDtos GetValidUserAchievementsDto()
        {
            return new UserAchievementsDtos()
            {
                UserAchievementId = 1,
                UserIdFk = 1,
                AchievementIdFk = 1
            };
        }

        private EditUserAchievementDtos GetValidEditUserAchievementDto()
        {
            return new EditUserAchievementDtos()
            {
                UserIdFk = 1,
                AchievementIdFk = 1
            };
        }
    }