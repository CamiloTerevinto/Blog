namespace CT.Examples.AspNetCoreWithAngular.Entities;

public enum WebRole
{
    User = 0,
    Admin = 1,
}

public class Account
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public WebRole Role { get; set; }
    public bool Enabled { get; set; }
}
