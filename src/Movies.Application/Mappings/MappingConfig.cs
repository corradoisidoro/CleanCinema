using Mapster;
using Movies.Contracts.Dtos;
using Movies.Contracts.Responses;
using Movies.Domain.Entities;

namespace Movies.Application.Mappings;

public static class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig.GlobalSettings
            .NewConfig<List<Movie>, GetMoviesResponse>()
            .Map(dest => dest.MovieDtos, src => src);

        TypeAdapterConfig.GlobalSettings
            .NewConfig<Movie, GetMovieByIdResponse>()
            .Map(dest => dest.MovieDto, src => src);

        TypeAdapterConfig.GlobalSettings
            .NewConfig<Movie, MovieDto>()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Category, src => src.Category)
            .Map(dest => dest.CreatedDate, src => src.CreatedDate);
    }
}
