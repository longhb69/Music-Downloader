using Music.Models;
using Music.Respone;

namespace Music.Respositories;

public interface ISong
{
    Task<SongResult> AddAsync(Song song, string ytUrl);
    Task<SongResult> GetByIdAsync(string id);   
    Task<Song> FindByIdAsync(string id);
}
