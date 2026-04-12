using AboriginalArtGallery.Api.DTOs.Artifacts;
using AboriginalArtGallery.Api.DTOs.Artists;
using AboriginalArtGallery.Api.DTOs.Tribes;

namespace AboriginalArtGallery.Api.Interfaces;

public interface ITribeService
{
    Task<List<TribeResponseDto>> GetAllTribesAsync();

    Task<TribeResponseDto?> GetTribeByIdAsync(int id);

    Task<TribeResponseDto> CreateTribeAsync(TribeCreateRequestDto dto);

    Task<TribeResponseDto?> UpdateTribeAsync(int id, TribeUpdateRequestDto dto);

    Task<bool> SoftDeleteTribeAsync(int id);

    Task<List<ArtistResponseDto>?> GetArtistsByTribeIdAsync(int id);

    Task<List<ArtifactResponseDto>?> GetArtifactsByTribeIdAsync(int id);
}
