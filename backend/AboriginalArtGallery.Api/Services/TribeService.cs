using AboriginalArtGallery.Api.Data;
using AboriginalArtGallery.Api.DTOs.Artifacts;
using AboriginalArtGallery.Api.DTOs.Artists;
using AboriginalArtGallery.Api.DTOs.Tribes;
using AboriginalArtGallery.Api.Interfaces;
using AboriginalArtGallery.Api.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace AboriginalArtGallery.Api.Services;

public class TribeService : ITribeService
{
    private readonly AppDbContext _context;

    public TribeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<TribeResponseDto>> GetAllTribesAsync()
    {
        var tribes = await _context.AboriginalTribes.ToListAsync();

        return tribes.Select(t => MapToResponseDto(t)).ToList();
    }

    public async Task<TribeResponseDto?> GetTribeByIdAsync(int id)
    {
        var tribe = await _context.AboriginalTribes.FindAsync(id);

        return tribe == null ? null : MapToResponseDto(tribe);
    }

    public async Task<TribeResponseDto> CreateTribeAsync(TribeCreateRequestDto dto)
    {
        var tribe = new AboriginalTribe
        {
            Name = dto.Name,
            Description = dto.Description,
            StateOrTerritory = dto.StateOrTerritory,
            OriginRegionName = dto.OriginRegionName,
            OriginLocation = CreatePoint(dto.Latitude, dto.Longitude),
        };

        _context.AboriginalTribes.Add(tribe);
        await _context.SaveChangesAsync();

        return MapToResponseDto(tribe);
    }

    public async Task<TribeResponseDto?> UpdateTribeAsync(int id, TribeUpdateRequestDto dto)
    {
        var tribe = await _context.AboriginalTribes.FindAsync(id);

        if (tribe == null)
        {
            return null;
        }

        tribe.Name = dto.Name;
        tribe.Description = dto.Description;
        tribe.StateOrTerritory = dto.StateOrTerritory;
        tribe.OriginRegionName = dto.OriginRegionName;
        tribe.OriginLocation = CreatePoint(dto.Latitude, dto.Longitude);
        tribe.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponseDto(tribe);
    }

    public async Task<bool> SoftDeleteTribeAsync(int id)
    {
        var tribe = await _context.AboriginalTribes.FindAsync(id);

        if (tribe == null)
        {
            return false;
        }

        tribe.IsDeleted = true;
        tribe.DeletedAt = DateTime.UtcNow;
        tribe.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<ArtistResponseDto>?> GetArtistsByTribeIdAsync(int id)
    {
        var tribeExists = await _context.AboriginalTribes.AnyAsync(t => t.Id == id);

        if (!tribeExists)
        {
            return null;
        }

        var artists = await _context
            .Artists.Include(a => a.AboriginalTribe)
            .Where(a => a.AboriginalTribeId == id)
            .ToListAsync();

        return artists.Select(a => ArtistService.MapToResponseDto(a)).ToList();
    }

    public async Task<List<ArtifactResponseDto>?> GetArtifactsByTribeIdAsync(int id)
    {
        var tribeExists = await _context.AboriginalTribes.AnyAsync(t => t.Id == id);

        if (!tribeExists)
        {
            return null;
        }

        var artifacts = await _context
            .Artifacts.Include(a => a.Artist)
            .Include(a => a.AboriginalTribe)
            .Include(a => a.Exhibition)
            .Where(a => a.AboriginalTribeId == id)
            .ToListAsync();

        return artifacts.Select(a => ArtifactService.MapToResponseDto(a)).ToList();
    }

    private static Point? CreatePoint(double? latitude, double? longitude)
    {
        if (!latitude.HasValue || !longitude.HasValue)
        {
            return null;
        }

        return new Point(longitude.Value, latitude.Value) { SRID = 4326 };
    }

    private static TribeResponseDto MapToResponseDto(AboriginalTribe tribe)
    {
        return new TribeResponseDto
        {
            Id = tribe.Id,
            Name = tribe.Name,
            Description = tribe.Description,
            StateOrTerritory = tribe.StateOrTerritory,
            OriginRegionName = tribe.OriginRegionName,
            Latitude = tribe.OriginLocation?.Y,
            Longitude = tribe.OriginLocation?.X,
        };
    }
}
