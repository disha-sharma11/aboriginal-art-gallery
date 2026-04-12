using AboriginalArtGallery.Api.Data;
using AboriginalArtGallery.Api.DTOs.Artifacts;
using AboriginalArtGallery.Api.Interfaces;
using AboriginalArtGallery.Api.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace AboriginalArtGallery.Api.Services;

public class ArtifactService : IArtifactService
{
    private readonly AppDbContext _context;

    public ArtifactService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ArtifactResponseDto>> GetAllArtifactsAsync()
    {
        var artifacts = await BaseArtifactQuery().ToListAsync();

        return artifacts.Select(a => MapToResponseDto(a)).ToList();
    }

    public async Task<ArtifactResponseDto?> GetArtifactByIdAsync(int id)
    {
        var artifact = await BaseArtifactQuery().FirstOrDefaultAsync(a => a.Id == id);

        return artifact == null ? null : MapToResponseDto(artifact);
    }

    public async Task<ArtifactResponseDto?> CreateArtifactAsync(ArtifactCreateRequestDto dto)
    {
        var relatedRecordsExist = await RelatedRecordsExistAsync(
            dto.ArtistId,
            dto.AboriginalTribeId,
            dto.ExhibitionId
        );

        if (!relatedRecordsExist)
        {
            return null;
        }

        var artifact = new Artifact
        {
            Title = dto.Title,
            Description = dto.Description,
            YearCreated = dto.YearCreated,
            ImageUrl = dto.ImageUrl,
            Material = dto.Material,
            ArtType = dto.ArtType,
            ArtStyle = dto.ArtStyle,
            Era = dto.Era,
            OriginPlaceName = dto.OriginPlaceName,
            OriginLocation = CreatePoint(dto.Latitude, dto.Longitude),
            ArtistId = dto.ArtistId,
            AboriginalTribeId = dto.AboriginalTribeId,
            ExhibitionId = dto.ExhibitionId,
        };

        _context.Artifacts.Add(artifact);
        await _context.SaveChangesAsync();

        var createdArtifact = await BaseArtifactQuery().FirstOrDefaultAsync(a => a.Id == artifact.Id);

        return MapToResponseDto(createdArtifact!);
    }

    public async Task<ArtifactResponseDto?> UpdateArtifactAsync(int id, ArtifactUpdateRequestDto dto)
    {
        var artifact = await _context.Artifacts.FindAsync(id);

        if (artifact == null)
        {
            return null;
        }

        var relatedRecordsExist = await RelatedRecordsExistAsync(
            dto.ArtistId,
            dto.AboriginalTribeId,
            dto.ExhibitionId
        );

        if (!relatedRecordsExist)
        {
            return null;
        }

        artifact.Title = dto.Title;
        artifact.Description = dto.Description;
        artifact.YearCreated = dto.YearCreated;
        artifact.ImageUrl = dto.ImageUrl;
        artifact.Material = dto.Material;
        artifact.ArtType = dto.ArtType;
        artifact.ArtStyle = dto.ArtStyle;
        artifact.Era = dto.Era;
        artifact.OriginPlaceName = dto.OriginPlaceName;
        artifact.OriginLocation = CreatePoint(dto.Latitude, dto.Longitude);
        artifact.ArtistId = dto.ArtistId;
        artifact.AboriginalTribeId = dto.AboriginalTribeId;
        artifact.ExhibitionId = dto.ExhibitionId;
        artifact.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var updatedArtifact = await BaseArtifactQuery().FirstOrDefaultAsync(a => a.Id == artifact.Id);

        return MapToResponseDto(updatedArtifact!);
    }

    public async Task<bool> SoftDeleteArtifactAsync(int id)
    {
        var artifact = await _context.Artifacts.FindAsync(id);

        if (artifact == null)
        {
            return false;
        }

        artifact.IsDeleted = true;
        artifact.DeletedAt = DateTime.UtcNow;
        artifact.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<ArtifactResponseDto>?> GetArtifactsByArtistIdAsync(int artistId)
    {
        var artistExists = await _context.Artists.AnyAsync(a => a.Id == artistId);

        if (!artistExists)
        {
            return null;
        }

        var artifacts = await BaseArtifactQuery()
            .Where(a => a.ArtistId == artistId)
            .ToListAsync();

        return artifacts.Select(a => MapToResponseDto(a)).ToList();
    }

    public async Task<List<ArtifactResponseDto>?> GetArtifactsByTribeIdAsync(int tribeId)
    {
        var tribeExists = await _context.AboriginalTribes.AnyAsync(t => t.Id == tribeId);

        if (!tribeExists)
        {
            return null;
        }

        var artifacts = await BaseArtifactQuery()
            .Where(a => a.AboriginalTribeId == tribeId)
            .ToListAsync();

        return artifacts.Select(a => MapToResponseDto(a)).ToList();
    }

    private IQueryable<Artifact> BaseArtifactQuery()
    {
        return _context
            .Artifacts.Include(a => a.Artist)
            .Include(a => a.AboriginalTribe)
            .Include(a => a.Exhibition);
    }

    private async Task<bool> RelatedRecordsExistAsync(
        int artistId,
        int aboriginalTribeId,
        int? exhibitionId
    )
    {
        var artistExists = await _context.Artists.AnyAsync(a => a.Id == artistId);
        var tribeExists = await _context.AboriginalTribes.AnyAsync(t => t.Id == aboriginalTribeId);

        if (!artistExists || !tribeExists)
        {
            return false;
        }

        if (!exhibitionId.HasValue)
        {
            return true;
        }

        return await _context.Exhibitions.AnyAsync(e => e.Id == exhibitionId.Value);
    }

    private static Point? CreatePoint(double? latitude, double? longitude)
    {
        if (!latitude.HasValue || !longitude.HasValue)
        {
            return null;
        }

        return new Point(longitude.Value, latitude.Value) { SRID = 4326 };
    }

    public static ArtifactResponseDto MapToResponseDto(Artifact artifact)
    {
        return new ArtifactResponseDto
        {
            Id = artifact.Id,
            Title = artifact.Title,
            Description = artifact.Description,
            YearCreated = artifact.YearCreated,
            ImageUrl = artifact.ImageUrl,
            Material = artifact.Material,
            ArtType = artifact.ArtType,
            ArtStyle = artifact.ArtStyle,
            Era = artifact.Era,
            OriginPlaceName = artifact.OriginPlaceName,
            Latitude = artifact.OriginLocation?.Y,
            Longitude = artifact.OriginLocation?.X,
            ArtistId = artifact.ArtistId,
            ArtistName = artifact.Artist?.FullName ?? string.Empty,
            AboriginalTribeId = artifact.AboriginalTribeId,
            TribeName = artifact.AboriginalTribe?.Name ?? string.Empty,
            ExhibitionId = artifact.ExhibitionId,
            ExhibitionName = artifact.Exhibition?.Name,
        };
    }
}
