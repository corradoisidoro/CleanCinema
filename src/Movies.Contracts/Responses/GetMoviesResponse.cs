using Movies.Contracts.Dtos;

namespace Movies.Contracts.Responses;

public record GetMoviesResponse(List<MovieDto> MovieDtos);