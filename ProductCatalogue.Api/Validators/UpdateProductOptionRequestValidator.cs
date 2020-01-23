using FluentValidation;
using ProductCatalogue.Api.ViewModels.Requests;

namespace ProductCatalogue.Api.Validators
{
    public class UpdateProductOptionRequestValidator : AbstractValidator<UpdateProductOptionRequest>
    {
        public UpdateProductOptionRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(9);

            RuleFor(x => x.Description)
                .MaximumLength(23);
        }
    }
}
