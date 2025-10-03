using Swashbuckle.AspNetCore.Filters;
namespace TvShowTracker.SwaggerExamples;
public class EpisodeDtoExample : IExamplesProvider<EpisodeDto>
{
    public EpisodeDto GetExamples() => new EpisodeDto
    {
        Title = "Chamados",
        SeasonNumber = 1,
        EpisodeNumber = 2,
        AirDate = new DateTime(2020, 1, 8)
    };
}
