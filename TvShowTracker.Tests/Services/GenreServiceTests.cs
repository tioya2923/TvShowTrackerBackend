using Xunit;
using TvShowTracker.Services;
using TvShowTracker.Data;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;



public class GenreServiceTests
{
    [Fact]
    public void GetGenres_ReturnsCachedGenres_WhenAvailable()
    {
        // Arrange: cria contexto em memória e cache
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDb")
            .Options;

        var context = new ApplicationDbContext(options);
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        var service = new GenreService(context, memoryCache);
        var expectedGenres = new List<string> { "Drama", "Comédia" };

        memoryCache.Set("genres_list", expectedGenres);

        // Act
        var result = service.GetGenres();

        // Assert
        Assert.Equal(expectedGenres, result);
    }
}
