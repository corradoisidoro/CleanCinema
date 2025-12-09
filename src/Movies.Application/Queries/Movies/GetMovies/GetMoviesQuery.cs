using MediatR;
using Movies.Contracts.Responses;

namespace Movies.Application.Queries.Movies.GetMovies;

public record GetMoviesQuery () :IRequest<GetMoviesResponse>;