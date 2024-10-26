using Backend.Controllers;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Backend.Services.Interfaces;

namespace TestProject1.AuthControllerTests
{
    [TestFixture]
    public class AuthControllerTests
    {
        private Mock<IAuthService> _authServiceMock;
        private AuthController _controller;

        [SetUp]
        public void SetUp()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController(_authServiceMock.Object);
        }

        [Test]
        public async Task RegisterUser_ShouldReturnOkResult_WhenUserIsRegistered()
        {
            // Arrange
            var registerUserDto = GetValidEditUserDto();
            _authServiceMock.Setup(service => service.RegisterUser(registerUserDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RegisterUser(registerUserDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(200);
        }

        [Test]
        public async Task RegisterUser_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var invalidDto = new EditUserDto();

            // Act
            var result = await _controller.RegisterUser(invalidDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task RegisterUser_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var registerUserDto = new EditUserDto();
            _authServiceMock.Setup(service => service.RegisterUser(registerUserDto))
                .ThrowsAsync(new Exception("Registration failed"));

            // Act
            var result = await _controller.RegisterUser(registerUserDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task LoginUser_ShouldReturnBadRequest_WhenValidationFails()
        {
            // Arrange
            var invalidDto = new AuthUserDto();

            // Act
            var result = await _controller.LoginUser(invalidDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        [Test]
        public async Task LoginUser_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var authUserDto = GetValidAuthUserDto();
            _authServiceMock.Setup(service => service.LoginUser(authUserDto))
                .ThrowsAsync(new Exception("Login failed"));

            // Act
            var result = await _controller.LoginUser(authUserDto);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.StatusCode.Should().Be(400);
        }

        private AuthUserDto GetValidAuthUserDto()
        {
            return new AuthUserDto
            {
                Email = "test@test.com",
                Password = "testtest"
            };
        }

        private EditUserDto GetValidEditUserDto()
        {
            return new EditUserDto
            {
                Email = "test@test.com",
                Password = "testtest",
                Username = "test",
                IsAdmin = false,
                Verified = false
            };
        }
    }
}
