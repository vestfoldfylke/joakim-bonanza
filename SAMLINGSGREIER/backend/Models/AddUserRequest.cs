using System.ComponentModel.DataAnnotations;

namespace Backend.Models;

public record AddUserRequest(
    [Required(ErrorMessage = "Brukernavn må fylles ut")] string Username,
    [Required] string FirstName,
    [Required] string LastName,
    Guid GroupId,
    [Required, MinLength(10, ErrorMessage = "Passordet må ha minst 10 tegn")] string Password
);
