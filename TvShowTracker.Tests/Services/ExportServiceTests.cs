using Xunit;
using TvShowTracker.Services;
using TvShowTracker.Data;
using TvShowTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class ExportServiceTests
{
    [Fact]
    public void ExportUserDataAsCsv_ReturnsCorrectFormat()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("ExportTestDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

        var userId = 1;
        context.Users.Add(new User
        {
            Id = userId,
            Email = "teste@exemplo.pt",
            Username = "joao",
            PasswordHash = "hash123"
        });

        context.Favorites.AddRange(new List<Favorite>
        {
           new Favorite { UserId = userId, TvShow = new TvShow { Title = "Breaking Bad", Description = "Drama sobre um professor de química que vira traficante." } },
            new Favorite { UserId = userId, TvShow = new TvShow { Title = "The Crown", Description = "Série sobre o reinado da Rainha Elizabeth II." } }

        });

        context.SaveChanges();

        var service = new ExportService(context);

        // Act
        var csv = service.ExportUserDataAsCsv(userId);

        // Assert
        Assert.Contains("UserId,Email,Favorites", csv);
        Assert.Contains("Breaking Bad", csv);
        Assert.Contains("The Crown", csv);
    }
}
