using FutSpect.Dal.Repositories.Leagues;
using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services.Leagues;
using FutSpect.Shared.Models.Leagues;
using Moq;

namespace FutSpect.Scraper.UnitTests.Services.Leagues;

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
    public async Task Add_WhenGivenLeagueScrapeInfo_SavesLeagueInfoAndLogo()
    {
        LeagueScrapeInfo leagueScrapeInfo =
            new()
            {
                CountryId = 1,
                Name = "Test Club 1",
                Website = "https://club.com",
                HasProRel = true,
                PyramidLevel = 1,
                Abbreviation = "TC1",
                Image = new ScrapedImage
                {
                    ImageBytes = [],
                    ImageSrcUrl = "https://club.com/image?=clubone",
                    ImageExtension = ".png",
                }
            };

        _leagueRepositoryMock
            .Setup(x => x.Add(It.IsAny<League>()))
            .ReturnsAsync(1);

        await _leagueService.Add(leagueScrapeInfo);

        _leagueRepositoryMock.Verify(x => x.Add(It.IsAny<League>()), Times.Once);
        _leagueRepositoryMock.Verify(x => x.AddImage(It.IsAny<LeagueLogo>()), Times.Once);
    }
}