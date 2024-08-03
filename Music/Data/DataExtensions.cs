using Microsoft.EntityFrameworkCore;
using Music.Respositories;
using System.Runtime.CompilerServices;

namespace Music.Data;

public static class DataExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        var connString = configuration.GetConnectionString("AuthDbContextConnection");
        services.AddDbContext<MusicApplicationContext>(options => options.UseNpgsql(connString))
            .AddScoped<IStorage, StorageRepository>()
            .AddScoped<ISong, SongRepository>()
            .AddScoped<IAudioProcessor, AudioProcessorRepository>();
            
        return services;
    }
}
