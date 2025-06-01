using FutSpect.Shared.Models.Clubs;

namespace FutSpect.Dal.Repositories.Clubs;

public interface IClubRepository
{
    Task<List<(int Id, string Name)>> Add(ICollection<ClubInfo> clubs);
    Task AddImages(ICollection<ClubLogo> logos);
}