using FluentValidation;

namespace LBHAddressesAPI.UseCases.V1.Search.Models
{
    public class SearchAddressRequestValidator : AbstractValidator<SearchAddressRequest>
    {
        public SearchAddressRequestValidator()
        {
            RuleFor(x => x).NotNull();
            RuleFor(x => x.addressID).NotNull().NotEmpty().WithMessage("addressID must be provided");
            RuleFor(x => x.addressID).Length(14).WithMessage("addressID must be 14 characters");

            //if (request.addressID == null || string.IsNullOrEmpty(request.addressID))
            //{
            //    throw new Exception("lpi_key must be provided");
            //}
            //else if (request.addressID.Length != 14)
            //{
            //    throw new Exception("lpi_key must be 14 characters");
            //}


            //RuleFor(x => x.addressID).NotNull().NotEmpty().When(m => string.IsNullOrEmpty(m.TenancyRef) && string.IsNullOrEmpty(m.LastName) && string.IsNullOrEmpty(m.Address) && string.IsNullOrEmpty(m.PostCode)).WithMessage("Please enter a search term into FirstName");
        }
    }
}