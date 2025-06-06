using FutSpect.Dal.Repositories.Images;
using FutSpect.Shared.Models.Clubs;
using FutSpect.Shared.Models.Leagues;

namespace FutSpect.Api.Services.Images;

public class ImageService : IImageService
{
    private readonly IImageRepository _imageRepository;

    public ImageService(IImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public async Task<LeagueLogo?> GetLeagueLogo(Guid id)
    {
        return await _imageRepository.GetLeagueLogo(id);
    }

    public async Task<ClubLogo?> GetClubLogo(Guid id)
    {
        return await _imageRepository.GetClubLogo(id);
    }
}
