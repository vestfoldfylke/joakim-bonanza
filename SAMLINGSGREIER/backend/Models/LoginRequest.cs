using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public record LoginRequest(
    [Required] string Username,
    [Required] string Password
);
