using FutSpect.Scraper.Models;

namespace FutSpect.Scraper.Services.Clubs;

public interface IClubService
{
    Task Add(ICollection<ClubScrapeInfo> clubInfos);
}