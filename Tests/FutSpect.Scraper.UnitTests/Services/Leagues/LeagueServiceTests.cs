using FutSpect.Dal.Repositories.Images;
using FutSpect.Dal.Repositories.Leagues;
using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services.Leagues;
using FutSpect.Shared.Models.Leagues;
using Moq;

namespace FutSpect.Scraper.UnitTests.Services.Leagues;

public class LeagueServiceTests
{
    private readonly Mock<ILeagueRepository> _leagueRepositoryMock;
    private readonly Mock<IImageRepository> _imageRepositoryMock = new();
    private readonly LeagueService _leagueService;

    public LeagueServiceTests()
    {
        _leagueRepositoryMock = new Mock<ILeagueRepository>();
        _leagueService = new LeagueService(_leagueRepositoryMock.Object, _imageRepositoryMock.Object);
    }

    [Fact]
    public async Task Upsert_WhenGivenNewLeagueScrapeInfo_AddsNewLeagueInfoAndLogo()
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
            .Setup(x => x.SearchId(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync((int)default);

        _leagueRepositoryMock
            .Setup(x => x.Add(It.IsAny<League>()))
            .ReturnsAsync(1);

        await _leagueService.Upsert(leagueScrapeInfo);

        _leagueRepositoryMock.Verify(x => x.Add(It.IsAny<League>()), Times.Once);
        _imageRepositoryMock.Verify(x => x.AddLeagueLogo(It.IsAny<LeagueLogo>()), Times.Once);

        _leagueRepositoryMock.Verify(x => x.Update(It.IsAny<League>()), Times.Never);
        _imageRepositoryMock.Verify(x => x.UpdateLeagueLogo(It.IsAny<LeagueLogo>()), Times.Never);
    }

    [Fact]
    public async Task Upsert_WhenGivenExistingLeagueScrapeInfo_UpdateLeagueInfoAndLogo()
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
            .Setup(x => x.SearchId(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync(1);

        await _leagueService.Upsert(leagueScrapeInfo);

        _leagueRepositoryMock.Verify(x => x.Update(It.IsAny<League>()), Times.Once);
        _imageRepositoryMock.Verify(x => x.UpdateLeagueLogo(It.IsAny<LeagueLogo>()), Times.Once);

        _leagueRepositoryMock.Verify(x => x.Add(It.IsAny<League>()), Times.Never);
        _imageRepositoryMock.Verify(x => x.AddLeagueLogo(It.IsAny<LeagueLogo>()), Times.Never);
    }
}