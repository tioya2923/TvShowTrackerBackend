using Swashbuckle.AspNetCore.Filters;
using TvShowTracker.DTOs;


public class TvShowDetailedDtoExample : IExamplesProvider<TvShowDetailedDto>
{
    public TvShowDetailedDto GetExamples()
    {
        return new TvShowDetailedDto
        {
            Id = 1,
            Title = "The Chronicles of Light",
            Description = "A gripping saga of hope and redemption across parallel worlds.",
            ReleaseDate = new DateTime(2023, 5, 12),
            Episodes = new List<EpisodeDto>
            {
                new EpisodeDto
                {
                    Id = 101,
                    Title = "Awakening",
                    SeasonNumber = 1,
                    EpisodeNumber = 1,
                    AirDate = new DateTime(2023, 5, 12),
                    TvShowId = 1
                },
                new EpisodeDto
                {
                    Id = 102,
                    Title = "Echoes of the Past",
                    SeasonNumber = 1,
                    EpisodeNumber = 2,
                    AirDate = new DateTime(2023, 5, 19),
                    TvShowId = 1
                }
            },
            Genres = new List<GenreDto>
            {
                new GenreDto { Id = 10, Name = "Fantasy" },
                new GenreDto { Id = 11, Name = "Drama" }
            },
            Actors = new List<ActorDto>
            {
                new ActorDto { Id = 201, Name = "Elena Varela", BirthDate = new DateTime(1990, 3, 15) },
                new ActorDto { Id = 202, Name = "Miguel Duarte", BirthDate = new DateTime(1985, 7, 22) }
            },
            Favorites = new List<FavoriteDto>
            {
                new FavoriteDto { Id = 301, UserId = 501, TvShowId = 1 }
            }
        };
    }
}
