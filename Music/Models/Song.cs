using System.ComponentModel.DataAnnotations;

namespace Music.Models;

public class Song
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime Release_date { get; set; }
    public int durations_ms { get; set; }
    public string File_path { get; set; }
    [Url]
    public string YoutubeUrl { get; set; }
    public ICollection<Image> Images { get; set; }
    public ICollection<Artist> Artists { get; set; }  
    public string? AlbumId { get; set; }
    public Album Album { get; set; }
}
