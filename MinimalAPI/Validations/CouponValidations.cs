using FluentValidation;
using MinimalAPI.Models.DTO;

namespace MinimalAPI.Validations
{
    public class CouponCreateValidation: AbstractValidator<CouponCreateDto>
    {
        public CouponCreateValidation()
        {
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1,100);
        }
    }

    public class CouponUpdateValidation : AbstractValidator<CouponUpdateDto>
    {
        public CouponUpdateValidation()
        {
            RuleFor(model => model.Id).NotEmpty().GreaterThan(0);
            RuleFor(model => model.Name).NotEmpty();
            RuleFor(model => model.Percent).InclusiveBetween(1, 100);
        }
    }
}
