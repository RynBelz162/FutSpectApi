using FutSpect.Shared.Models.Leagues;

namespace FutSpect.DAL.Repositories.Leagues;

public interface ILeagueRepository
{
    Task<int> Get(string name, int countryId);

    Task<int?> Find(string name, int countryId);

    Task<int> Save(League league);
}