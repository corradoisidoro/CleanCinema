namespace Movies.Domain.Entities;

public class Movie : BaseEntity
{
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Category { get; set; }
}