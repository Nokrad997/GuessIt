using Backend.Controllers;
using Backend.Dtos;
using Backend.Services.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestProject1.ControllersTests;

[TestFixture]
public class TokenControllerTests
{
    private Mock<ITokenService> _tokenServiceMock;
    private TokenController _controller;

    [SetUp]
    public void Setup()
    {
        _tokenServiceMock = new Mock<ITokenService>();
        _controller = new TokenController(_tokenServiceMock.Object);
    }

    [Test]
    public void ValidateToken_ShouldReturnBadRequest_WhenTokenIsEmpty()
    {
        // Arrange
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        // Act
        var result = _controller.ValidateToken();

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new { message = "Token is empty" });
    }

    [Test]
    public void ValidateToken_ShouldReturnOk_WhenTokenIsValid()
    {
        // Arrange
        var token = "valid_token";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        _controller.Request.Headers.Authorization = $"Bearer {token}";
        _tokenServiceMock.Setup(service => service.Validate(token)).Returns(true);

        // Act
        var result = _controller.ValidateToken();

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new { message = "Token is valid" });
    }

    [Test]
    public void ValidateToken_ShouldReturnBadRequest_WhenTokenIsInvalid()
    {
        // Arrange
        var token = "invalid_token";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        _controller.Request.Headers.Authorization = $"Bearer {token}";
        _tokenServiceMock.Setup(service => service.Validate(token)).Returns(false);

        // Act
        var result = _controller.ValidateToken();

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().Be("Token is invalid");
    }

    [Test]
    public void ValidateToken_ShouldReturnBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var token = "token_with_error";
        _controller.ControllerContext.HttpContext = new DefaultHttpContext { Request = { Headers = { Authorization = "Bearer testtoken"}}};
        _controller.Request.Headers.Authorization = $"Bearer {token}";
        _tokenServiceMock.Setup(service => service.Validate(token)).Throws(new Exception("Validation error"));

        // Act
        var result = _controller.ValidateToken();

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new { message = "Token is invalid"});
    }

    [Test]
    public void RefreshAccessToken_ShouldReturnBadRequest_WhenRefreshTokenIsEmpty()
    {
        // Arrange
        var tokens = new TokensDto { RefreshToken = string.Empty };

        // Act
        var result = _controller.RefreshAccessToken(tokens);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new { message = "Refresh token is empty" });
    }

    [Test]
    public void RefreshAccessToken_ShouldReturnOk_WhenRefreshTokenIsValid()
    {
        // Arrange
        var refreshToken = "valid_refresh_token";
        var newTokens = new Dictionary<string, string> { ["AccessToken"] = "new_access_token", ["RefreshToken"] = "new_refresh_token" };
        _tokenServiceMock.Setup(service => service.RefreshToken(refreshToken)).Returns(newTokens);

        // Act
        var result = _controller.RefreshAccessToken(new TokensDto { RefreshToken = refreshToken });

        // Assert
        result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().Be(newTokens);
    }

    [Test]
    public void RefreshAccessToken_ShouldReturnBadRequest_WhenExceptionIsThrown()
    {
        // Arrange
        var refreshToken = "token_with_error";
        _tokenServiceMock.Setup(service => service.RefreshToken(refreshToken)).Throws(new Exception("Refresh error"));

        // Act
        var result = _controller.RefreshAccessToken(new TokensDto() { RefreshToken = refreshToken });

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>()
            .Which.Value.Should().BeEquivalentTo(new { message = "Failed in refreshing token", error = "Refresh error" });
    }
}