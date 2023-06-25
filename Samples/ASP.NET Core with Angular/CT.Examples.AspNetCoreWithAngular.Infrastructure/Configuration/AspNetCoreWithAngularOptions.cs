namespace CT.Examples.AspNetCoreWithAngular.Infrastructure.Configuration;

internal class AspNetCoreWithAngularOptions
{
    public string? Server { get; set; }
    public string? Database { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }

    public void Validate()
    {
        ArgumentNullException.ThrowIfNull(nameof(Server));
        ArgumentNullException.ThrowIfNull(nameof(Database));
        ArgumentNullException.ThrowIfNull(nameof(Username));
        ArgumentNullException.ThrowIfNull(nameof(Password));
    }
}
