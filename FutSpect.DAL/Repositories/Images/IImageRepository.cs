using FutSpect.Shared.Models.Clubs;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Dal.Repositories.Images;

public interface IImageRepository
{
    Task<LeagueLogo?> GetLeagueLogo(Guid id);
    Task<ClubLogo?> GetClubLogo(Guid id);
    Task AddLeagueLogo(LeagueLogo logo);
    Task UpdateLeagueLogo(LeagueLogo logo);
}
