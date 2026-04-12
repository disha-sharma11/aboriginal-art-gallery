using AboriginalArtGallery.Api.Data;
using AboriginalArtGallery.Api.DTOs.Exhibitions;
using AboriginalArtGallery.Api.Interfaces;
using AboriginalArtGallery.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AboriginalArtGallery.Api.Services;

public class ExhibitionService : IExhibitionService
{
    private readonly AppDbContext _context;

    public ExhibitionService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ExhibitionResponseDto>> GetAllExhibitionsAsync()
    {
        var exhibitions = await _context // await because it is an asynchronous operation that retrieves data from the database
            .Exhibitions.OrderBy(e => e.StartDate)
            .ToListAsync();

        return exhibitions.Select(e => MapToResponseDto(e)).ToList();
    }

    public async Task<ExhibitionResponseDto?> GetExhibitionByIdAsync(int id)
    {
        var exhibition = await _context.Exhibitions.FirstOrDefaultAsync(e => e.Id == id);

        if (exhibition == null)
        {
            return null;
        }

        return MapToResponseDto(exhibition);
    }

    public async Task<ExhibitionResponseDto> CreateExhibitionAsync(ExhibitionCreateRequestDto dto)
    {
        var exhibition = new Exhibition
        {
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Location = dto.Location,
        };

        _context.Exhibitions.Add(exhibition);
        await _context.SaveChangesAsync();

        return MapToResponseDto(exhibition);
    }

    public async Task<ExhibitionResponseDto?> UpdateExhibitionAsync(
        int id,
        ExhibitionUpdateRequestDto dto
    )
    {
        var exhibition = await _context.Exhibitions.FindAsync(id);

        if (exhibition == null)
        {
            return null;
        }

        exhibition.Name = dto.Name;
        exhibition.Description = dto.Description;
        exhibition.StartDate = dto.StartDate;
        exhibition.EndDate = dto.EndDate;
        exhibition.Location = dto.Location;
        exhibition.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MapToResponseDto(exhibition);
    }

    public async Task<bool> SoftDeleteExhibitionAsync(int id)
    {
        var exhibition = await _context.Exhibitions.FindAsync(id);

        if (exhibition == null)
        {
            return false;
        }

        exhibition.IsDeleted = true;
        exhibition.DeletedAt = DateTime.UtcNow;
        exhibition.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    private static ExhibitionResponseDto MapToResponseDto(Exhibition exhibition)
    {
        return new ExhibitionResponseDto
        {
            Id = exhibition.Id,
            Name = exhibition.Name,
            Description = exhibition.Description,
            StartDate = exhibition.StartDate,
            EndDate = exhibition.EndDate,
            Location = exhibition.Location,
            CreatedAt = exhibition.CreatedAt,
            UpdatedAt = exhibition.UpdatedAt,
        };
    }
}
