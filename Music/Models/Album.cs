namespace Music.Models;

public class Album
{
    public string Id { get; set; }  
    public string Name { get; set; }
    public DateTime ReleaRelease_date { get; set; }
    public int total_tracks { get; set; }
    public ICollection<Image> Images { get; set; }
    public ICollection<Artist> Artists { get; set; }
    public ICollection<Song> Songs { get; set; }   
}
