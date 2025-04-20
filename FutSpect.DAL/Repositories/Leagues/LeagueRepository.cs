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


    public async Task<League?> Get(string name)
    {
        return await futSpectContext.Leagues
            .Where(x => x.Name == name)
            .Select(x => new League
            {
                Id = x.Id,
                Name = x.Name,
                CountryId = x.CountryId,
                CountryName = x.Country.Name
            })
            .FirstOrDefaultAsync();
    }

    public async Task<int> Save(League league)
    {
        var entity = new LeagueEntity
        {
            Name = league.Name,
            CountryId = league.CountryId,
        };

        await futSpectContext.AddAsync(entity);
        await futSpectContext.SaveChangesAsync();

        return entity.Id;
    }
}