using AboriginalArtGallery.Api.Data;
using AboriginalArtGallery.Api.DTOs.Comments;
using AboriginalArtGallery.Api.Interfaces;
using AboriginalArtGallery.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AboriginalArtGallery.Api.Services;

public class CommentService : ICommentService
{
    private readonly AppDbContext _context;

    public CommentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CommentResponseDto>> GetAllCommentsAsync()
    {
        var comments = await _context
            .Comments.Include(c => c.Artifact)
            .ToListAsync();

        return comments.Select(c => MapToResponseDto(c)).ToList();
    }

    public async Task<CommentResponseDto?> GetCommentByIdAsync(int id)
    {
        var comment = await _context
            .Comments.Include(c => c.Artifact)
            .FirstOrDefaultAsync(c => c.Id == id);

        return comment == null ? null : MapToResponseDto(comment);
    }

    public async Task<List<CommentResponseDto>?> GetCommentsByArtifactIdAsync(int artifactId)
    {
        var artifactExists = await _context.Artifacts.AnyAsync(a => a.Id == artifactId);

        if (!artifactExists)
        {
            return null;
        }

        var comments = await _context
            .Comments.Include(c => c.Artifact)
            .Where(c => c.ArtifactId == artifactId)
            .ToListAsync();

        return comments.Select(c => MapToResponseDto(c)).ToList();
    }

    public async Task<CommentResponseDto?> CreateCommentAsync(CommentCreateRequestDto dto)
    {
        var artifactExists = await _context.Artifacts.AnyAsync(a => a.Id == dto.ArtifactId);

        if (!artifactExists)
        {
            return null;
        }

        var comment = new Comment
        {
            VisitorName = dto.VisitorName,
            VisitorEmail = dto.VisitorEmail,
            Content = dto.Content,
            Rating = dto.Rating,
            ArtifactId = dto.ArtifactId,
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        var createdComment = await _context
            .Comments.Include(c => c.Artifact)
            .FirstOrDefaultAsync(c => c.Id == comment.Id);

        return MapToResponseDto(createdComment!);
    }

    public async Task<CommentResponseDto?> UpdateCommentAsync(int id, CommentUpdateRequestDto dto)
    {
        var comment = await _context.Comments.FindAsync(id);

        if (comment == null)
        {
            return null;
        }

        var artifactExists = await _context.Artifacts.AnyAsync(a => a.Id == dto.ArtifactId);

        if (!artifactExists)
        {
            return null;
        }

        comment.VisitorName = dto.VisitorName;
        comment.VisitorEmail = dto.VisitorEmail;
        comment.Content = dto.Content;
        comment.Rating = dto.Rating;
        comment.ArtifactId = dto.ArtifactId;
        comment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var updatedComment = await _context
            .Comments.Include(c => c.Artifact)
            .FirstOrDefaultAsync(c => c.Id == comment.Id);

        return MapToResponseDto(updatedComment!);
    }

    public async Task<bool> SoftDeleteCommentAsync(int id)
    {
        var comment = await _context.Comments.FindAsync(id);

        if (comment == null)
        {
            return false;
        }

        comment.IsDeleted = true;
        comment.DeletedAt = DateTime.UtcNow;
        comment.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private static CommentResponseDto MapToResponseDto(Comment comment)
    {
        return new CommentResponseDto
        {
            Id = comment.Id,
            VisitorName = comment.VisitorName,
            VisitorEmail = comment.VisitorEmail,
            Content = comment.Content,
            Rating = comment.Rating,
            ArtifactId = comment.ArtifactId,
            ArtifactTitle = comment.Artifact?.Title ?? string.Empty,
        };
    }
}
