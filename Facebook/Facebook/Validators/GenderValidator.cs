using FaceBook.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class GenderValidator : AbstractValidator<Gender>
    {
        public GenderValidator()
        {
            RuleFor(x => x.GenderName).MustnotBeEmpty().MustnotStartWithWhiteSpace().PropLengthRange();
            //RuleFor(x => x.CreatedAt).NotNull().WithMessage("Date is required.");

            RuleForEach(c => c.Users).NotEmpty();

        }
    }
}
