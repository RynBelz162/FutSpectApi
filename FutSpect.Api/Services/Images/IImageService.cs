using FutSpect.Shared.Models.Clubs;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Api.Services.Images;

public interface IImageService
{
    Task<LeagueLogo?> GetLeagueLogo(Guid id);
    Task<ClubLogo?> GetClubLogo(Guid id);
}
