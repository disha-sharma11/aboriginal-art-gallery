using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Api.DTOs.Comments;

public class CommentUpdateRequestDto
{
    [Required]
    [MaxLength(100)]
    public string VisitorName { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? VisitorEmail { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    public int? Rating { get; set; }

    [Required]
    public int ArtifactId { get; set; }
}
