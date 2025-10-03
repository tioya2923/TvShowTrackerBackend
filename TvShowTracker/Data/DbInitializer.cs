using TvShowTracker.Data;
using TvShowTracker.Models;

namespace TvShowTracker.Data
{
    public static class DbInitializer
    {
        public static async Task SeedMassiveAsync(ApplicationDbContext context)
        {
            if (context.TvShows.Any()) return;

            var genres = new[] { "Drama", "Comédia", "Ação", "Mistério", "Histórico" }
                .Select(name => new Genre { Name = name }).ToList();

            var actors = Enumerable.Range(1, 100).Select(i => new Actor
            {
                Name = $"Ator {i}",
                BirthDate = DateTime.UtcNow.AddYears(-20 - i),
                Biography = $"Biografia do Ator {i}",
                PhotoUrl = $"https://picsum.photos/seed/ator{i}/200/300"
            }).ToList();

            var tvShows = Enumerable.Range(1, 50).Select(i => new TvShow
            {
                Title = $"Série {i}",
                Description = $"Descrição da Série {i}",
                ReleaseDate = DateTime.UtcNow.AddYears(-i),
                ImageUrl = $"https://picsum.photos/seed/serie{i}/300/400",
                VideoUrl = $"https://www.w3schools.com/html/mov_bbb.mp4",
                CreatedAt = DateTime.UtcNow,
                Genres = genres.OrderBy(_ => Guid.NewGuid()).Take(2).ToList(),
                Actors = actors.OrderBy(_ => Guid.NewGuid()).Take(5).ToList(),

                //  Aqui colocas o bloco dos episódios
                Episodes = Enumerable.Range(1, 10).Select(j => new Episode
                {
                    Title = $"Série {i} — Episódio {j}",
                    SeasonNumber = (j - 1) / 5 + 1,
                    EpisodeNumber = j,
                    AirDate = DateTime.UtcNow.AddDays(-j - i * 10),
                    VideoUrl = $"https://www.w3schools.com/html/mov_bbb.mp4",
                    ImageUrl = $"https://picsum.photos/seed/serie{i}-ep{j}/300/200",
                    Description = $"Este é o episódio {j} da série {i}, com conteúdo exclusivo e reviravoltas inesperadas."
                }).ToList()
            }).ToList();


            await context.Genres.AddRangeAsync(genres);
            await context.Actors.AddRangeAsync(actors);
            await context.TvShows.AddRangeAsync(tvShows);
            await context.SaveChangesAsync();
        }
    }
}
