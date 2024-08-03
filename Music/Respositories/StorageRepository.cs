
using Music.Models;
using Music.Respone;
using System.Diagnostics;

namespace Music.Respositories;

public class StorageRepository : IStorage
{
    private readonly string ytpDlpDirectory = "C:\\yt-dlp";

    public async Task<Stream> DowloadAndReturn(string songName, string url)
    {
        string customTempDirectory = @"D:\musics";
        string outputPath = Path.Combine(customTempDirectory, songName + ".mp3");
        try
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "yt-dlp",
                Arguments = $"-f bestaudio --extract-audio --audio-format mp3 --audio-quality 0 -o \"{outputPath}\" {url}",
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(ytpDlpDirectory)
            };
            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();

                var stdOutPut = await process.StandardOutput.ReadToEndAsync();
                var stdError = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();
                if (process.ExitCode != 0)
                {
                    return null;
                }
                else
                {
                    var outputStream = new MemoryStream();
                    //using (var fileStream = File.OpenRead(outputPath))
                    //{
                    //    await fileStream.CopyToAsync(outputStream);
                    //}
                    //outputStream.Position = 0;
                    return outputStream;
                }
            };
        
        }
        catch (Exception ex)
        {
           return null;

        }
    }

    public async Task<DowloadSongResult> DowloadSongAsync(string songName, string url)
    {

        string customTempDirectory = @"D:\musics";
        string outputPath = Path.Combine(customTempDirectory, songName + ".mp3");
        Console.WriteLine("Extrac from youtube song ");
        Console.WriteLine($"{outputPath}");
        try
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = "yt-dlp",
                Arguments = $"-f bestaudio --extract-audio --audio-format mp3 --audio-quality 0 -o \"{outputPath}\" {url}",
                RedirectStandardInput = true,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Path.GetDirectoryName(ytpDlpDirectory)
            };
            using (var process = new Process { StartInfo = processStartInfo })
            {
                process.Start();

                var stdOutPut = await process.StandardOutput.ReadToEndAsync();
                var stdError = await process.StandardError.ReadToEndAsync();
                await process.WaitForExitAsync();
                if(process.ExitCode != 0) 
                {
                    return new DowloadSongResult
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        ErrorMessage = stdError
                    };
                } 
                else
                {
                    string directory = Path.GetDirectoryName(outputPath);
                    string fileName = Path.GetFileName(outputPath);
                    string sanitizedFileName = fileName.Replace(":", "#");
                    string sanitizedOutputPath = Path.Combine(directory, sanitizedFileName);

                    return new DowloadSongResult
                    {
                        File_path = sanitizedOutputPath,
                        StatusCode = StatusCodes.Status201Created
                    };
                }
            };
        } catch (Exception ex)
        {
            return new DowloadSongResult
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                ErrorMessage = ex.ToString()
            };
        }
    }

}


