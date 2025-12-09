using FluentValidation;
using Movies.Domain.Entities;

namespace Movies.Application.Commands.Movies.CreateMovie;

public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage($"The {nameof(Movie.Title)} cannot be empty")
            .MaximumLength(100)
            .WithMessage($"The {nameof(Movie.Title)} cannot be longer than 100 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage($"The {nameof(Movie.Description)} cannot be empty")
            .MaximumLength(1000)
            .WithMessage($"The {nameof(Movie.Description)} cannot be longer than 1000 characters");
        
        RuleFor(x => x.Category)
            .NotEmpty()
            .WithMessage($"The {nameof(Movie.Category)} cannot be empty")
            .MaximumLength(30)
            .WithMessage($"The {nameof(Movie.Category)} cannot be longer than 30 characters");
    }
}