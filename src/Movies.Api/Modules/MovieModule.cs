using MediatR;
using Movies.Application.Commands.Movies.CreateMovie;
using Movies.Application.Commands.Movies.DeleteMovie;
using Movies.Application.Commands.Movies.UpdateMovie;
using Movies.Application.Queries.Movies.GetMovieById;
using Movies.Application.Queries.Movies.GetMovies;
using Movies.Contracts.Requests.Movies;

namespace Movies.Api.Modules;

public static class MovieModule
{
    public static void AddMoviesEndpoints(this IEndpointRouteBuilder app)
    {
        const string IntPattern = @"^-?(?:0|[1-9]\d*)$";
        
        app.MapGet("/api/movies", async (IMediator mediator, CancellationToken ct) =>
        {
            var movies = await mediator.Send(new GetMoviesQuery(), ct);
            return Results.Ok(movies);
        }).WithTags("Movies");
        
        app.MapGet("/api/movies/{id}", async (IMediator mediator, string id, CancellationToken ct) =>
        {
            var movie = await mediator.Send(new GetMovieByIdQuery(int.Parse(id)), ct);
            return Results.Ok(movie);
        }).WithTags("Movies");
        
        app.MapPost("/api/movies", async (IMediator mediator, CreateMovieRequest createMovieRequest, CancellationToken ct) =>
        {
            var command = new CreateMovieCommand(createMovieRequest.Title, createMovieRequest.Description, createMovieRequest.Category);
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("Movies");
        
        app.MapPut("/api/movies/{id}", async (IMediator mediator, string id, UpdateMovieRequest updateMovieRequest, CancellationToken ct) =>
        {
            var command = new UpdateMovieCommand(int.Parse(id) , updateMovieRequest.Title, updateMovieRequest.Description, updateMovieRequest.Category);
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("Movies");
        
        app.MapDelete("/api/movies/{id}", async (IMediator mediator, string id, CancellationToken ct) =>
        {
            var command = new DeleteMovieCommand(int.Parse(id));
            var result = await mediator.Send(command, ct);
            return Results.Ok(result);
        }).WithTags("Movies");
    }
}   
