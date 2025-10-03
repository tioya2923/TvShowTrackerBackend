using TvShowTracker.DTOs;

namespace TvShowTracker.SwaggerExamples
{
    public class ActorDtoExample
    {
        public static ActorDto GetExample()
        {
            return new ActorDto
            {
                Id = 1,
                Name = "Leonardo Almeida",
                BirthDate = new DateTime(1982, 7, 15),
                PhotoUrl = "https://exemplo.com/foto.jpg",
                Biography = "Exemplo de biografia...",
                TvShows = new List<TvShowDto>()
            };
        }
    }
}
