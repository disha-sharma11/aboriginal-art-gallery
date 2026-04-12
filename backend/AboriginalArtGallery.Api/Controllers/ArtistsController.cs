using AboriginalArtGallery.Api.DTOs.Artists;
using AboriginalArtGallery.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AboriginalArtGallery.Api.Controllers;

/// <summary>
/// Manages artist records and artist-related artifact queries.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ArtistsController : ControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistsController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    /// <summary>
    /// Gets all artists.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllArtists()
    {
        var artists = await _artistService.GetAllArtistsAsync();

        return Ok(artists);
    }

    /// <summary>
    /// Gets one artist by id.
    /// </summary>
    /// <param name="id">The id of the artist.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetArtistById(int id)
    {
        var artist = await _artistService.GetArtistByIdAsync(id);

        if (artist == null)
        {
            return NotFound("Artist not found");
        }

        return Ok(artist);
    }

    /// <summary>
    /// Creates a new artist.
    /// </summary>
    /// <param name="dto">The artist data from the request body.</param>
    [HttpPost]
    public async Task<IActionResult> CreateArtist([FromBody] ArtistCreateRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var artist = await _artistService.CreateArtistAsync(dto);

        if (artist == null)
        {
            return BadRequest("Selected tribe does not exist");
        }

        return Ok(artist);
    }

    /// <summary>
    /// Updates an existing artist.
    /// </summary>
    /// <param name="id">The id of the artist to update.</param>
    /// <param name="dto">The updated artist data.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArtist(int id, [FromBody] ArtistUpdateRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var artist = await _artistService.UpdateArtistAsync(id, dto);

        if (artist == null)
        {
            return NotFound("Artist not found or selected tribe does not exist");
        }

        return Ok(artist);
    }

    /// <summary>
    /// Soft deletes an artist by id.
    /// </summary>
    /// <param name="id">The id of the artist to soft delete.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtist(int id)
    {
        var deleted = await _artistService.SoftDeleteArtistAsync(id);

        if (!deleted)
        {
            return NotFound("Artist not found");
        }

        return Ok("Artist deleted successfully");
    }

    /// <summary>
    /// Gets all artifacts created by a specific artist.
    /// </summary>
    /// <param name="id">The id of the artist.</param>
    [HttpGet("{id}/artifacts")]
    public async Task<IActionResult> GetArtifactsByArtistId(int id)
    {
        var artifacts = await _artistService.GetArtifactsByArtistIdAsync(id);

        if (artifacts == null)
        {
            return NotFound("Artist not found");
        }

        return Ok(artifacts);
    }
}
