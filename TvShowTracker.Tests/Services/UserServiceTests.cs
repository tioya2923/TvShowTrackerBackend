using Xunit;
using TvShowTracker.Services;
using TvShowTracker.Data;
using TvShowTracker.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class UserServiceTests
{
    [Fact]
    public void DeleteUserData_RemovesUserAndFavorites()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("UserTestDb")
            .EnableSensitiveDataLogging()
            .Options;

        using var context = new ApplicationDbContext(options);

        var userId = 1;
        context.Users.Add(new User
        {
            Id = userId,
            Email = "joao@example.com",
            Username = "joao",
            PasswordHash = "hash123"
        });

        context.Favorites.Add(new Favorite
        {
            UserId = userId,
           TvShow = new TvShow { Title = "Lost", Description = "Série sobre sobreviventes de um acidente aéreo numa ilha misteriosa." }
        });

        context.SaveChanges();

        var service = new UserService(context);

        // Act
        var result = service.DeleteUserData(userId);

        // Assert
        Assert.True(result);
        Assert.Null(context.Users.Find(userId));
        Assert.Empty(context.Favorites.Where(f => f.UserId == userId));
    }
}
