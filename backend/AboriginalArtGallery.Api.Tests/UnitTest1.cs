using AboriginalArtGallery.Api.Data;
using AboriginalArtGallery.Api.DTOs.Artists;
using AboriginalArtGallery.Api.Models;
using AboriginalArtGallery.Api.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace AboriginalArtGallery.Api.Tests;

public class ArtistServiceTests
{
    [Fact]
    public async Task CreateArtistAsync_WhenTribeExists_CreatesArtist()
    {
        await using var context = CreateDbContext();

        context.AboriginalTribes.Add(
            new AboriginalTribe
            {
                Id = 1,
                Name = "Wurundjeri",
                Description = "Traditional custodians of the Melbourne region.",
                StateOrTerritory = "Victoria",
                OriginRegionName = "Kulin Nation",
            }
        );

        await context.SaveChangesAsync();

        var service = new ArtistService(context);

        var result = await service.CreateArtistAsync(
            new ArtistCreateRequestDto
            {
                FullName = "Test Artist",
                Biography = "A test biography.",
                BirthYear = 1980,
                Region = "Victoria",
                PhotoUrl = "https://example.com/artist.jpg",
                AboriginalTribeId = 1,
            }
        );

        result.Should().NotBeNull();
        result!.FullName.Should().Be("Test Artist");
        result.TribeName.Should().Be("Wurundjeri");
        context.Artists.Should().ContainSingle(a => a.FullName == "Test Artist");
    }

    [Fact]
    public async Task CreateArtistAsync_WhenTribeDoesNotExist_ReturnsNull()
    {
        await using var context = CreateDbContext();
        var service = new ArtistService(context);

        var result = await service.CreateArtistAsync(
            new ArtistCreateRequestDto
            {
                FullName = "Test Artist",
                Biography = "A test biography.",
                Region = "Victoria",
                AboriginalTribeId = 999,
            }
        );

        result.Should().BeNull();
        context.Artists.Should().BeEmpty();
    }

    private static AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
