using FutSpect.Dal.Entities.Clubs;
using FutSpect.Shared.Models.Clubs;

namespace FutSpect.Dal.Repositories.Clubs;

public class ClubRepository : IClubRepository
{
    private readonly FutSpectContext _context;

    public ClubRepository(FutSpectContext context)
    {
        _context = context;
    }

    public async Task<List<(int Id, string Name)>> Add(ICollection<ClubInfo> clubs)
    {
        var entities = clubs
            .Select(x => new ClubEntity
            {
                LeagueId = x.LeagueId,
                Name = x.Name,
                RosterUrl = x.RosterUrl,
                ScheduleUrl = x.ScheduleUrl
            });

        await _context.Clubs.AddRangeAsync(entities);
        await _context.SaveChangesAsync();

        return [.. entities.Select(x => (x.Id, x.Name))];
    }

    public async Task AddImages(ICollection<ClubLogo> logos)
    {
        var entities = logos
            .Select(x => new ClubLogoEntity
            {
                ClubId = x.ClubId,
                Bytes = x.ImageBytes,
                SrcUrl = x.ImageSrc,
                Extension = x.FileExtension
            });

        await _context.ClubLogos.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }
}