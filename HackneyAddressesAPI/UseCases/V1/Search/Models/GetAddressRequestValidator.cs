using FluentValidation;

namespace LBHAddressesAPI.UseCases.V1.Search.Models
{
    public class GetAddressRequestValidator : AbstractValidator<GetAddressRequest>
    {
        public GetAddressRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.addressID).NotNull().NotEmpty().WithMessage("addressID must be provided");
            RuleFor(x => x.addressID).Length(14).WithMessage("addressID must be 14 characters");

        }
    }
}