using System.ComponentModel.DataAnnotations;

namespace CT.Examples.AspNetCoreWithAngular.Models.Input;

public class SignInDto
{
    [Required][EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
}
