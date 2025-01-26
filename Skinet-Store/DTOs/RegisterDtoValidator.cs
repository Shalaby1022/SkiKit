using FluentValidation;

namespace Skinet_Store.DTOs
{
	public class RegisterDtoValidator : AbstractValidator<RegisterDto>
	{
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Email)
                   .NotEmpty()
                   .WithMessage("Email is required");

            RuleFor(x => x.Password)
				.NotEmpty()
				   .WithMessage("Password is required")
				   .MinimumLength(8)
				   .WithMessage("Password must be at least 8 characters");

			RuleFor(x => x.FirstName)
				.NotEmpty().WithMessage("Nationality is required")
			.MinimumLength(2).WithMessage("Nationality must be at least 2 characters")
			.MaximumLength(50).WithMessage("Nationality cannot exceed 50 characters");




		}
    }
}
