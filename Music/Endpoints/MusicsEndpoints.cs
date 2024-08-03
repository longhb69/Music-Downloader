using AngleSharp.Io;
using Music.Models;
using Music.Respositories;
using System.Diagnostics;
using System.IO;
using YoutubeExplode.Videos.Streams;

namespace Music.Endpoints;

public static class MusicsEndpoints
{
    const string GetSongEndpointName = "GetSong";
    public static RouteGroupBuilder MapMusicsEndpoints( this WebApplication app)
    {
        var group = app.MapGroup("musics");

        group.MapPost("/", async (CreateSongDto createSongDto, string ytUrl, string albumId, HttpResponse response, ISong songRepository) => {

            Song song = new ()
            {
                Id = createSongDto.Id,
                Name = createSongDto.Name,
                Release_date = createSongDto.Release_date,
                durations_ms = createSongDto.durations_ms,
                YoutubeUrl = ytUrl,
                //Images = createSongDto.Images
            };
            var result = await songRepository.AddAsync(song, ytUrl);

            if (result.StatusCode == StatusCodes.Status201Created)
            {
                return Results.CreatedAtRoute(GetSongEndpointName, new { id = createSongDto?.Id }, song);
            }
            else if (result.StatusCode == StatusCodes.Status500InternalServerError)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Results.BadRequest(result.ErrorMessage);
        });

        group.MapGet("/{id}", async (string id, ISong songRepository, HttpResponse response) =>
        {
            var song = await songRepository.FindByIdAsync(id);
            if(song == null)
            {
                return Results.StatusCode(StatusCodes.Status404NotFound);
            }
            else
            {
                return Results.Ok(song);
            }
     
        }).WithName(GetSongEndpointName);


        group.MapGet("/long", async (string ytUrl, string songName, IStorage storageRepository, HttpResponse response) =>
        {
            var stream = await storageRepository.DowloadAndReturn(songName, ytUrl);
            return Results.Stream(stream, "audio/mpeg", $"{songName}.mp3");
        });

        group.MapGet("/streaming/{id}", async (string id, ISong songRepository, HttpResponse response) =>
        {

            var result = await songRepository.GetByIdAsync(id);
            if (result.StatusCode == StatusCodes.Status200OK)
            {
                response.Headers["Accept-Ranges"] = "bytes";
                response.Headers["Content-Length"] = (1023 + 1).ToString();
                response.Headers["Content-Range"] = $"bytes 0-1023/{result.Song.Length}";
                return Results.Stream(result.Song, "audio/mpeg");
            }
            else if (result.StatusCode == StatusCodes.Status404NotFound)
            {
                return Results.StatusCode(StatusCodes.Status404NotFound);
            }

            return Results.StatusCode(StatusCodes.Status404NotFound);

        });


        return group;

    }
}
