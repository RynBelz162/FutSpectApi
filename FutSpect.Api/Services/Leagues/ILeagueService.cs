using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Api.Services.Leagues;

public interface ILeagueService
{
    Task<IEnumerable<League>> Get();
}
