using Swashbuckle.AspNetCore.Filters;
namespace TvShowTracker.SwaggerExamples; using TvShowTracker.DTOs;

public class TvShowDtoExample : IExamplesProvider<TvShowDto>
{
    public TvShowDto GetExamples() => new TvShowDto
    {
        Title = "The Chosen",
        Description = "Série sobre a vida de Jesus",
        ReleaseDate = new DateTime(2020, 1, 1),
       
     
    };
}
