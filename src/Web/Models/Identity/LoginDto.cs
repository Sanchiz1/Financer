namespace Web.Models.Identity;

public record LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}