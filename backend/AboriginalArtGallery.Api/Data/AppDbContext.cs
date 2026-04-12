using AboriginalArtGallery.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace AboriginalArtGallery.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // DbSets for each entity. these are the tables in the database

    public DbSet<AboriginalTribe> AboriginalTribes => Set<AboriginalTribe>();
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Artifact> Artifacts => Set<Artifact>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Exhibition> Exhibitions => Set<Exhibition>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure table names and relationships

        modelBuilder.Entity<AboriginalTribe>().ToTable("aboriginal_tribes");

        modelBuilder.Entity<Artist>().ToTable("artists");

        modelBuilder.Entity<Artifact>().ToTable("artifacts");

        modelBuilder.Entity<Comment>().ToTable("comments");

        modelBuilder.Entity<Exhibition>().ToTable("exhibitions");

        // Global query filters hide soft-deleted records from normal queries.
        modelBuilder.Entity<AboriginalTribe>().HasQueryFilter(t => !t.IsDeleted);
        modelBuilder.Entity<Artist>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<Artifact>().HasQueryFilter(a => !a.IsDeleted);
        modelBuilder.Entity<Comment>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Exhibition>().HasQueryFilter(e => !e.IsDeleted);

        // CreatedAt is set by the database when a new row is inserted.
        modelBuilder
            .Entity<AboriginalTribe>()
            .Property(t => t.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder
            .Entity<Artist>()
            .Property(a => a.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder
            .Entity<Artifact>()
            .Property(a => a.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder
            .Entity<Comment>()
            .Property(c => c.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder
            .Entity<Exhibition>()
            .Property(e => e.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // OriginLocation is PostGIS geometry type, so we need to specify the column type for EF Core to handle it correctly

        modelBuilder
            .Entity<AboriginalTribe>()
            .Property(t => t.OriginLocation)
            .HasColumnType("geometry (point, 4326)");

        modelBuilder
            .Entity<Artifact>()
            .Property(a => a.OriginLocation)
            .HasColumnType("geometry (point, 4326)");

        // relationships and delete rules

        modelBuilder
            .Entity<Artist>()
            .HasOne(a => a.AboriginalTribe)
            .WithMany(t => t.Artists)
            .HasForeignKey(a => a.AboriginalTribeId)
            .OnDelete(DeleteBehavior.Restrict); // Restrict deletion of tribe if artists are associated

        modelBuilder
            .Entity<Artifact>()
            .HasOne(a => a.Artist)
            .WithMany(ar => ar.Artifacts)
            .HasForeignKey(a => a.ArtistId)
            .OnDelete(DeleteBehavior.Restrict); // Restrict deletion of artist if artifacts are associated

        modelBuilder
            .Entity<Artifact>()
            .HasOne(a => a.AboriginalTribe)
            .WithMany(t => t.Artifacts)
            .HasForeignKey(a => a.AboriginalTribeId)
            .OnDelete(DeleteBehavior.Restrict); // Restrict deletion of tribe if artifacts are associated

        modelBuilder
            .Entity<Artifact>()
            .HasOne(a => a.Exhibition)
            .WithMany(e => e.Artifacts)
            .HasForeignKey(a => a.ExhibitionId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder
            .Entity<Comment>()
            .HasOne(c => c.Artifact)
            .WithMany(a => a.Comments)
            .HasForeignKey(c => c.ArtifactId)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete comments when an artifact is deleted
    }
}
