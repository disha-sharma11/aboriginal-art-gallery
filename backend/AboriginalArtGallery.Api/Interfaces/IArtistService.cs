using AboriginalArtGallery.Api.DTOs.Artifacts;
using AboriginalArtGallery.Api.DTOs.Artists;

namespace AboriginalArtGallery.Api.Interfaces;

public interface IArtistService
{
    Task<List<ArtistResponseDto>> GetAllArtistsAsync();

    Task<ArtistResponseDto?> GetArtistByIdAsync(int id);

    Task<ArtistResponseDto?> CreateArtistAsync(ArtistCreateRequestDto dto);

    Task<ArtistResponseDto?> UpdateArtistAsync(int id, ArtistUpdateRequestDto dto);

    Task<bool> SoftDeleteArtistAsync(int id);

    Task<List<ArtifactResponseDto>?> GetArtifactsByArtistIdAsync(int id);
}
