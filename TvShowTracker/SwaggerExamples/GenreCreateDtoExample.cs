using Swashbuckle.AspNetCore.Filters;
using TvShowTracker.DTOs;

namespace TvShowTracker.SwaggerExamples
{
    public class GenreCreateDtoExample : IExamplesProvider<GenreCreateDto>
    {
        public GenreCreateDto GetExamples()
        {
            return new GenreCreateDto
            {
                Name = "Ficção Científica"
            };
        }
    }
}
