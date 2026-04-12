namespace AboriginalArtGallery.Api.DTOs.Comments;

public class CommentResponseDto
{
    public int Id { get; set; }

    public string VisitorName { get; set; } = string.Empty;

    public string? VisitorEmail { get; set; }

    public string Content { get; set; } = string.Empty;

    public int? Rating { get; set; }

    public int ArtifactId { get; set; }

    public string ArtifactTitle { get; set; } = string.Empty;
}
