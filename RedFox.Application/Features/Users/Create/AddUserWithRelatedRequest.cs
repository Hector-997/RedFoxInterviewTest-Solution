namespace RedFox.Application.DTO;

public record AddUserWithRelatedRequest(
    string Name,
    string Username,
    string Email,
    string Phone,
    string Website,
    AddressDto Address,
    CompanyDto Company);
