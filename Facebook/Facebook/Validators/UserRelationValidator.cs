using FaceBook.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class UserRelationValidator : AbstractValidator<UserRelation>
    {
        public UserRelationValidator()
        {
            RuleFor(x => x.SocialStatusId).NotNull().WithMessage("Id is required"); //range???
            RuleFor(x => x.InitiatorId).NotNull().WithMessage("Id is required"); //range???
            RuleFor(x => x.DesiderId).NotNull().WithMessage("Id is required"); //range???
            RuleFor(x => x.IsDeleted).Must(x => x.Equals(true)).WithMessage("Check is Required");
            //RuleFor(x => x.CreatedAt).NotNull().WithMessage("Date is required.");
        }
    }
}
