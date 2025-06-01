using FutSpect.Dal.Repositories.Leagues;
using FutSpect.Scraper.Models;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Scraper.Services.Leagues;

public class LeagueService : ILeagueService
{
    private readonly ILeagueRepository _leagueRepository;

    public LeagueService(ILeagueRepository leagueRepository)
    {
        _leagueRepository = leagueRepository;
    }

    public Task<int> GetId(League league) =>
        _leagueRepository.GetId(league.Name, league.CountryId);

    public async Task Add(LeagueScrapeInfo leagueInfo)
    {
        var league = new League
        {
            Name = leagueInfo.Name,
            PyramidLevel = leagueInfo.PyramidLevel,
            Abbreviation = leagueInfo.Abbreviation,
            CountryId = leagueInfo.CountryId,
            HasProRel = leagueInfo.HasProRel,
            Website = leagueInfo.Website,
        };

        var id = await _leagueRepository.Add(league);

        var logo = new LeagueLogo
        {
            LeagueId = id,
            ImageBytes = leagueInfo.Image.ImageBytes,
            ImageSrc = leagueInfo.Image.ImageSrcUrl,
            FileExtension = leagueInfo.Image.ImageExtension,
        };

        await _leagueRepository.AddImage(logo);
    }
}