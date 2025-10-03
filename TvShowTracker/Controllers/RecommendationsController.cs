using Microsoft.AspNetCore.Mvc;
using TvShowTracker.Services;
using TvShowTracker.DTOs;

[ApiController]
[Route("api/[controller]")]
public class RecommendationsController : ControllerBase
{
    private readonly RecommendationService _recommendationService;

    public RecommendationsController(RecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    [HttpGet("{userId}")]
    public IActionResult GetRecommendations(int userId)
    {
        var result = _recommendationService.GetRecommendationsForUser(userId);
        return Ok(result);
    }
    [HttpPost("{userId}/schedule-email")]
public IActionResult ScheduleEmail(int userId, [FromQuery] string email)
{
    _recommendationService.EnqueueRecommendationEmail(userId, email);
    return Ok("Envio de e-mail agendado com sucesso.");
}



}
