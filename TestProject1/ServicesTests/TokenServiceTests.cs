using Backend.Services;
using Backend.Utility.Interfaces;
using FluentAssertions;
using Moq;

namespace TestProject1.ServicesTests;

[TestFixture]
public class TokenServiceTests
{
    private TokenService _service;
    private Mock<ITokenUtil> _tokenUtilMock;

    [SetUp]
    public void Setup()
    {
        _tokenUtilMock = new Mock<ITokenUtil>();
        _service = new TokenService(_tokenUtilMock.Object);
    }

    [Test]
    public void Validate_ShouldReturnTrue_WhenTokenIsValid()
    {
        // Arrange
        string token = "validToken";
        _tokenUtilMock.Setup(t => t.ValidateToken(token)).Returns(true);

        // Act
        var result = _service.Validate(token);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Validate_ShouldReturnFalse_WhenTokenIsInvalid()
    {
        // Arrange
        string token = "invalidToken";
        _tokenUtilMock.Setup(t => t.ValidateToken(token)).Returns(false);

        // Act
        var result = _service.Validate(token);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void RefreshToken_ShouldReturnNewTokens_WhenTokenIsValid()
    {
        // Arrange
        string token = "validToken";
        var newTokens = new Dictionary<string, string>
        {
            { "accessToken", "newAccessToken" },
            { "refreshToken", "newRefreshToken" }
        };
        _tokenUtilMock.Setup(t => t.RefreshAccessToken(token)).Returns(newTokens);

        // Act
        var result = _service.RefreshToken(token);

        // Assert
        result.Should().BeEquivalentTo(newTokens);
    }

    [Test]
    public void RefreshToken_ShouldThrowException_WhenTokenIsInvalid()
    {
        // Arrange
        string token = "invalidToken";
        _tokenUtilMock.Setup(t => t.RefreshAccessToken(token))
            .Throws(new ArgumentException("Invalid refresh token"));

        // Act
        Action act = () => _service.RefreshToken(token);

        // Assert
        act.Should().Throw<ArgumentException>().WithMessage("Invalid refresh token");
    }
}