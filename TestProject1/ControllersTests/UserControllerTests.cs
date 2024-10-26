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
    public class UserControllerTests
    {
        private Mock<IUserService> _userServiceMock;
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Test]
        public async Task GetAllUsers_ShouldReturnOkResult_WhenUsersAreRetrieved()
        {
            // Arrange
            _userServiceMock.Setup(service => service.Retrieve())
                .ReturnsAsync(new List<UserDto>());

            // Act
            var result = await _controller.GetAllUsers();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task GetUser_ShouldReturnOkResult_WhenUserIsRetrieved()
        {
            // Arrange
            int userId = 1;
            _userServiceMock.Setup(service => service.Retrieve(userId))
                .ReturnsAsync(new UserDto());

            // Act
            var result = await _controller.GetUser(userId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task AddUserAsAdmin_ShouldReturnOkResult_WhenUserIsAdded()
        {
            // Arrange
            var userDto = GetValidUserDto();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { Request = { Headers = { Authorization = "Bearer testtoken"}}};
            _userServiceMock.Setup(service => service.AddUserAsAdmin(userDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddUserAsAdmin(userDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task EditUserAsUser_ShouldReturnOkResult_WhenUserIsEdited()
        {
            // Arrange
            int userId = 1;
            var editUserDto = GetValidEditUserDto();
            var token = "valid_token";
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { Request = { Headers = { Authorization = "Bearer testtoken"}}};

            _userServiceMock.Setup(service => service.EditUserAsUser(userId, editUserDto, token))
                .ReturnsAsync(new UserDto());

            // Act
            var result = await _controller.EditUserAsUser(userId, editUserDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task EditUserAsAdmin_ShouldReturnOkResult_WhenUserIsEditedAsAdmin()
        {
            // Arrange
            int userId = 1;
            var editUserDto = GetValidEditUserDto();
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { Request = { Headers = { Authorization = "Bearer testtoken"}}};
            
            _userServiceMock.Setup(service => service.EditUserAsAdmin(userId, editUserDto))
                .ReturnsAsync(new UserDto());

            // Act
            var result = await _controller.EditUserAsAdmin(userId, editUserDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task DeleteUser_ShouldReturnOkResult_WhenUserIsDeleted()
        {
            // Arrange
            int userId = 1;
            var token = "valid_token";
            SetupHttpContextWithToken(token);

            _userServiceMock.Setup(service => service.DeleteUser(userId, token))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteUser(userId);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task GetUserDataBasedOnToken_ShouldReturnOkResult_WhenUserDataIsRetrieved()
        {
            // Arrange
            var token = "valid_token";
            SetupHttpContextWithToken(token);

            _userServiceMock.Setup(service => service.Retrieve(token))
                .ReturnsAsync(new UserDto());

            // Act
            var result = await _controller.GetUserDataBasedOnToken();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        private void SetupHttpContextWithToken(string token)
        {
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { Request = { Headers = { Authorization = "Bearer testtoken"}}};
            _controller.Request.Headers.Authorization = $"Bearer {token}";
        }

        // Private helper methods for creating valid and invalid DTOs
        private UserDto GetValidUserDto()
        {
            return new UserDto()
            {
                UserId = 1,
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123",
                Verified = false,
                IsAdmin = false
            };
        }

        private EditUserDto GetValidEditUserDto()
        {
            return new EditUserDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123",
                Verified = false,
                IsAdmin = false
            };
        }

        private EditUserDto GetInvalidEditUserDto()
        {
            return new EditUserDto()
            {
                Username = "",
                Email = "test@.com",
                Password = "",
                Verified = false,
                IsAdmin = false
            };
        }
    }