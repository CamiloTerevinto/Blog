namespace CT.Examples.SimplifiedCA.Infrastructure.Configuration;

internal class WeatherApiOptions
{
    public string BaseUrl { get; set; }
    public string Token { get; set; }

    public void Validate()
    {
        ArgumentNullException.ThrowIfNull(nameof(BaseUrl));
        ArgumentNullException.ThrowIfNull(nameof(Token));
    }
}
