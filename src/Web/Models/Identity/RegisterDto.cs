﻿using Domain.ValueObjects;

namespace Web.Models.Identity;

public record RegisterDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required Currency PreferredCurrency { get; set; }
}
