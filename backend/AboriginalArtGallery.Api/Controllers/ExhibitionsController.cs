using AboriginalArtGallery.Api.DTOs.Exhibitions;
using AboriginalArtGallery.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AboriginalArtGallery.Api.Controllers;

/// <summary>
/// Manages exhibition records for the Aboriginal art gallery.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ExhibitionsController : ControllerBase
{
    private readonly IExhibitionService _exhibitionService;

    public ExhibitionsController(IExhibitionService exhibitionService)
    {
        _exhibitionService = exhibitionService;
    }

    /// <summary>
    /// Gets all exhibitions.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllExhibitions()
    {
        var exhibitions = await _exhibitionService.GetAllExhibitionsAsync();

        return Ok(exhibitions);
    }

    /// <summary>
    /// Gets one exhibition by id.
    /// </summary>
    /// <param name="id">The id of the exhibition.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetExhibitionById(int id)
    {
        var exhibition = await _exhibitionService.GetExhibitionByIdAsync(id);

        if (exhibition == null)
        {
            return NotFound("Exhibition not found");
        }

        return Ok(exhibition);
    }

    /// <summary>
    /// Creates a new exhibition.
    /// </summary>
    /// <param name="dto">The exhibition data from the request body.</param>
    [HttpPost]
    public async Task<IActionResult> CreateExhibition([FromBody] ExhibitionCreateRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var exhibition = await _exhibitionService.CreateExhibitionAsync(dto);

        return Ok(exhibition);
    }

    /// <summary>
    /// Updates an existing exhibition.
    /// </summary>
    /// <param name="id">The id of the exhibition to update.</param>
    /// <param name="dto">The updated exhibition data.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExhibition(
        int id,
        [FromBody] ExhibitionUpdateRequestDto dto
    )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var exhibition = await _exhibitionService.UpdateExhibitionAsync(id, dto);

        if (exhibition == null)
        {
            return NotFound("Exhibition not found");
        }

        return Ok(exhibition);
    }

    /// <summary>
    /// Soft deletes an exhibition by id.
    /// </summary>
    /// <param name="id">The id of the exhibition to soft delete.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExhibition(int id)
    {
        var deleted = await _exhibitionService.SoftDeleteExhibitionAsync(id);

        if (!deleted)
        {
            return NotFound("Exhibition not found");
        }

        return Ok("Exhibition deleted successfully");
    }
}
