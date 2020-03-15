using FaceBook.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class ActionsValidator : AbstractValidator<Actions>
    {
        public ActionsValidator()
        {
            RuleFor(x => x.Name).MustnotBeEmpty().MustnotStartWithWhiteSpace().PropLengthRange();
            RuleFor(x => x.Icon).MustnotBeEmpty().MustnotStartWithWhiteSpace();
            RuleFor(x => x.Url).MustnotBeEmpty().MustnotStartWithWhiteSpace();

            RuleForEach(c => c.RoleActions).NotEmpty();
        }
    }
}
