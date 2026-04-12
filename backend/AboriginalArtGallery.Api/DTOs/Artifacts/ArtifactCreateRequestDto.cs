using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Api.DTOs.Artifacts;

public class ArtifactCreateRequestDto
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public int? YearCreated { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [Required]
    [MaxLength(150)]
    public string Material { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ArtType { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string ArtStyle { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Era { get; set; } = string.Empty;

    [MaxLength(150)]
    public string OriginPlaceName { get; set; } = string.Empty;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    [Required]
    public int ArtistId { get; set; }

    [Required]
    public int AboriginalTribeId { get; set; }

    public int? ExhibitionId { get; set; }
}
