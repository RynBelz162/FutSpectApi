using FutSpect.Scraper.Services.Scraping;
using FutSpect.Shared.Constants;
using Shouldly;

namespace FutSpect.Scraper.Tests.Services.Scraping;

public class PlayerInfoParseServiceTests
{
    private readonly PlayerInfoParseService _playerInfoParseService;

    public PlayerInfoParseServiceTests()
    {
        _playerInfoParseService = new();
    }

    [Theory]
    [InlineData(null, 0)]
    [InlineData("#7 - Test Striker Man", 7)]
    [InlineData("Test Striker Man - #8", 8)]
    [InlineData("#11", 11)]
    [InlineData("17", 17)]
    [InlineData("####99", 99)]
    [InlineData("Senior Citymanhjk1", 1)]
    public void GetNumber_WhenGivenString_ShouldParseOutNumber(string? value, short expected)
    {
        var number = _playerInfoParseService.GetNumber(value);
        number.ShouldBe(expected);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("tor")]
    [InlineData("shortstop")]
    public void GetPositionId_WhenInvalid_DefaultsToGoalKeeper(string? position)
    {
        var positionId = _playerInfoParseService.GetPositionId(position);
        positionId.ShouldBe(Positions.Goalkeeper);
    }

    [Theory]
    [InlineData("defender")]
    [InlineData("Defender")]
    [InlineData("deFenDER")]
    [InlineData("DEFENDER")]
    public void GetPositionId_WhenDefender_ReturnsDefenderId(string position)
    {
        var positionId = _playerInfoParseService.GetPositionId(position);
        positionId.ShouldBe(Positions.Defender);
    }

    [Theory]
    [InlineData("Midfielder")]
    [InlineData("MIDFIELDER")]
    [InlineData("midfielder")]
    [InlineData("midFielDER")]
    public void GetPositionId_WhenMidfielder_ReturnsMidfieldId(string position)
    {
        var positionId = _playerInfoParseService.GetPositionId(position);
        positionId.ShouldBe(Positions.Midfielder);
    }

    [Theory]
    [InlineData("Forward")]
    [InlineData("FORWARD")]
    [InlineData("forward")]
    [InlineData("foRWARD")]
    public void GetPositionId_WhenForward_ReturnsForwardId(string position)
    {
        var positionId = _playerInfoParseService.GetPositionId(position);
        positionId.ShouldBe(Positions.Forward);
    }

    [Theory]
    [InlineData("Goalkeeper")]
    [InlineData("GOALKEEPER")]
    [InlineData("goalkeeper")]
    [InlineData("GOALkeeper")]
    public void GetPositionId_WhenGoalkeeper_ReturnsGoalkeeperId(string position)
    {
        var positionId = _playerInfoParseService.GetPositionId(position);
        positionId.ShouldBe(Positions.Goalkeeper);
    }

    [Theory]
    [InlineData(null, null, "")]
    [InlineData("   ", null, "")]
    [InlineData("John Smith", "John", "Smith")]
    [InlineData("Jorginho", null, "Jorginho")]
    [InlineData("Lionel Messi", "Lionel", "Messi")]
    public void GetName_WhenValidName_ReturnsFirstAndLast(string? value, string? expectedFirstName, string expectedLastName)
    {
        var (firstName, lastName) = _playerInfoParseService.GetName(value);
        firstName.ShouldBe(expectedFirstName);
        lastName.ShouldBe(expectedLastName);
    }
}