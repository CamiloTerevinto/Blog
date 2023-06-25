using System.ComponentModel.DataAnnotations;

namespace CT.Examples.AspNetCoreWithAngular.Models.Input;

public class SignUpDto
{
    [Required][EmailAddress] public string Email { get; set; }
    [Required] public string Password { get; set; }
    [Required] public string RepeatPassword { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string PhoneNumber { get; set; }
}
