using Backend.Dtos;
using Backend.Entities;
using Backend.Repositories.Interfaces;
using Backend.Services;
using FluentAssertions;
using Moq;

namespace TestProject1.ServicesTests;

[TestFixture]
public class AchievementsServiceTests
{
    private AchievementService _service;
    private Mock<IAchievementRepository> _repositoryMock;

    [SetUp]
    public void Setup()
    {
        _repositoryMock = new Mock<IAchievementRepository>();
        _service = new AchievementService(_repositoryMock.Object);
    }

    [Test]
    public void Retrieve_ShouldReturnAllAchievements_WhenAnyIsPresent()
    {
        //Arrange
        var expectedResult = GetListOfAchievements();
        _repositoryMock.Setup(rm => rm.GetAllAchievements()).ReturnsAsync(expectedResult);
        
        //Act
        var result = _service.Retrieve().GetAwaiter().GetResult();

        //Assert
        result.Should().BeEquivalentTo(expectedResult.Select(ach => ach.ConvertToDto()).ToList());
    }
    
    [Test]
    public void Retrieve_ShouldReturnEmptyCollection_WhenNoneIsPresent()
    {
        //Arrange
        var expectedResult = new List<Achievement>();
        _repositoryMock.Setup(rm => rm.GetAllAchievements()).ReturnsAsync(expectedResult);
        
        //Act
        var result = _service.Retrieve().GetAwaiter().GetResult();

        //Assert
        result.Should().BeEmpty();
    }
    
    [Test]
    public void RetrieveById_ShouldThrowException_WhenNoneIsPresent()
    {
        //Arrange
        Achievement? expectedResult = null;
        _repositoryMock.Setup(rm => rm.GetAchievementById(It.IsAny<int>())).ReturnsAsync(expectedResult);
        
        //Act
        Action result = () => _service.Retrieve(It.IsAny<int>()).GetAwaiter().GetResult();

        //Assert
        result.Should().Throw<ArgumentException>().WithMessage("No achievement with provided id");
    }
    
    [Test]
    public void RetrieveByid_ShouldReturnObject_WhenIsPresent()
    {
        //Arrange
        var expectedResult = GetSingleAchievement();
        _repositoryMock.Setup(rm => rm.GetAchievementById(It.IsAny<int>())).ReturnsAsync(expectedResult);
        
        //Act
        var result = _service.Retrieve(It.IsAny<int>()).GetAwaiter().GetResult();

        //Assert
        result.Should().BeEquivalentTo(expectedResult.ConvertToDto());
    }

    [Test]
    public void AddAchievement_ShouldNotThrowException_WhenAchievementIsUnique()
    {
        //Arrange
        _repositoryMock.Setup(rm => rm.GetAchievementByName(It.IsAny<string>())).ReturnsAsync(() => null);
        _repositoryMock.Setup(rm => rm.GetAchievementByCriteria(It.IsAny<Dictionary<string, object>>())).ReturnsAsync(() => null);
        
        //Act
        Action result = () => _service.AddAchievement(It.IsAny<AchievementDto>()).GetAwaiter().GetResult();
        
        //Assert
        result.Should().NotThrow<ArgumentException>();
    }
    
    [TestCaseSource(nameof(AddAchievementTestCases))]
    public void AddAchievement_ShouldThrowException_WhenAchievementWithGivenNameOrCriteriaAlreadyExists(Achievement[] achievements)
    {
        //Arrange
        _repositoryMock.Setup(rm => rm.GetAchievementByName(It.IsAny<string>())).ReturnsAsync(achievements[0]);
        _repositoryMock.Setup(rm => rm.GetAchievementByCriteria(It.IsAny<Dictionary<string, object>>())).ReturnsAsync(achievements[1]);
        
        //Act
        Action result = () => _service.AddAchievement(GetSingleAchievement().ConvertToDto()).GetAwaiter().GetResult();
        
        //Assert
        result.Should().Throw<ArgumentException>();
    }
    
    [Test]
    public void EditAchievement_ShouldNotThrowException_WhenAchievementIsUnique()
    {
        var expectedResult = GetSingleAchievement();
        //Arrange
        _repositoryMock.Setup(rm => rm.GetAchievementByName(It.IsAny<string>())).ReturnsAsync(() => null);
        _repositoryMock.Setup(rm => rm.GetAchievementById(It.IsAny<int>())).ReturnsAsync(expectedResult);
        
        //Act
        Action result = () => _service.EditAchievement(1, GetSingleAchievement().ConvertToDto()).GetAwaiter().GetResult();
        
        //Assert
        result.Should().NotThrow<ArgumentException>();
    }
    
