using AboriginalArtGallery.Api.DTOs.Comments;

namespace AboriginalArtGallery.Api.Interfaces;

public interface ICommentService
{
    Task<List<CommentResponseDto>> GetAllCommentsAsync();

    Task<CommentResponseDto?> GetCommentByIdAsync(int id);

    Task<List<CommentResponseDto>?> GetCommentsByArtifactIdAsync(int artifactId);

    Task<CommentResponseDto?> CreateCommentAsync(CommentCreateRequestDto dto);

    Task<CommentResponseDto?> UpdateCommentAsync(int id, CommentUpdateRequestDto dto);

    Task<bool> SoftDeleteCommentAsync(int id);
}
