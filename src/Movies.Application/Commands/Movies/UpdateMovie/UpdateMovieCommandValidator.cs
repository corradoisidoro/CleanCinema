using FluentValidation;
using Movies.Domain.Entities;

namespace Movies.Application.Commands.Movies.UpdateMovie;

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage($"The {nameof(Movie.Id)} cannot be empty");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage($"The {nameof(Movie.Title)} cannot be empty")
            .MaximumLength(100).WithMessage($"The {nameof(Movie.Title)} cannot be longer than 100 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage($"The {nameof(Movie.Description)} cannot be empty")
            .MaximumLength(1000).WithMessage($"The {nameof(Movie.Description)} cannot be longer than 1000 characters");
        
        RuleFor(x => x.Category)
            .NotEmpty().WithMessage($"The {nameof(Movie.Category)} cannot be empty")
            .MaximumLength(30).WithMessage($"The {nameof(Movie.Category)} cannot be longer than 30 characters");
    }
}