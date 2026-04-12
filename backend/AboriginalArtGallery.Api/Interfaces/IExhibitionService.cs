using AboriginalArtGallery.Api.DTOs.Exhibitions;

namespace AboriginalArtGallery.Api.Interfaces;

public interface IExhibitionService
{
    Task<List<ExhibitionResponseDto>> GetAllExhibitionsAsync();

    Task<ExhibitionResponseDto?> GetExhibitionByIdAsync(int id);

    Task<ExhibitionResponseDto> CreateExhibitionAsync(ExhibitionCreateRequestDto dto);

    Task<ExhibitionResponseDto?> UpdateExhibitionAsync(int id, ExhibitionUpdateRequestDto dto);

    Task<bool> SoftDeleteExhibitionAsync(int id);
}
