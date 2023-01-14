namespace CT.Examples.SimplifiedCA.Entities;

public class WeatherForecast
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public DateTimeOffset Time { get; set; }
    public double Temperature { get; set; }
    public string Unit { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }

    public Account Account { get; set; }
}
