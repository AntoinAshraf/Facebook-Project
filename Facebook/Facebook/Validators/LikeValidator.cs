using FaceBook.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class LikeValidator : AbstractValidator<Like>
    {
        public LikeValidator()
        {
            RuleFor(x => x.PostId).NotNull().WithMessage("Id is required"); //range???
            RuleFor(x => x.ReactionStatusId).NotNull().WithMessage("Id is required"); //range???
            RuleFor(x => x.UserId).NotNull().WithMessage("Id is required"); //range???
            RuleFor(x => x.IsDeleted).Must(x => x.Equals(true)).WithMessage("Check is Required");
           // RuleFor(x => x.CreatedAt).NotNull().WithMessage("Date is required.");

        }
    }
}
