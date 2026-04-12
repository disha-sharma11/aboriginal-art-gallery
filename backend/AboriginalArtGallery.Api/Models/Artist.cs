using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Api.Models;

public class Artist
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string FullName { get; set; } = string.Empty; // string.Empty - default value to avoid nulls as it is a required field

    [Required]
    [MaxLength(2000)]
    public string Biography { get; set; } = string.Empty;

    public int? BirthYear { get; set; } // ? - Optional, as some artists may not have a known birth year

    public int? DeathYear { get; set; } // ? - Optional, as some artists may still be alive or may not have a known death year

    [MaxLength(150)]
    public string Region { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? PhotoUrl { get; set; }

    public int AboriginalTribeId { get; set; }

    public AboriginalTribe? AboriginalTribe { get; set; } // ? - Optional, as some artists may not be associated with a specific tribe

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }

    public List<Artifact> Artifacts { get; set; } = new();
}
