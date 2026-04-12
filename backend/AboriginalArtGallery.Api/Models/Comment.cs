using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Api.Models;

public class Comment
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string VisitorName { get; set; } = string.Empty; // string.Empty - default value to avoid nulls as it is a required field

    [MaxLength(150)]
    public string? VisitorEmail { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    public int? Rating { get; set; } // ? - Optional, as not all comments may include a rating

    public int ArtifactId { get; set; }

    public Artifact? Artifact { get; set; } // navigation property to the associated artifact

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }
}
