using FutSpect.DAL.Repositories.Leagues;
using FutSpect.Scraper.Services.Leagues;
using FutSpect.Shared.Models.Leagues;
using Moq;
using Shouldly;

namespace FutSpect.Scraper.Tests.Services.Leagues;

public class LeagueServiceTests
{
    private readonly Mock<ILeagueRepository> _leagueRepositoryMock;
    private readonly LeagueService _leagueService;

    public LeagueServiceTests()
    {
        _leagueRepositoryMock = new Mock<ILeagueRepository>();
        _leagueService = new LeagueService(_leagueRepositoryMock.Object);
    }

    [Fact]
    public async Task GetOrSave_ShouldReturnExistingLeagueId_WhenLeagueExists()
    {
        const int existingLeagueId = 1;
        var league = new League
        {
            Id = existingLeagueId,
            Name = "Test",
            Abbreviation = "TST",
            HasProRel = false,
            PyramidLevel = 1,
            CountryId = 1
        };

        _leagueRepositoryMock
            .Setup(repo => repo.Find("Test", existingLeagueId))
            .ReturnsAsync(existingLeagueId);

        var result = await _leagueService.GetOrSave(league);

        result.ShouldBe(existingLeagueId);
        _leagueRepositoryMock.Verify(repo => repo.Save(It.IsAny<League>()), Times.Never);
    }

    [Fact]
    public async Task GetOrSave_ShouldSaveAndReturnNewLeagueId_WhenLeagueDoesNotExist()
    {
        const int existingLeagueId = 1;
        var league = new League
        {
            Id = existingLeagueId,
            Name = "Test",
            Abbreviation = "TST",
            HasProRel = false,
            PyramidLevel = 1,
            CountryId = 1
        };

        _leagueRepositoryMock
            .Setup(repo => repo.Find("Test", existingLeagueId))
            .ReturnsAsync((int?)null);

        _leagueRepositoryMock
            .Setup(repo => repo.Save(league))
            .ReturnsAsync(existingLeagueId);

        var result = await _leagueService.GetOrSave(league);

        result.ShouldBe(existingLeagueId);
    }
}