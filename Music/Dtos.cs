using Music.Models;
using System.ComponentModel.DataAnnotations;

namespace Music;

public record CreateSongDto(
    [Required] string Id,
    [Required] string Name,
    DateTime Release_date,
    int durations_ms,
    [Url]
    string YoutubeUrl
    //ICollection<Image> Images
);

