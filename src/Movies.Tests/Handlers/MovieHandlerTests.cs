using Microsoft.EntityFrameworkCore;
using Movies.Application.Commands.Movies.CreateMovie;
using Movies.Application.Commands.Movies.DeleteMovie;
using Movies.Application.Commands.Movies.UpdateMovie;
using Movies.Application.Queries.Movies.GetMovieById;
using Movies.Application.Queries.Movies.GetMovies;
using Movies.Contracts.Exceptions;
using Movies.Domain.Entities;

namespace Movies.Tests.Handlers;

[Collection(AppCollection.Name)]
public class MovieHandlerTests
{
    private static Movie SeedMovie(Movies.Infrastructure.MoviesDbContext db, string title = "Blade Runner")
    {
        var movie = new Movie
        {
            Title = title,
            Description = "A blade runner must pursue replicants.",
            Category = "Sci-Fi",
            CreatedDate = DateTime.UtcNow
        };
        db.Movies.Add(movie);
        db.SaveChanges();
        return movie;
    }

    // ---------------------------------------------------------------- Create

    [Fact]
    public async Task CreateMovie_PersistsAndReturnsNewId()
    {
        using var db = new TestDatabase();
        var handler = new CreateMovieCommandHandler(db.Context);

        var id = await handler.Handle(
            new CreateMovieCommand("Arrival", "Linguists meet alien life.", "Sci-Fi"),
            CancellationToken.None);

        Assert.True(id > 0);
        var saved = await db.Context.Movies.SingleAsync();
        Assert.Equal("Arrival", saved.Title);
        Assert.Equal("Linguists meet alien life.", saved.Description);
        Assert.Equal("Sci-Fi", saved.Category);
    }

    [Fact]
    public async Task CreateMovie_StampsCreatedDateAsUtc()
    {
        using var db = new TestDatabase();
        var handler = new CreateMovieCommandHandler(db.Context);
        var before = DateTime.UtcNow;

        await handler.Handle(
            new CreateMovieCommand("Title", "Description", "Category"),
            CancellationToken.None);

        var saved = await db.Context.Movies.SingleAsync();
        Assert.InRange(saved.CreatedDate, before.AddSeconds(-1), DateTime.UtcNow.AddSeconds(1));
        Assert.Equal(DateTimeKind.Utc, saved.CreatedDate.Kind);
    }

    // ---------------------------------------------------------------- Read

    [Fact]
    public async Task GetMovies_ReturnsAllSeededMovies()
    {
        using var db = new TestDatabase();
        SeedMovie(db.Context, "Blade Runner");
        SeedMovie(db.Context, "Arrival");
        var handler = new GetMoviesQueryHandler(db.Context);

        var response = await handler.Handle(new GetMoviesQuery(), CancellationToken.None);

        Assert.Equal(2, response.MovieDtos.Count);
        Assert.Contains(response.MovieDtos, m => m.Title == "Blade Runner");
        Assert.Contains(response.MovieDtos, m => m.Title == "Arrival");
    }

    [Fact]
    public async Task GetMovies_WithNoData_ReturnsEmptyList()
    {
        using var db = new TestDatabase();
        var handler = new GetMoviesQueryHandler(db.Context);

        var response = await handler.Handle(new GetMoviesQuery(), CancellationToken.None);

        Assert.Empty(response.MovieDtos);
    }

    [Fact]
    public async Task GetMovieById_ReturnsRequestedMovie()
    {
        using var db = new TestDatabase();
        var seeded = SeedMovie(db.Context, "The Thing");
        var handler = new GetMovieByIdQueryHandler(db.Context);

        var response = await handler.Handle(new GetMovieByIdQuery(seeded.Id), CancellationToken.None);

        Assert.Equal(seeded.Id, response.MovieDto.Id);
        Assert.Equal("The Thing", response.MovieDto.Title);
        Assert.Equal("Sci-Fi", response.MovieDto.Category);
    }

    [Fact]
    public async Task GetMovieById_WhenMissing_ThrowsNotFound()
    {
        using var db = new TestDatabase();
        var handler = new GetMovieByIdQueryHandler(db.Context);

        var ex = await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new GetMovieByIdQuery(999), CancellationToken.None));

        Assert.Contains("999", ex.Message);
    }

    // ---------------------------------------------------------------- Update

    [Fact]
    public async Task UpdateMovie_PersistsChanges()
    {
        using var db = new TestDatabase();
        var seeded = SeedMovie(db.Context, "Old Title");
        var handler = new UpdateMovieCommandHandler(db.Context);

        await handler.Handle(
            new UpdateMovieCommand(seeded.Id, "New Title", "New description.", "Drama"),
            CancellationToken.None);

        var saved = await db.Context.Movies.SingleAsync(m => m.Id == seeded.Id);
        Assert.Equal("New Title", saved.Title);
        Assert.Equal("New description.", saved.Description);
        Assert.Equal("Drama", saved.Category);
    }

    [Fact]
    public async Task UpdateMovie_WhenMissing_ThrowsNotFound()
    {
        using var db = new TestDatabase();
        var handler = new UpdateMovieCommandHandler(db.Context);

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(
                new UpdateMovieCommand(999, "Title", "Description", "Category"),
                CancellationToken.None));
    }

    // ---------------------------------------------------------------- Delete

    [Fact]
    public async Task DeleteMovie_RemovesTheRow()
    {
        using var db = new TestDatabase();
        var seeded = SeedMovie(db.Context);
        var handler = new DeleteMovieCommandHandler(db.Context);

        await handler.Handle(new DeleteMovieCommand(seeded.Id), CancellationToken.None);

        Assert.Empty(await db.Context.Movies.ToListAsync());
    }

    [Fact]
    public async Task DeleteMovie_WhenMissing_ThrowsNotFound()
    {
        using var db = new TestDatabase();
        var handler = new DeleteMovieCommandHandler(db.Context);

        await Assert.ThrowsAsync<NotFoundException>(
            () => handler.Handle(new DeleteMovieCommand(999), CancellationToken.None));
    }
}
