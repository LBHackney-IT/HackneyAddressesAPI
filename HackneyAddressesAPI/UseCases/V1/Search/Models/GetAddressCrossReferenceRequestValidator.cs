using FluentValidation;

namespace LBHAddressesAPI.UseCases.V1.Search.Models
{
    public class GetAddressCrossReferenceRequestValidator : AbstractValidator<GetAddressCrossReferenceRequest>
    {
        public GetAddressCrossReferenceRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.uprn).NotNull().NotEmpty().WithMessage("UPRN must be provided");
        }
    }
}