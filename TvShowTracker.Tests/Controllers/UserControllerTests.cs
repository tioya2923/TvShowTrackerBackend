using Xunit;
using Moq;
using TvShowTracker.Controllers;
using TvShowTracker.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class UserControllerTests
{
    [Fact]
    public void GiveConsent_ReturnsOk_WhenUserExists()
    {
        // Arrange
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.RegisterConsent(1));

        var controller = new UserController(mockService.Object);

        // Act
        var result = controller.GiveConsent(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Consentimento registado com sucesso.", okResult.Value);
    }

    [Fact]
    public void DeleteUser_ReturnsNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var mockService = new Mock<IUserService>();
        mockService.Setup(s => s.DeleteUserData(99)).Returns(false);

        var controller = new UserController(mockService.Object);

        // Act
        var result = controller.DeleteUser(99);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Utilizador com ID 99 não encontrado.", notFoundResult.Value);
    }
}
