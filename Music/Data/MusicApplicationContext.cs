using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Music.Models;

namespace Music.Data;

public class MusicApplicationContext : DbContext
{
    private readonly IConfiguration _configuration;
    public MusicApplicationContext(IConfiguration configuration)
    {
        _configuration = configuration; 
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_configuration.GetConnectionString("AuthDbContextConnection"));
    }
    public DbSet<Song> Songs { get; set; }
    public DbSet<Album> Albums { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(a => a.Id);   

            entity.HasMany(a => a.Songs)
                .WithOne(s => s.Album)
                .HasForeignKey(s => s.AlbumId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired();
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.AlbumId).IsRequired(false);
        });

        base.OnModelCreating(modelBuilder);
    }
}
