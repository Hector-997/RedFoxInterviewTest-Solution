using RedFox.Application.DTO;

namespace RedFox.Application.Features.Users.Update;

public record UpdateUserWithRelatedRequest(
    string Name,
    string Username,
    string Email,
    string Phone,
    string Website,
    AddressDto Address,
    CompanyDto Company);
