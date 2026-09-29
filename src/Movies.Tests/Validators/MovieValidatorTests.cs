using FluentValidation;
using Movies.Application.Commands.Movies.CreateMovie;
using Movies.Application.Commands.Movies.DeleteMovie;
using Movies.Application.Commands.Movies.UpdateMovie;
using Movies.Application.Queries.Movies.GetMovieById;

namespace Movies.Tests.Validators;

public class MovieValidatorTests
{
    // ---------------------------------------------------------------- CreateMovie

    [Fact]
    public void CreateMovie_WithValidCommand_Passes()
    {
        var result = new CreateMovieCommandValidator()
            .Validate(new CreateMovieCommand("Blade Runner", "Replicants hunt a runaway.", "Sci-Fi"));

        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateMovie_WithBlankTitle_Fails(string title)
    {
        var result = new CreateMovieCommandValidator()
            .Validate(new CreateMovieCommand(title, "A description.", "Sci-Fi"));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateMovieCommand.Title));
    }

    [Fact]
    public void CreateMovie_WithTitleOver100Chars_Fails()
    {
        var result = new CreateMovieCommandValidator()
            .Validate(new CreateMovieCommand(new string('a', 101), "A description.", "Sci-Fi"));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateMovieCommand.Title));
    }

    [Fact]
    public void CreateMovie_WithCategoryOver30Chars_Fails()
    {
        var result = new CreateMovieCommandValidator()
            .Validate(new CreateMovieCommand("Title", "A description.", new string('c', 31)));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateMovieCommand.Category));
    }

    [Fact]
    public void CreateMovie_WithDescriptionOver1000Chars_Fails()
    {
        var result = new CreateMovieCommandValidator()
            .Validate(new CreateMovieCommand("Title", new string('d', 1001), "Sci-Fi"));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(CreateMovieCommand.Description));
    }

    [Fact]
    public void CreateMovie_ReportsEveryInvalidField()
    {
        var result = new CreateMovieCommandValidator()
            .Validate(new CreateMovieCommand("", "", ""));

        Assert.Equal(3, result.Errors.Count);
    }

    // ---------------------------------------------------------------- UpdateMovie

    [Fact]
    public void UpdateMovie_WithValidCommand_Passes()
    {
        var result = new UpdateMovieCommandValidator()
            .Validate(new UpdateMovieCommand(1, "Title", "Description", "Category"));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void UpdateMovie_WithIdZero_Fails()
    {
        var result = new UpdateMovieCommandValidator()
            .Validate(new UpdateMovieCommand(0, "Title", "Description", "Category"));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateMovieCommand.Id));
    }

    [Fact]
    public void UpdateMovie_WithBlankTitle_Fails()
    {
        var result = new UpdateMovieCommandValidator()
            .Validate(new UpdateMovieCommand(1, "", "Description", "Category"));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(UpdateMovieCommand.Title));
    }

    // ---------------------------------------------------------------- DeleteMovie

    [Fact]
    public void DeleteMovie_WithValidId_Passes()
    {
        var result = new DeleteMovieCommandValidator().Validate(new DeleteMovieCommand(1));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void DeleteMovie_WithIdZero_Fails()
    {
        var result = new DeleteMovieCommandValidator().Validate(new DeleteMovieCommand(0));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(DeleteMovieCommand.Id));
    }

    // ---------------------------------------------------------------- GetMovieById

    [Fact]
    public void GetMovieById_WithValidId_Passes()
    {
        var result = new GetMovieByIdQueryValidator().Validate(new GetMovieByIdQuery(1));

        Assert.True(result.IsValid);
    }

    [Fact]
    public void GetMovieById_WithIdZero_Fails()
    {
        var result = new GetMovieByIdQueryValidator().Validate(new GetMovieByIdQuery(0));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == nameof(GetMovieByIdQuery.Id));
    }
}
