namespace RedFox.Domain.ValueObjects;

public record Address(
    string Street,
    string Suite,
    string City,
    string Zipcode,
    Geolocation Geo);