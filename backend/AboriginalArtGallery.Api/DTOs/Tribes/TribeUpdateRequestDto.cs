using System.ComponentModel.DataAnnotations;

namespace AboriginalArtGallery.Api.DTOs.Tribes;
// This DTO is used for updating tribe data, it includes all properties that can be updated
public class TribeUpdateRequestDto
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(100)]
    public string StateOrTerritory { get; set; } = string.Empty;

    [MaxLength(150)]
    public string OriginRegionName { get; set; } = string.Empty;

    // here we use Latitude and Longitude instead of Point because it is easier to work with in the DTOs,
    // and we can convert it to Point in the service layer when we need to save it to the database
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}
