using FaceBook.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class UserValidator: AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x).NotNull().WithMessage("User Data cannot be Empity");
            RuleFor(x => x.Email).MustnotBeEmpty().MustnotStartWithWhiteSpace().EmailAddress();
            RuleFor(x => x.FirstName).MustnotBeEmpty().MustnotStartWithWhiteSpace().PropLengthRange();
            RuleFor(x => x.LastName).MustnotBeEmpty().MustnotStartWithWhiteSpace().PropLengthRange();
            RuleFor(x => x.Password).MustnotBeEmpty().MustnotStartWithWhiteSpace().PassFormate();
            RuleFor(x => x.PhoneNumber).MustnotBeEmpty().MustnotStartWithWhiteSpace().MatchPhoneNumberRule();
            RuleFor(x => x.BirthDate).NotNull().WithMessage("BirthDate is required.");
            RuleFor(x => x.GenderId).NotNull().WithMessage("Id is required"); //range???
            RuleFor(x => x.RoleId).NotNull().WithMessage("Id is required"); //range???
            RuleFor(x => x.CreatedBy).MustnotBeEmpty().MustnotStartWithWhiteSpace().PropLengthRange();
            RuleFor(x => x.UpdatedBy).MustnotBeEmpty().MustnotStartWithWhiteSpace().PropLengthRange();
            RuleFor(x => x.DeletedBy).MustnotBeEmpty().MustnotStartWithWhiteSpace().PropLengthRange();
            //RuleFor(x => x.CreatedAt).NotNull().WithMessage("Date is required.");
            //RuleFor(x => x.UpdatedAt).NotNull().WithMessage("Date is required.");
            //RuleFor(x => x.DeletedAt).NotNull().WithMessage("Date is required.");
            RuleFor(x => x.IsActive).Must(x => x.Equals(true)).WithMessage("Check is Required");
            RuleFor(x => x.IsDeleted).Must(x => x.Equals(true)).WithMessage("Check is Required");
            RuleFor(x => x.IsCreatedByAdmin).Must(x => x.Equals(true)).WithMessage("Check is Required");

            //RuleForEach(x => x.Comments).NotEmpty();
            //RuleForEach(x => x.Likes).NotEmpty();
            //RuleForEach(x => x.ProfilePhotos).NotEmpty();
            //RuleForEach(x => x.UserRelationsDesider).NotEmpty();
            //RuleForEach(x => x.UserRelationsInitiator).NotEmpty();
            //RuleForEach(x => x.UsersPosts).NotEmpty();
        }
    }
}
