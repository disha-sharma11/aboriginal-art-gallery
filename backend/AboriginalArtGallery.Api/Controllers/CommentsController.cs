using AboriginalArtGallery.Api.DTOs.Comments;
using AboriginalArtGallery.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AboriginalArtGallery.Api.Controllers;

/// <summary>
/// Manages visitor comments linked to artifacts.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CommentsController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentsController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    /// <summary>
    /// Gets all comments.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAllComments()
    {
        var comments = await _commentService.GetAllCommentsAsync();

        return Ok(comments);
    }

    /// <summary>
    /// Gets one comment by id.
    /// </summary>
    /// <param name="id">The id of the comment.</param>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCommentById(int id)
    {
        var comment = await _commentService.GetCommentByIdAsync(id);

        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        return Ok(comment);
    }

    /// <summary>
    /// Gets all comments for a specific artifact.
    /// </summary>
    /// <param name="artifactId">The id of the artifact.</param>
    [HttpGet("artifact/{artifactId}")]
    public async Task<IActionResult> GetCommentsByArtifactId(int artifactId)
    {
        var comments = await _commentService.GetCommentsByArtifactIdAsync(artifactId);

        if (comments == null)
        {
            return NotFound("Artifact not found");
        }

        return Ok(comments);
    }

    /// <summary>
    /// Creates a new comment for an artifact.
    /// </summary>
    /// <param name="dto">The comment data from the request body.</param>
    [HttpPost]
    public async Task<IActionResult> CreateComment([FromBody] CommentCreateRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var comment = await _commentService.CreateCommentAsync(dto);

        if (comment == null)
        {
            return BadRequest("Selected artifact does not exist");
        }

        return Ok(comment);
    }

    /// <summary>
    /// Updates an existing comment.
    /// </summary>
    /// <param name="id">The id of the comment to update.</param>
    /// <param name="dto">The updated comment data.</param>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentUpdateRequestDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var comment = await _commentService.UpdateCommentAsync(id, dto);

        if (comment == null)
        {
            return NotFound("Comment not found or selected artifact does not exist");
        }

        return Ok(comment);
    }

    /// <summary>
    /// Soft deletes a comment by id.
    /// </summary>
    /// <param name="id">The id of the comment to soft delete.</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteComment(int id)
    {
        var deleted = await _commentService.SoftDeleteCommentAsync(id);

        if (!deleted)
        {
            return NotFound("Comment not found");
        }

        return Ok("Comment deleted successfully");
    }
}
