using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

namespace AboriginalArtGallery.Api.Models;

public class AboriginalTribe
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty; // string.Empty - default value to avoid nulls as it is a required field

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(100)]
    public string StateOrTerritory { get; set; } = string.Empty;

    [MaxLength(150)]
    public string OriginRegionName { get; set; } = string.Empty;

    // [Column(TypeName = "geometry (point, 4326)")]
    // could use this above Point. but instead we will configure the column type in the DbContext because it is 
    // better to keep all database-related configurations in one place (DbContext) rather than scattering them across multiple model classes.
    public Point? OriginLocation { get; set; } // ? - Optional, as some tribes may not have a specific location

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public List<Artist> Artists { get; set; } = new(); // relationship to artists

    public List<Artifact> Artifacts { get; set; } = new(); // relationship to artifacts
}
