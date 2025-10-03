using Swashbuckle.AspNetCore.Filters;
namespace TvShowTracker.SwaggerExamples;
public class GenreDtoExample : IExamplesProvider<GenreDto>
{
    public GenreDto GetExamples()
    {
        return new GenreDto
        {
            Id = 1,
            Name = "Drama"
        };
    }
}
