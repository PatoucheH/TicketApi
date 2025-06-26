using FluentValidation;
using TicketApi.Models;
using TicketApi.Data;
using Microsoft.EntityFrameworkCore;


namespace TicketApi.Validators
{
    public class TicketDTOValidator : AbstractValidator<TicketCreateDTO>
    {
        private readonly ContextDatabase _context;
        public TicketDTOValidator(ContextDatabase context) 
        {
            _context = context;


            RuleFor(t => t.Title)
                .NotEmpty().WithMessage("The title is needed !")
                .MaximumLength(150).WithMessage("The max length for the title is 150 characters.");

            RuleFor(t => t.UserId)
                .NotEmpty().WithMessage("You need to enter the id of the user you need to join the ticket.");
        }

    }
}
