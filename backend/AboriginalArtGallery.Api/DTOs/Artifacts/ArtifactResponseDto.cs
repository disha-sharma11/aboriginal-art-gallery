namespace AboriginalArtGallery.Api.DTOs.Artifacts;

public class ArtifactResponseDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int? YearCreated { get; set; }

    public string? ImageUrl { get; set; }

    public string Material { get; set; } = string.Empty;

    public string ArtType { get; set; } = string.Empty;

    public string ArtStyle { get; set; } = string.Empty;

    public string Era { get; set; } = string.Empty;

    public string OriginPlaceName { get; set; } = string.Empty;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = string.Empty;

    public int AboriginalTribeId { get; set; }

    public string TribeName { get; set; } = string.Empty;

    public int? ExhibitionId { get; set; }

    public string? ExhibitionName { get; set; }
}
