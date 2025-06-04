using FutSpect.Dal.Entities.Clubs;
using FutSpect.Dal.Entities.Leagues;
using FutSpect.Dal.Interfaces;
using FutSpect.Shared.Models.Leagues;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.Dal.Repositories.Leagues;

public class LeagueRepository : ILeagueRepository
{
    private readonly FutSpectContext _futSpectContext;

    public LeagueRepository(FutSpectContext futSpectContext)
    {
        _futSpectContext = futSpectContext;
    }

    public async Task<int> GetId(string name, int countryId)
    {
        return await _futSpectContext.Leagues
            .Where(x => x.Name == name && x.CountryId == countryId)
            .Select(x => x.Id)
            .SingleAsync();
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
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
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
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };

        await _futSpectContext.LeagueLogos.AddAsync(entity);
        await _futSpectContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<League>> Get(IPageable pageable)
    {
        return await _futSpectContext.Leagues
            .Select(l => new League
            {
                Id = l.Id,
                Name = l.Name,
                Abbreviation = l.Abbreviation,
                CountryId = l.CountryId,
                HasProRel = l.HasProRel,
                PyramidLevel = l.PyramidLevel,
                Website = l.Website
            })
            .OrderBy(l => l.Name)
            .Skip(pageable.Skip)
            .Take(pageable.PageSize)
            .ToListAsync();
    }

    public async Task Update(League league)
    {
        await _futSpectContext.Leagues
            .Where(x => x.Id == league.Id)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(p => p.Name, league.Name)
                    .SetProperty(p => p.HasProRel, league.HasProRel)
                    .SetProperty(p => p.PyramidLevel, league.PyramidLevel)
                    .SetProperty(p => p.Abbreviation, league.Abbreviation)
                    .SetProperty(p => p.CountryId, league.CountryId)
                    .SetProperty(p => p.ModifiedOn, DateTime.UtcNow)
            );
    }
}