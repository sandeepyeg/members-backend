using EnterpriseMembers.Application.Features.Members.Commands;
using FluentValidation;

namespace EnterpriseMembers.Application.Validators;

public class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Invalid member ID");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3).WithMessage("Name must be at least 3 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be valid");

        RuleFor(x => x.MembershipType)
            .NotEmpty().WithMessage("Membership type is required")
            .Must(x => x.Equals("Basic", StringComparison.OrdinalIgnoreCase) ||
                      x.Equals("Premium", StringComparison.OrdinalIgnoreCase))
            .WithMessage("Membership type must be Basic or Premium");

        RuleFor(x => x.ExpiryDate)
            .NotEmpty().WithMessage("Expiry date is required");
    }
}
