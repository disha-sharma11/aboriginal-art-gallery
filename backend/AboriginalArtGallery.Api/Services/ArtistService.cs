using AboriginalArtGallery.Api.Data;
using AboriginalArtGallery.Api.DTOs.Artifacts;
using AboriginalArtGallery.Api.DTOs.Artists;
using AboriginalArtGallery.Api.Interfaces;
using AboriginalArtGallery.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AboriginalArtGallery.Api.Services;

public class ArtistService : IArtistService
{
    private readonly AppDbContext _context;

    public ArtistService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ArtistResponseDto>> GetAllArtistsAsync()
    {
        var artists = await _context
            .Artists.Include(a => a.AboriginalTribe)
            .ToListAsync();

        return artists.Select(a => MapToResponseDto(a)).ToList();
    }

    public async Task<ArtistResponseDto?> GetArtistByIdAsync(int id)
    {
        var artist = await _context
            .Artists.Include(a => a.AboriginalTribe)
            .FirstOrDefaultAsync(a => a.Id == id);

        return artist == null ? null : MapToResponseDto(artist);
    }

    public async Task<ArtistResponseDto?> CreateArtistAsync(ArtistCreateRequestDto dto)
    {
        var tribeExists = await _context.AboriginalTribes.AnyAsync(t =>
            t.Id == dto.AboriginalTribeId
        );

        if (!tribeExists)
        {
            return null;
        }

        var artist = new Artist
        {
            FullName = dto.FullName,
            Biography = dto.Biography,
            BirthYear = dto.BirthYear,
            DeathYear = dto.DeathYear,
            Region = dto.Region,
            PhotoUrl = dto.PhotoUrl,
            AboriginalTribeId = dto.AboriginalTribeId,
        };

        _context.Artists.Add(artist);
        await _context.SaveChangesAsync();

        var createdArtist = await _context
            .Artists.Include(a => a.AboriginalTribe)
            .FirstOrDefaultAsync(a => a.Id == artist.Id);

        return MapToResponseDto(createdArtist!);
    }

    public async Task<ArtistResponseDto?> UpdateArtistAsync(int id, ArtistUpdateRequestDto dto)
    {
        var artist = await _context.Artists.FindAsync(id);

        if (artist == null)
        {
            return null;
        }

        var tribeExists = await _context.AboriginalTribes.AnyAsync(t =>
            t.Id == dto.AboriginalTribeId
        );

        if (!tribeExists)
        {
            return null;
        }

        artist.FullName = dto.FullName;
        artist.Biography = dto.Biography;
        artist.BirthYear = dto.BirthYear;
        artist.DeathYear = dto.DeathYear;
        artist.Region = dto.Region;
        artist.PhotoUrl = dto.PhotoUrl;
        artist.AboriginalTribeId = dto.AboriginalTribeId;
        artist.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        var updatedArtist = await _context
            .Artists.Include(a => a.AboriginalTribe)
            .FirstOrDefaultAsync(a => a.Id == artist.Id);

        return MapToResponseDto(updatedArtist!);
    }

    public async Task<bool> SoftDeleteArtistAsync(int id)
    {
        var artist = await _context.Artists.FindAsync(id);

        if (artist == null)
        {
            return false;
        }

        artist.IsDeleted = true;
        artist.DeletedAt = DateTime.UtcNow;
        artist.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<List<ArtifactResponseDto>?> GetArtifactsByArtistIdAsync(int id)
    {
        var artistExists = await _context.Artists.AnyAsync(a => a.Id == id);

        if (!artistExists)
        {
            return null;
        }

        var artifacts = await _context
            .Artifacts.Include(a => a.Artist)
            .Include(a => a.AboriginalTribe)
            .Include(a => a.Exhibition)
            .Where(a => a.ArtistId == id)
            .ToListAsync();

        return artifacts.Select(a => ArtifactService.MapToResponseDto(a)).ToList();
    }

    public static ArtistResponseDto MapToResponseDto(Artist artist)
    {
        return new ArtistResponseDto
        {
            Id = artist.Id,
            FullName = artist.FullName,
            Biography = artist.Biography,
            BirthYear = artist.BirthYear,
            DeathYear = artist.DeathYear,
            Region = artist.Region,
            PhotoUrl = artist.PhotoUrl,
            AboriginalTribeId = artist.AboriginalTribeId,
            TribeName = artist.AboriginalTribe?.Name ?? string.Empty,
        };
    }
}
