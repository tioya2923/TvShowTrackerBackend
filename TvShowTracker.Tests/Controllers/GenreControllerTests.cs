using Xunit;
using Moq;
using TvShowTracker.Controllers;
using TvShowTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class GenreControllerTests
{
    [Fact]
    public void GetGenres_ReturnsOkWithGenres()
    {
        // Simulação do serviço
        var mockService = new Mock<IGenreService>();
        mockService.Setup(s => s.GetGenres()).Returns(new List<string> { "Drama", "Comédia" });

        // Injeta o mock no controlador
        var controller = new GenreController(mockService.Object);

        // Executa o método
        var result = controller.GetGenres();

        // Verifica o resultado
        var okResult = Assert.IsType<OkObjectResult>(result);
        var genres = Assert.IsType<List<string>>(okResult.Value);
        Assert.Contains("Drama", genres);
        Assert.Contains("Comédia", genres);
    }
}
