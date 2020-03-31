using FaceBook.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class RoleValidator: AbstractValidator<Role>
    {
        public RoleValidator()
        {
            RuleFor(x => x.Title).MustnotBeEmpty().MustnotStartWithWhiteSpace();
            RuleFor(x => x.Description).MustnotBeEmpty().MustnotStartWithWhiteSpace();
            //RuleFor(x => x.CreatedBy).MustnotBeEmpty().MustnotStartWithWhiteSpace().PropLengthRange();
            //RuleFor(x => x.UpdatedBy).MustnotBeEmpty().MustnotStartWithWhiteSpace().PropLengthRange();
            //RuleFor(x => x.CreatedAt).NotNull().WithMessage("Date is required.");
            //RuleFor(x => x.UpdatedAt).NotNull().WithMessage("Date is required.");

            RuleForEach(c => c.RoleActions).NotEmpty(); 
            RuleForEach(c => c.Users).NotEmpty();
        }
    }
}
