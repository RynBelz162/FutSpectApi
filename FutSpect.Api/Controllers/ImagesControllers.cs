using Microsoft.AspNetCore.Mvc;
using FutSpect.Api.Services.Images;

namespace FutSpect.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImagesController(IImageService imageService)
    {
        _imageService = imageService;
    }

    [Tags("Images")]
    [HttpGet("league/{id}")]
    [EndpointSummary("Get League Logo")]
    [EndpointDescription("Returns the logo image for the specified league.")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetLeagueLogo(Guid id)
    {
        var logo = await _imageService.GetLeagueLogo(id);
        if (logo is null)
        {
            return NotFound();
        }

        return File(logo.ImageBytes, $"image/{logo.FileExtension[1..]}");
    }

    [Tags("Images")]
    [HttpGet("club/{id}")]
    [EndpointSummary("Get Club Logo")]
    [EndpointDescription("Returns the logo image for the specified club.")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClubLogo(Guid id)
    {
        var logo = await _imageService.GetClubLogo(id);
        if (logo is null)
        {
            return NotFound();
        }

        return File(logo.ImageBytes, $"image/{logo.FileExtension[1..]}");
    }
}