    [TestCaseSource(nameof(EditAchievementTestCases))]
    public void EditAchievement_ShouldThrowException_WhenAchievementWithGivenNameAlreadyExistsOrNotPresentWithGivenId(Achievement[] achievements)
    {
        //Arrange
        _repositoryMock.Setup(rm => rm.GetAchievementByName(It.IsAny<string>())).ReturnsAsync(achievements[0]);
        _repositoryMock.Setup(rm => rm.GetAchievementById(It.IsAny<int>())).ReturnsAsync(achievements[1]);
        
        //Act
        Action result = () => _service.EditAchievement(It.IsAny<int>(), GetSingleAchievement().ConvertToDto()).GetAwaiter().GetResult();
        
        //Assert
        result.Should().Throw<ArgumentException>();
    }

    [Test]
    public void DeleteAchievement_ShouldNotThrowException_WhenOneIsPresent()
    {
        var expectedResult = GetSingleAchievement();
        //Arrange
        _repositoryMock.Setup(rm => rm.GetAchievementById(It.IsAny<int>())).ReturnsAsync(expectedResult);
        
        //Act
        Action result = () => _service.DeleteAchievement(1).GetAwaiter().GetResult();
        
        //Assert
        result.Should().NotThrow<ArgumentException>();
    }
    
    [Test]
    public void DeleteAchievement_ShouldThrowException_WhenOneIsNotPresent()
    {
        //Arrange
        _repositoryMock.Setup(rm => rm.GetAchievementById(It.IsAny<int>())).ReturnsAsync(() => null);
        
        //Act
        Action result = () => _service.DeleteAchievement(It.IsAny<int>()).GetAwaiter().GetResult();
        
        //Assert
        result.Should().Throw<ArgumentException>();
    }

    private IEnumerable<Achievement> GetListOfAchievements()
    {
        return new List<Achievement>
        {
            new()
            {
                AchievementId = 1,
                AchievementName = "test1",
                AchievementDescription = "test1",
                AchievementCriteria = new Dictionary<string, object>
                {
                    ["test1"] = "test1",
                    ["test2"] = "test2"
                }
            },
            new()
            {
                AchievementId = 2,
                AchievementName = "test2",
                AchievementDescription = "test2",
                AchievementCriteria = new Dictionary<string, object>
                {
                    ["test1"] = "test1",
                    ["test2"] = "test2"
                }
            }
        };
    }

    private Achievement GetSingleAchievement()
    {
        return new()
        {
            AchievementId = 1,
            AchievementName = "test1",
            AchievementDescription = "test1",
            AchievementCriteria = new Dictionary<string, object>
            {
                ["test1"] = "test1",
                ["test2"] = "test2"
            }
        };
    }
    
    public static IEnumerable<Achievement[]> AddAchievementTestCases
    {
        get
        {
            yield return
            [
                new ()
                {
                    AchievementId = 1,
                    AchievementName = "test1",
                    AchievementDescription = "test1",
                    AchievementCriteria = new Dictionary<string, object>
                    {
                        ["test1"] = "test1",
                        ["test2"] = "test2"
                    }
                },
                null
            ];

            yield return
            [
                null,
                new ()
                {
                    AchievementId = 2,
                    AchievementName = "test2",
                    AchievementDescription = "test2",
                    AchievementCriteria = new Dictionary<string, object>
                    {
                        ["criteria1"] = "value1",
                        ["criteria2"] = "value2"
                    }
                }
            ];
        }
    }
    
    public static IEnumerable<Achievement[]> EditAchievementTestCases
    {
        get
        {
            yield return
            [
                new ()
                {
                    AchievementId = 1,
                    AchievementName = "test1",
                    AchievementDescription = "test1",
                    AchievementCriteria = new Dictionary<string, object>
                    {
                        ["test1"] = "test1",
                        ["test2"] = "test2"
                    }
                },
                null
            ];

            yield return
            [
                new ()
                {
                    AchievementId = 1,
                    AchievementName = "test1",
                    AchievementDescription = "test1",
                    AchievementCriteria = new Dictionary<string, object>
                    {
                        ["test1"] = "test1",
                        ["test2"] = "test2"
                    }
                },
                new ()
                {
                    AchievementId = 2,
                    AchievementName = "test2",
                    AchievementDescription = "test2",
                    AchievementCriteria = new Dictionary<string, object>
                    {
                        ["criteria1"] = "value1",
                        ["criteria2"] = "value2"
                    }
                }
            ];
        }
        
        
    }
}