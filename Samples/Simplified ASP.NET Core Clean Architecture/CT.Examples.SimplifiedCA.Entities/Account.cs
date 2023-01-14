namespace CT.Examples.SimplifiedCA.Entities;

public class Account
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public ICollection<WeatherForecast> WeatherForecasts { get; set; }
    public bool Enabled { get; set; }

    public Account()
    {
        WeatherForecasts= new HashSet<WeatherForecast>();
    }
}
