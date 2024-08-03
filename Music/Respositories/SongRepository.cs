using Music.Data;
using Music.Models;
using Music.Respone;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Music.Respositories;

public class SongRepository : ISong
{
    private readonly IStorage _storage;
    private readonly MusicApplicationContext _dbContext;
    public SongRepository(IStorage storage, MusicApplicationContext dbContext)
    {
        _storage = storage;
        _dbContext = dbContext;
    }
    public async Task<SongResult> AddAsync(Song song, string ytUrl)
    {
        //can remove this step to cilent handle 
        var songExist = await _dbContext.Songs.FindAsync(song.Id);
        if (songExist == null) 
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = await _storage.DowloadSongAsync(song.Name, ytUrl);

            if (result.StatusCode == StatusCodes.Status201Created)
            {
                song.File_path = result.File_path;
                _dbContext.Songs.Add(song);
                await _dbContext.SaveChangesAsync();

                stopwatch.Stop();
                Console.WriteLine($"Time taken to download song: {stopwatch.Elapsed.TotalSeconds} ms");

                return new SongResult
                {
                    StatusCode = StatusCodes.Status201Created,
                };
                
            }
            else if (result.StatusCode == StatusCodes.Status500InternalServerError)
            {
                return new SongResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    ErrorMessage = "Internal Sever Error"
                };
            }
            return new SongResult
            {
                StatusCode = StatusCodes.Status400BadRequest,
                ErrorMessage = "Bad Request"
            };
        }
        else
        {
            var outputStream2 = new MemoryStream();
            Console.WriteLine("Song aready exists " + songExist.File_path);
            string directory = Path.GetDirectoryName(song.File_path);
            string fileName = Path.GetFileName(song.File_path);
            string sanitizedFileName = fileName.Replace(":", "#");
            string sanitizedOutputPath = Path.Combine(directory, sanitizedFileName);
            using (var fileStream = File.OpenRead(sanitizedOutputPath))
            {
                await fileStream.CopyToAsync(outputStream2);
                outputStream2.Position = 0;
                return new SongResult
                {
                    Song = outputStream2,
                    StatusCode = StatusCodes.Status201Created,
                };
            }
        }
    }

    public async Task<Song> FindByIdAsync(string id)
    {
        var song = await _dbContext.Songs.FindAsync(id);
        if (song == null)
        {
            return null;
        }
        else
        {
            return song;
        }
    }

    public async Task<SongResult> GetByIdAsync(string id)
    {
        var outputStream = new MemoryStream();
        var song = await _dbContext.Songs.FindAsync(id);
        if(song == null)
        {
            return new SongResult
            {
                StatusCode = StatusCodes.Status404NotFound
            };
        }
        else
        {
            Console.WriteLine(song.File_path);
            string directory = Path.GetDirectoryName(song.File_path);
            string fileName = Path.GetFileName(song.File_path);
            string sanitizedFileName = fileName.Replace(":", "#");
            string sanitizedOutputPath = Path.Combine(directory, sanitizedFileName);
            Console.WriteLine(sanitizedOutputPath);
            using (var fileStream = File.OpenRead(sanitizedOutputPath))
            {
                await fileStream.CopyToAsync(outputStream);
            }
            outputStream.Position = 0;
            return new SongResult
            {
                Song = outputStream,
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
