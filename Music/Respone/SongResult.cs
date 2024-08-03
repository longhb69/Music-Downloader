using Music.Models;

namespace Music.Respone;

public class SongResult
{
    public Stream? Song {  get; set; }
    public int StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
}
