using FutSpect.Dal.Interfaces;
using FutSpect.Shared.Models;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Api.Services.Leagues;

public interface ILeagueService
{
    Task<Paged<League>> Get(IPageable pageable);
}
