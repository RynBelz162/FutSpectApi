using FutSpect.Dal.Entities.Clubs;
using FutSpect.Shared.Models.Clubs;
using FutSpect.Shared.Models.Leagues;
using Microsoft.EntityFrameworkCore;

namespace FutSpect.Dal.Repositories.Images;

public class ImageRepository : IImageRepository
{
    private readonly FutSpectContext _context;

    public ImageRepository(FutSpectContext context)
    {
        _context = context;
    }

    public async Task<LeagueLogo?> GetLeagueLogo(Guid id)
    {
        var entity = await _context.LeagueLogos
            .Where(x => x.Id == id)
            .Select(x => new LeagueLogo
            {
                LeagueId = x.LeagueId,
                ImageBytes = x.Bytes,
                ImageSrc = x.SrcUrl ?? string.Empty,
                FileExtension = x.Extension
            })
            .SingleOrDefaultAsync();

        return entity;
    }

    public async Task<ClubLogo?> GetClubLogo(Guid id)
    {
        var entity = await _context.ClubLogos
            .Where(x => x.Id == id)
            .Select(x => new ClubLogo
            {
                ClubId = x.ClubId,
                ImageBytes = x.Bytes,
                ImageSrc = x.SrcUrl ?? string.Empty,
                FileExtension = x.Extension
            })
            .SingleOrDefaultAsync();

        return entity;
    }

    public async Task AddLeagueLogo(LeagueLogo logo)
    {
        var entity = new LeagueLogoEntity
        {
            LeagueId = logo.LeagueId,
            Bytes = logo.ImageBytes,
            SrcUrl = logo.ImageSrc,
            Extension = logo.FileExtension,
            CreatedOn = DateTime.UtcNow,
            ModifiedOn = DateTime.UtcNow
        };

        await _context.LeagueLogos.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLeagueLogo(LeagueLogo logo)
    {
        await _context.LeagueLogos
            .Where(x => x.LeagueId == logo.LeagueId)
            .ExecuteUpdateAsync(setters =>
                setters
                    .SetProperty(p => p.Bytes, logo.ImageBytes)
                    .SetProperty(p => p.SrcUrl, logo.ImageSrc)
                    .SetProperty(p => p.Extension, logo.FileExtension)
                    .SetProperty(p => p.ModifiedOn, DateTime.UtcNow)
            );
    }
}
