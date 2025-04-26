using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Scraper.Services.Leagues;

public interface ILeagueService
{
    Task<int> GetOrSave(League league);
}