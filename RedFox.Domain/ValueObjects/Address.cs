namespace RedFox.Domain.ValueObjects;

public record Address
{
    public required string Street { get; init; }
    public required string Suite { get; init; }
    public required string City { get; init; }
    public required string Zipcode { get; init; }
    public required Geolocation Geo { get; init; }
}