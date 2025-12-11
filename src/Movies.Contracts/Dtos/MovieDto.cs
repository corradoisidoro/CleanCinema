namespace Movies.Contracts.Dtos;

public record MovieDto (int Id, string Title, string Description, DateTime CreatedDate, string Category);