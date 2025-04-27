using FutSpect.DAL.Repositories.Clubs;
using FutSpect.Scraper.Models;
using FutSpect.Shared.Models.Clubs;

namespace FutSpect.Scraper.Services.Clubs;

public class ClubService : IClubService
{
    private readonly IClubRepository _clubRepository;

    public ClubService(IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
    }

    public async Task Add(ICollection<ClubScrapeInfo> clubInfos)
    {
        var clubs = clubInfos
            .Select(x => new ClubInfo
            {
                Name = x.Name,
                LeagueId = x.LeagueId,
                RosterUrl = x.RosterUrl,
                ScheduleUrl = x.ScheduleUrl
            })
            .ToArray();

        var results = await _clubRepository.Add(clubs);

        var logos = clubInfos
            .Select(club =>
            {
                var (Id, _) = results.Single(result => result.Name == club.Name);

                return new ClubLogo
                {
                    ClubId = Id,
                    ImageBytes = club.Image.ImageBytes,
                    ImageSrc = club.Image.ImageSrcUrl,
                    FileExtension = club.Image.ImageExtension,
                };
            })
            .ToArray();

        await _clubRepository.AddImages(logos);
    }
}