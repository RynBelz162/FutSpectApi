using FutSpect.Shared.Models.Leagues;

namespace FutSpect.DAL.Repositories.Leagues;

public interface ILeagueRepository
{
    Task<League?> Get(string name);

    Task<int> Save(League league);
}