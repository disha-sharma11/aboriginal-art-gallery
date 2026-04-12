using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace AboriginalArtGallery.Api.Models;

public class Artifact
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public int? YearCreated { get; set; } // ? - Optional, as some artifacts may not have a known creation year

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

    public Point? OriginLocation { get; set; } // ? - Optional, as some artifacts may not have a specific origin location

    public int ArtistId { get; set; }

    public Artist? Artist { get; set; } // ? - Optional, as some artifacts may not have a known artist (e.g., ancient artifacts)

    public int AboriginalTribeId { get; set; }

    public AboriginalTribe? AboriginalTribe { get; set; } // ? - Optional, as some artifacts may not be associated with a specific tribe

    public int? ExhibitionId { get; set; }

    public Exhibition? Exhibition { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public List<Comment> Comments { get; set; } = new();
}
