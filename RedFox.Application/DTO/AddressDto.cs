namespace RedFox.Application.DTO;

public record AddressDto(
    string Street,
    string Suite,
    string City,
    string Zipcode,
    GeolocationDto Geo);
