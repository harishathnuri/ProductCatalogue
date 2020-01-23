using FluentValidation;
using ProductCatalogue.Api.ViewModels.Requests;

namespace ProductCatalogue.Api.Validators
{
    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(17);

            RuleFor(x => x.Description)
                .MaximumLength(35);
        }
    }
}
