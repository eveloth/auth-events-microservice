using AuthEvents.Contracts.Requests;
using FluentValidation;

namespace AuthEvents.Validation;

public class EventValidator : AbstractValidator<EventRequest>
{
    public EventValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().Must(IsValidGuid);
        RuleFor(x => x.EventType).IsInEnum();
        RuleFor(x => x.TimeFired)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow)
            .GreaterThan(DateTime.MinValue);
        RuleFor(x => x.Payload).NotEmpty();
    }

    private static bool IsValidGuid(Guid guid)
    {
        return !guid.Equals(Guid.Empty);
    }
}