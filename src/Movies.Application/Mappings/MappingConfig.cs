using Mapster;
using Movies.Contracts.Responses;
using Movies.Domain.Entities;

namespace Movies.Application.Mappings;

public class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<List<Movie>, GetMoviesResponse>.NewConfig()
            .Map(dest => dest.MovieDtos, src => src);
        
        TypeAdapterConfig<Movie, GetMovieByIdResponse>.NewConfig()
            .Map(dest => dest.MovieDto, src => src);
    }
}