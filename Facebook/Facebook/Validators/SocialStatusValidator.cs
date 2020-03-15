using FaceBook.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class SocialStatusValidator : AbstractValidator<SocialStatus>
    {
        public SocialStatusValidator()
        {
            RuleFor(x => x.Status).MustnotBeEmpty().MustnotStartWithWhiteSpace();
            RuleFor(x => x.IsDeleted).Must(x => x.Equals(true)).WithMessage("Check is Required");
            //RuleFor(x => x.CreatedAt).NotNull().WithMessage("Date is required.");

            RuleForEach(c => c.UserRelations).NotEmpty();
        }
    }
}
