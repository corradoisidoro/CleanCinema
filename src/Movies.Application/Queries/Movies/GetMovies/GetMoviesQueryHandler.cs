using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.Contracts.Responses;
using Movies.Infrastructure;

namespace Movies.Application.Queries.Movies.GetMovies;

public class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, GetMoviesResponse>
{
    private readonly MoviesDbContext _moviesDbContext;

    public GetMoviesQueryHandler(MoviesDbContext moviesDbContext)
    {
        _moviesDbContext = moviesDbContext;
    }
    
    public async Task<GetMoviesResponse> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await _moviesDbContext.Movies.ToListAsync(cancellationToken);
        return movies.Adapt<GetMoviesResponse>();
    }
}