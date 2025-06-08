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

    public async Task<int> SearchId(string name, int countryId)
    {
        return await _futSpectContext.Leagues
            .Where(x => x.Name == name && x.CountryId == countryId)
            .Select(x => x.Id)
            .SingleOrDefaultAsync();
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

    public async Task<IEnumerable<League>> Get(ISearchable searchable)
    {
        var query = from league in _futSpectContext.Leagues
                    join country in _futSpectContext.Countries on league.CountryId equals country.Id
                    join logo in _futSpectContext.LeagueLogos on league.Id equals logo.LeagueId into logos
                    from image in logos.DefaultIfEmpty()
                    where string.IsNullOrEmpty(searchable.SearchTerm)
                          || league.Name.Contains(searchable.SearchTerm)
                          || country.Name.Contains(searchable.SearchTerm)
                          || league.Abbreviation.Contains(searchable.SearchTerm)
                          || country.Abbreviation.Contains(searchable.SearchTerm)
                    orderby league.Name
                    select new League
                    {
                        Id = league.Id,
                        Name = league.Name,
                        Abbreviation = league.Abbreviation,
                        CountryName = country.Name,
                        HasProRel = league.HasProRel,
                        PyramidLevel = league.PyramidLevel,
                        Website = league.Website,
                        LogoId = image.Id,
                    };

        return await query
            .Skip(searchable.Skip)
            .Take(searchable.PageSize)
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