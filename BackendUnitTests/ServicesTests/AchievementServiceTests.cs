using Backend.Dtos;
using Backend.Entities;
using Backend.Repositories;
using Backend.Services;
using FluentAssertions;
using Moq;

namespace BackendUnitTests.ServicesTests;

[TestFixture]
public class AchievementServiceTests
{
    private Mock<AchievementRepository> _achievementRepositoryMock;
        private AchievementService _achievementService;

        [SetUp]
        public void Setup()
        {
            _achievementRepositoryMock = new Mock<AchievementRepository>();
            _achievementService = new AchievementService(_achievementRepositoryMock.Object);
        }

        [Test]
        public async Task Retrieve_ShouldReturnAllAchievements_AsDtos()
        {
            // Arrange
            var achievements = new List<Achievement>
            {
                new Achievement { AchievementId = 1, AchievementName = "First Achievement" },
                new Achievement { AchievementId = 2, AchievementName = "Second Achievement" }
            };

            _achievementRepositoryMock.Setup(repo => repo.GetAllAchievements()).ReturnsAsync(achievements);

            // Act
            var result = await _achievementService.Retrieve();

            // Assert
            result.Should().HaveCount(2)
                .And.Contain(a => a.AchievementName == "First Achievement")
                .And.Contain(a => a.AchievementName == "Second Achievement");

            _achievementRepositoryMock.Verify(repo => repo.GetAllAchievements(), Times.Once);
        }

        [Test]
        public async Task Retrieve_ById_ShouldReturnAchievement_AsDto()
        {
            // Arrange
            var achievement = new Achievement { AchievementId = 1, AchievementName = "Test Achievement" };
            _achievementRepositoryMock.Setup(repo => repo.GetAchievementById(1)).ReturnsAsync(achievement);

            // Act
            var result = await _achievementService.Retrieve(1);

            // Assert
            result.AchievementName.Should().Be("Test Achievement");
            _achievementRepositoryMock.Verify(repo => repo.GetAchievementById(1), Times.Once);
        }

        [Test]
        public async Task Retrieve_ById_ShouldThrowArgumentException_WhenAchievementNotFound()
        {
            // Arrange
            _achievementRepositoryMock.Setup(repo => repo.GetAchievementById(It.IsAny<int>())).ReturnsAsync((Achievement)null);

            // Act
            Func<Task> act = async () => await _achievementService.Retrieve(1);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("No achievement with provided id");
        }

        [Test]
        public async Task AddAchievement_ShouldAddAchievement_WhenCriteriaAndNameAreUnique()
        {
            // Arrange
            var dto = new AchievementDto() { AchievementName = "Unique Achievement", AchievementCriteria = new Dictionary<string, object> { { "points", 100 } } };

            _achievementRepositoryMock.Setup(repo => repo.GetAchievementByName(dto.AchievementName)).ReturnsAsync((Achievement)null);
            _achievementRepositoryMock.Setup(repo => repo.GetAchievementByCriteria(dto.AchievementCriteria)).ReturnsAsync((Achievement)null);

            // Act
            Func<Task> act = async () => await _achievementService.AddAchievement(dto);

            // Assert
            await act.Should().NotThrowAsync();
            _achievementRepositoryMock.Verify(repo => repo.AddAchievement(It.IsAny<Achievement>()), Times.Once);
        }

        [Test]
        public async Task AddAchievement_ShouldThrowArgumentException_WhenAchievementNameExists()
        {
            // Arrange
            var dto = new AchievementDto { AchievementName = "Duplicate Achievement", AchievementCriteria = new Dictionary<string, object> { { "points", 100 } } };
            _achievementRepositoryMock.Setup(repo => repo.GetAchievementByName(dto.AchievementName)).ReturnsAsync(new Achievement());

            // Act
            Func<Task> act = async () => await _achievementService.AddAchievement(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Achievement with provided name already exists");
        }

        [Test]
        public async Task AddAchievement_ShouldThrowArgumentException_WhenAchievementCriteriaExists()
        {
            // Arrange
            var dto = new AchievementDto { AchievementName = "New Achievement", AchievementCriteria = new Dictionary<string, object> { { "points", 100 } } };
            var existingAchievement = new Achievement { AchievementCriteria = new Dictionary<string, object> { { "points", 100 } } };

            _achievementRepositoryMock.Setup(repo => repo.GetAchievementByName(dto.AchievementName)).ReturnsAsync((Achievement)null);
            _achievementRepositoryMock.Setup(repo => repo.GetAchievementByCriteria(dto.AchievementCriteria)).ReturnsAsync(existingAchievement);

            // Act
            Func<Task> act = async () => await _achievementService.AddAchievement(dto);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Achievement with provided criteria already exists: points: 100");
        }

        [Test]
        public async Task DeleteAchievement_ShouldDeleteAchievement_WhenExists()
        {
            // Arrange
            var achievement = new Achievement { AchievementId = 1 };
            _achievementRepositoryMock.Setup(repo => repo.GetAchievementById(1)).ReturnsAsync(achievement);

            // Act
            Func<Task> act = async () => await _achievementService.DeleteAchievement(1);

            // Assert
            await act.Should().NotThrowAsync();
            _achievementRepositoryMock.Verify(repo => repo.DeleteAchievementById(1), Times.Once);
        }

        [Test]
        public async Task DeleteAchievement_ShouldThrowArgumentException_WhenAchievementDoesNotExist()
        {
            // Arrange
            _achievementRepositoryMock.Setup(repo => repo.GetAchievementById(1)).ReturnsAsync((Achievement)null);

            // Act
            Func<Task> act = async () => await _achievementService.DeleteAchievement(1);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Achievement doesn't exist");
        }
}