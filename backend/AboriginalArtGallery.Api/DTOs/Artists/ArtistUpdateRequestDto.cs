using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Api.DTOs.Artists;

public class ArtistUpdateRequestDto
{
    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Biography { get; set; } = string.Empty;

    public int? BirthYear { get; set; }

    public int? DeathYear { get; set; }

    [MaxLength(150)]
    public string Region { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? PhotoUrl { get; set; }

    [Required]
    public int AboriginalTribeId { get; set; }
}
