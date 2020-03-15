using FaceBook.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class UserPostValidator : AbstractValidator<UsersPost>
    {
        public UserPostValidator()
        {
            RuleFor(x => x.UserId).NotNull().WithMessage("Id is required"); //range???
            RuleFor(x => x.PostId).NotNull().WithMessage("Id is required"); //range???
            //RuleFor(x => x.CreatedAt).NotNull().WithMessage("Date is required.");
            RuleFor(x => x.IsCreator).Must(x => x.Equals(true)).WithMessage("Check is Required");
        }
    }
}
