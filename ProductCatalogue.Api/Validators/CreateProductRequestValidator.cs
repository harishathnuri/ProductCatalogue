using FluentValidation;
using ProductCatalogue.Api.ViewModels.Requests;

namespace ProductCatalogue.Api.Validators
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(17);

            RuleFor(x => x.Description)
                .MaximumLength(35);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.DeliveryPrice)
                .GreaterThan(0);
        }
    }
}
