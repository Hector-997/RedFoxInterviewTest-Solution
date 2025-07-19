namespace RedFox.Domain.ValueObjects;

public record Geolocation
{
    public required string Latitude { get; init; }
    public required string Longitude { get; init; }
}
