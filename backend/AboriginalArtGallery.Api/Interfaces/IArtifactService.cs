using AboriginalArtGallery.Api.DTOs.Artifacts;

namespace AboriginalArtGallery.Api.Interfaces;

public interface IArtifactService
{
    Task<List<ArtifactResponseDto>> GetAllArtifactsAsync();

    Task<ArtifactResponseDto?> GetArtifactByIdAsync(int id);

    Task<ArtifactResponseDto?> CreateArtifactAsync(ArtifactCreateRequestDto dto);

    Task<ArtifactResponseDto?> UpdateArtifactAsync(int id, ArtifactUpdateRequestDto dto);

    Task<bool> SoftDeleteArtifactAsync(int id);

    Task<List<ArtifactResponseDto>?> GetArtifactsByArtistIdAsync(int artistId);

    Task<List<ArtifactResponseDto>?> GetArtifactsByTribeIdAsync(int tribeId);
}
