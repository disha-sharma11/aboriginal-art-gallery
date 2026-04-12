using AboriginalArtGallery.Api.DTOs.Tribes;
using AboriginalArtGallery.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AboriginalArtGallery.Api.Controllers;

/// <summary>
/// Manages Aboriginal tribe records and their related data.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TribesController : ControllerBase
{
    private readonly ITribeService _tribeService;

    public TribesController(ITribeService tribeService)
    {
        _tribeService = tribeService;
    }

    /// <summary>
    /// Gets all Aboriginal tribes.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllTribes()
    {
        var tribes = await _tribeService.GetAllTribesAsync();

        return Ok(tribes);
    }

    /// <summary>
    /// Gets one Aboriginal tribe by its id.
    /// </summary>
    /// <param name="id">The id of the tribe.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTribeById(int id)
    {
        var tribe = await _tribeService.GetTribeByIdAsync(id);

        if (tribe == null)
        {
            return NotFound("Tribe not found");
        }

        return Ok(tribe);
    }

    /// <summary>
    /// Creates a new Aboriginal tribe.
    /// </summary>
    /// <param name="dto">The tribe data from the request body.</param>
    [HttpPost]
    public async Task<IActionResult> CreateTribe([FromBody] TribeCreateRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var tribe = await _tribeService.CreateTribeAsync(dto);

        return Ok(tribe);
    }

    /// <summary>
    /// Updates an existing Aboriginal tribe.
    /// </summary>
    /// <param name="id">The id of the tribe to update.</param>
    /// <param name="dto">The updated tribe data.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTribe(int id, [FromBody] TribeUpdateRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var tribe = await _tribeService.UpdateTribeAsync(id, dto);

        if (tribe == null)
        {
            return NotFound("Tribe not found");
        }

        return Ok(tribe);
    }

    /// <summary>
    /// Soft deletes an Aboriginal tribe by id.
    /// </summary>
    /// <param name="id">The id of the tribe to soft delete.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTribe(int id)
    {
        var deleted = await _tribeService.SoftDeleteTribeAsync(id);

        if (!deleted)
        {
            return NotFound("Tribe not found");
        }

        return Ok("Tribe deleted successfully");
    }

    /// <summary>
    /// Gets all artists that belong to a specific tribe.
    /// </summary>
    /// <param name="id">The id of the tribe.</param>
    [HttpGet("{id}/artists")]
    public async Task<IActionResult> GetArtistsByTribeId(int id)
    {
        var artists = await _tribeService.GetArtistsByTribeIdAsync(id);

        if (artists == null)
        {
            return NotFound("Tribe not found");
        }

        return Ok(artists);
    }

    /// <summary>
    /// Gets all artifacts linked to a specific tribe.
    /// </summary>
    /// <param name="id">The id of the tribe.</param>
    [HttpGet("{id}/artifacts")]
    public async Task<IActionResult> GetArtifactsByTribeId(int id)
    {
        var artifacts = await _tribeService.GetArtifactsByTribeIdAsync(id);

        if (artifacts == null)
        {
            return NotFound("Tribe not found");
        }

        return Ok(artifacts);
    }
}
