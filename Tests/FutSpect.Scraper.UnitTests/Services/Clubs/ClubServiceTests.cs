using FutSpect.DAL.Repositories.Clubs;
using FutSpect.Scraper.Models;
using FutSpect.Scraper.Services.Clubs;
using FutSpect.Shared.Models.Clubs;
using Moq;

namespace FutSpect.Scraper.UnitTests.Services.Clubs;

public class ClubServiceTests
{
    private readonly ClubService _clubService;
    private readonly Mock<IClubRepository> _mockClubRepo = new();

    public ClubServiceTests()
    {
        _clubService = new(_mockClubRepo.Object);
    }

    [Fact]
    public async Task Add_WhenGivenClubScrapeInfo_SavesClubInfoAndLogo()
    {
        List<ClubScrapeInfo> clubScrapeInfo =
        [
            new()
            {
                LeagueId = 1,
                Name = "Test Club 1",
                ScheduleUrl = "https://club.com/schedule",
                RosterUrl = "https://club.com/roster",
                Image = new ScrapedImage
                {
                    ImageBytes = [],
                    ImageSrcUrl = "https://club.com/image?=clubone",
                    ImageExtension = ".png",
                }
            },
            new()
            {
                LeagueId = 1,
                Name = "Test Club 2",
                ScheduleUrl = "https://clubtwo.com/schedule",
                RosterUrl = "https://clubtwo.com/roster",
                Image = new ScrapedImage
                {
                    ImageBytes = [],
                    ImageSrcUrl = "https://club.com/image?=clubtwo",
                    ImageExtension = ".jpeg",
                }
            }
        ];

        _mockClubRepo
            .Setup(x => x.Add(It.IsAny<ICollection<ClubInfo>>()))
            .ReturnsAsync(
                [
                    (1, "Test Club 1"),
                    (2, "Test Club 2")
                ]
            );

        await _clubService.Add(clubScrapeInfo);

        _mockClubRepo.Verify(x => x.Add(It.IsAny<ICollection<ClubInfo>>()), Times.Once);
        _mockClubRepo.Verify(x => x.AddImages(It.IsAny<ICollection<ClubLogo>>()), Times.Once);
    }
}