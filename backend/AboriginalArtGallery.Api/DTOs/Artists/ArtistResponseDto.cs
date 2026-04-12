namespace AboriginalArtGallery.Api.DTOs.Artists;

public class ArtistResponseDto
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Biography { get; set; } = string.Empty;

    public int? BirthYear { get; set; }

    public int? DeathYear { get; set; }

    public string Region { get; set; } = string.Empty;

    public string? PhotoUrl { get; set; }

    public int AboriginalTribeId { get; set; }

    public string TribeName { get; set; } = string.Empty;

}
