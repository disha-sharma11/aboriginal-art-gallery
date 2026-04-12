using AboriginalArtGallery.Api.DTOs.Artifacts;
using AboriginalArtGallery.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AboriginalArtGallery.Api.Controllers;

/// <summary>
/// Manages artifact records for the Aboriginal art gallery.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ArtifactsController : ControllerBase
{
    private readonly IArtifactService _artifactService;

    public ArtifactsController(IArtifactService artifactService)
    {
        _artifactService = artifactService;
    }

    /// <summary>
    /// Gets all artifacts.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllArtifacts()
    {
        var artifacts = await _artifactService.GetAllArtifactsAsync();

        return Ok(artifacts);
    }

    /// <summary>
    /// Gets one artifact by id.
    /// </summary>
    /// <param name="id">The id of the artifact.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetArtifactById(int id)
    {
        var artifact = await _artifactService.GetArtifactByIdAsync(id);

        if (artifact == null)
        {
            return NotFound("Artifact not found");
        }

        return Ok(artifact);
    }

    /// <summary>
    /// Creates a new artifact.
    /// </summary>
    /// <param name="dto">The artifact data from the request body.</param>
    [HttpPost]
    public async Task<IActionResult> CreateArtifact([FromBody] ArtifactCreateRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var artifact = await _artifactService.CreateArtifactAsync(dto);

        if (artifact == null)
        {
            return BadRequest("Selected artist, tribe, or exhibition does not exist");
        }

        return Ok(artifact);
    }

    /// <summary>
    /// Updates an existing artifact.
    /// </summary>
    /// <param name="id">The id of the artifact to update.</param>
    /// <param name="dto">The updated artifact data.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArtifact(int id, [FromBody] ArtifactUpdateRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var artifact = await _artifactService.UpdateArtifactAsync(id, dto);

        if (artifact == null)
        {
            return NotFound("Artifact not found or selected artist, tribe, or exhibition does not exist");
        }

        return Ok(artifact);
    }

    /// <summary>
    /// Soft deletes an artifact by id.
    /// </summary>
    /// <param name="id">The id of the artifact to soft delete.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArtifact(int id)
    {
        var deleted = await _artifactService.SoftDeleteArtifactAsync(id);

        if (!deleted)
        {
            return NotFound("Artifact not found");
        }

        return Ok("Artifact deleted successfully");
    }

    /// <summary>
    /// Gets all artifacts created by a specific artist.
    /// </summary>
    /// <param name="artistId">The id of the artist.</param>
    [HttpGet("artist/{artistId}")]
    public async Task<IActionResult> GetArtifactsByArtistId(int artistId)
    {
        var artifacts = await _artifactService.GetArtifactsByArtistIdAsync(artistId);

        if (artifacts == null)
        {
            return NotFound("Artist not found");
        }

        return Ok(artifacts);
    }

    /// <summary>
    /// Gets all artifacts linked to a specific tribe.
    /// </summary>
    /// <param name="tribeId">The id of the tribe.</param>
    [HttpGet("tribe/{tribeId}")]
    public async Task<IActionResult> GetArtifactsByTribeId(int tribeId)
    {
        var artifacts = await _artifactService.GetArtifactsByTribeIdAsync(tribeId);

        if (artifacts == null)
        {
            return NotFound("Tribe not found");
        }

        return Ok(artifacts);
    }
}
