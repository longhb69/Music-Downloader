namespace Music.Respositories;

public interface IAudioProcessor
{
    public Task<Stream> ExtractFirst10SecondsAsync(string songName, string url);
}
