namespace Movies.Contracts.Errors;

public class ValidationError
{
    public required string Property {get; set;}
    public required string ErrorMessage {get; set;}
} 