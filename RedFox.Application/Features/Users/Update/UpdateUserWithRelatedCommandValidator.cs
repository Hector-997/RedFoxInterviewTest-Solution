using FluentValidation;

namespace RedFox.Application.Features.Users.Update;

public class UpdateUserWithRelatedCommandValidator : AbstractValidator<UpdateUserWithRelatedCommand>
{
    public UpdateUserWithRelatedCommandValidator()
    {
        RuleFor(x => x.Payload.Name)
            .NotEmpty().WithMessage("Name is required");

        RuleFor(x => x.Payload.Username)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(x => x.Payload.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");

        RuleFor(x => x.Payload.Phone)
            .NotEmpty().WithMessage("Phone is required");

        RuleFor(x => x.Payload.Website)
            .NotEmpty().WithMessage("Website is required")
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.RelativeOrAbsolute))
            .WithMessage("Website must be a valid URL");
    }
}
