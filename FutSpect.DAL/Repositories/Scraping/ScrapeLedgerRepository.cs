using FutSpect.DAL.Entities.Scraping;
using FutSpect.Shared.Models.Scraping;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.DAL.Repositories.Scraping;

public class ScrapeLedgerRepository : IScrapeLedgerRepository
{
    private readonly FutSpectContext _context;

    public ScrapeLedgerRepository(FutSpectContext context)
    {
        _context = context;
    }

    public async Task Add(ScrapeLedger scrapeLedger)
    {
        var entity = new ScrapeLedgerEntity
        {
            LeagueId = scrapeLedger.LeagueId,
            TypeId = scrapeLedger.TypeId,
            CreatedDate = DateTime.UtcNow
        };

        await _context.ScrapeLedgers.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> Any(string name, int countryId, int typeId, DateTime createdDate)
    {
        return await _context.ScrapeLedgers
            .AnyAsync(sl =>
                sl.League.Name == name
                && sl.League.CountryId == countryId
                && sl.TypeId == typeId
                && sl.CreatedDate.Date >= createdDate.Date
            );
    }
}