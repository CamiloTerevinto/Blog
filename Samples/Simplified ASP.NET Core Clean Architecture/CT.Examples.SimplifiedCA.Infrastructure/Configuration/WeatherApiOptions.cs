namespace CT.Examples.SimplifiedCA.Infrastructure.Configuration;

internal class WeatherApiOptions
{
    public string BaseUrl { get; set; } = default!;
    public string Token { get; set; } = default!;

    public void Validate()
    {
        ArgumentNullException.ThrowIfNull(nameof(BaseUrl));
        ArgumentNullException.ThrowIfNull(nameof(Token));
    }
}
