using Music.Respone;

namespace Music.Respositories;

public interface IStorage
{
    Task<DowloadSongResult> DowloadSongAsync(string songName, string url);
    Task<Stream> DowloadAndReturn(string songName, string url);
}
