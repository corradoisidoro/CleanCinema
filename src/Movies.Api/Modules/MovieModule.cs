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
        app.MapGet("/api/movies", async (IMediator mediator, CancellationToken ct) =>
        {
            var movies = await mediator.Send(new GetMoviesQuery(), ct);
            return Results.Ok(movies);
        }).WithTags("Movies");

        app.MapGet("/api/movies/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
        {
            var movie = await mediator.Send(new GetMovieByIdQuery(id), ct);
            return Results.Ok(movie);
        }).WithTags("Movies");

        app.MapPost("/api/movies", async (IMediator mediator, CreateMovieRequest createMovieRequest, CancellationToken ct) =>
        {
            var command = new CreateMovieCommand(createMovieRequest.Title, createMovieRequest.Description, createMovieRequest.Category);
            var id = await mediator.Send(command, ct);
            return Results.Created($"/api/movies/{id}", id);
        }).WithTags("Movies");

        app.MapPut("/api/movies/{id:int}", async (IMediator mediator, int id, UpdateMovieRequest updateMovieRequest, CancellationToken ct) =>
        {
            var command = new UpdateMovieCommand(id, updateMovieRequest.Title, updateMovieRequest.Description, updateMovieRequest.Category);
            await mediator.Send(command, ct);
            return Results.NoContent();
        }).WithTags("Movies");

        app.MapDelete("/api/movies/{id:int}", async (IMediator mediator, int id, CancellationToken ct) =>
        {
            var command = new DeleteMovieCommand(id);
            await mediator.Send(command, ct);
            return Results.NoContent();
        }).WithTags("Movies");
    }
}
