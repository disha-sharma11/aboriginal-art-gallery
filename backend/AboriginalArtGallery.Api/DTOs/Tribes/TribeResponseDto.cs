namespace AboriginalArtGallery.Api.DTOs.Tribes;

// This DTO is used for sending tribe data back to the client, including all relevant details
// all properties are included to provide comprehensive information about the tribe in the response
public class TribeResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string StateOrTerritory { get; set; } = string.Empty;

    public string OriginRegionName { get; set; } = string.Empty;

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }
}
