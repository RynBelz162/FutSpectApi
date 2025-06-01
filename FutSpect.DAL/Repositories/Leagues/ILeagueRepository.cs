using FutSpect.Dal.Interfaces;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Dal.Repositories.Leagues;

public interface ILeagueRepository
{
    Task<int> GetId(string name, int countryId);

    Task<int> Add(League league);
    
    Task AddImage(LeagueLogo logo);

    Task<IEnumerable<League>> Get(IPageable pageable);
}