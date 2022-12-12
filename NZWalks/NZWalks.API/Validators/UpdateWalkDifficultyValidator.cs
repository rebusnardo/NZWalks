using FluentValidation;

namespace NZWalks.API.Validators
{
    public class UpdateWalkDifficultyValidator: AbstractValidator<Models.DTO.UpdateWalkDifficultyRequest>
    {
        public UpdateWalkDifficultyValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
