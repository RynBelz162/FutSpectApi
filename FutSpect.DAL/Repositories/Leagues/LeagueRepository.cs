using FutSpect.DAL.Entities.Clubs;
using FutSpect.DAL.Entities.Leagues;
using FutSpect.Shared.Models.Leagues;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Repositories.Leagues;

public class LeagueRepository : ILeagueRepository
{
    private readonly FutSpectContext _futSpectContext;

    public LeagueRepository(FutSpectContext futSpectContext)
    {
        _futSpectContext = futSpectContext;
    }

    public async Task<int> Get(string name, int countryId)
    {
        return await _futSpectContext.Leagues
            .Where(x => x.Name == name && x.CountryId == countryId)
            .Select(x => x.Id)
            .SingleAsync();
    }


    public async Task<int?> Find(string name, int countryId)
    {
        return await _futSpectContext.Leagues
            .Where(x => x.Name == name && x.CountryId == countryId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> Add(League league)
    {
        var entity = new LeagueEntity
        {
            Name = league.Name,
            Abbreviation = league.Abbreviation,
            CountryId = league.CountryId,
            PyramidLevel = league.PyramidLevel,
            HasProRel = league.HasProRel,
            Website = league.Website,
            CreatedDate = DateTime.UtcNow
        };

        await _futSpectContext.Leagues.AddAsync(entity);
        await _futSpectContext.SaveChangesAsync();

        return entity.Id;
    }

    public async Task AddImage(LeagueLogo logo)
    {
        var entity = new LeagueLogoEntity
        {
            LeagueId = logo.LeagueId,
            Bytes = logo.ImageBytes,
            SrcUrl = logo.ImageSrc,
            Extension = logo.FileExtension,
            CreatedDate = DateTime.UtcNow
        };

        await _futSpectContext.LeagueLogos.AddAsync(entity);
        await _futSpectContext.SaveChangesAsync();
    }
}