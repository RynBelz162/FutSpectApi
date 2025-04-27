using FutSpect.Shared.Models.Clubs;

namespace FutSpect.DAL.Repositories.Clubs;

public interface IClubRepository
{
    Task<List<(int Id, string Name)>> Add(ICollection<ClubInfo> clubs);
    Task AddImages(ICollection<ClubLogo> logos);
}