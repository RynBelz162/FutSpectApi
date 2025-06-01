using FutSpect.DAL.Interfaces;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.DAL.Repositories.Leagues;

public interface ILeagueRepository
{
    Task<int> GetId(string name, int countryId);

    Task<int> Add(League league);
    
    Task AddImage(LeagueLogo logo);

    Task<IEnumerable<League>> Get(IPageable pageable);
}