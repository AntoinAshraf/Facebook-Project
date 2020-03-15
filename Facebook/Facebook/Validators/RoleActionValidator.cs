using FaceBook.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class RoleActionValidator:AbstractValidator<RoleAction>
    {
        public RoleActionValidator()
        {
            RuleFor(x => x.RoleId).NotNull().WithMessage("Id is required"); //range???
            RuleFor(x => x.ActionId).NotNull().WithMessage("Id is required"); //range???
            RuleFor(x => x.CreatedBy).MustnotBeEmpty().MustnotStartWithWhiteSpace().PropLengthRange();
            //RuleFor(x => x.CreatedAt).NotNull().WithMessage("Date is required.");

        }
    }
}
