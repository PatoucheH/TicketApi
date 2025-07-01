using FluentValidation;
using TicketApi.Models.DTOs;


namespace TicketApi.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDTOValidator"/> class,  which provides validation rules for
        /// the <see cref="UserDTO"/> object.
        /// </summary>
        public UserDTOValidator()
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("The name is needed !")
                .MaximumLength(100).WithMessage("The name cannot be longer than 100 characters. ");
            RuleFor(u => u.Email)
                .EmailAddress().WithMessage("The email adress must be valid !");
        }
    }
}
