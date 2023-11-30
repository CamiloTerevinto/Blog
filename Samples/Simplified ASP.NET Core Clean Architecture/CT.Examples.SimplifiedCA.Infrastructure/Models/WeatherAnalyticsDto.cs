namespace CT.Examples.SimplifiedCA.Infrastructure.Models;

public class WeatherAnalyticsDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? TimeOfDay { get; set; }
    public string? Weather { get; set; }
    public double Temperature { get; set; }
    public string? Unit { get; set; }
    public double Range { get; set; }
}
