
using AngleSharp.Dom;
using System.Diagnostics;
using System.IO;
using System.IO.Pipelines;

namespace Music.Respositories;

public class AudioProcessorRepository : IAudioProcessor
{
    private readonly string ytpDlpDirectory = "C:\\yt-dlp";
    public async Task<Stream> ExtractFirst10SecondsAsync(string songName, string url)
    {
        var outputStream = new MemoryStream();

        string customTempDirectory = @"D:\musics";
        string outputPath = Path.Combine(customTempDirectory, songName + ".mp3");
        Console.WriteLine(outputPath);
        Console.WriteLine("15s");
        var ytDlpProcessStartInfo = new ProcessStartInfo()
        {
            FileName = "yt-dlp",
            Arguments = $"-f bestaudio --extract-audio --audio-format mp3 --audio-quality 0 --postprocessor-args \"-ss 0:0:0 -t 0:0:15\" -o {outputPath} {url}",
            RedirectStandardInput = true,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = Path.GetDirectoryName(ytpDlpDirectory)
        };

        using (var process = new Process { StartInfo = ytDlpProcessStartInfo })
        {
            process.Start();

            await process.WaitForExitAsync();
        }

        using (var fileStream = File.OpenRead(outputPath))
        {
            await fileStream.CopyToAsync(outputStream);
        }

        outputStream.Position = 0;
        Console.WriteLine($"Extracted audio size: {outputStream.Length} bytes");
        return outputStream;
    }
}
