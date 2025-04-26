using FutSpect.DAL.Entities.Leagues;
using FutSpect.Shared.Models.Leagues;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Repositories.Leagues;

public class LeagueRepository : ILeagueRepository
{
    private readonly FutSpectContext futSpectContext;

    public LeagueRepository(FutSpectContext futSpectContext)
    {
        this.futSpectContext = futSpectContext;
    }

    public async Task<int> Get(string name, int countryId)
    {
        return await futSpectContext.Leagues
            .Where(x => x.Name == name && x.CountryId == countryId)
            .Select(x => x.Id)
            .SingleAsync();
    }


    public async Task<int?> Find(string name, int countryId)
    {
        return await futSpectContext.Leagues
            .Where(x => x.Name == name && x.CountryId == countryId)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> Save(League league)
    {
        var entity = new LeagueEntity
        {
            Name = league.Name,
            Abbreviation = league.Abbreviation,
            CountryId = league.CountryId,
            PyramidLevel = league.PyramidLevel
        };

        await futSpectContext.AddAsync(entity);
        await futSpectContext.SaveChangesAsync();

        return entity.Id;
    }
}