using Facebook.Recources;
using Facebook.Utilities.Enums;
using FaceBook.Models;
using FacebookDbContext;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        private readonly FacebookDataContext db;
        public UserValidator(ValidationMode mode, FacebookDataContext _db)
        {
            db = _db;
            switch (mode)
            {
                case ValidationMode.Create:
                    {
                        Initialize();
                        RuleFor(x => x.Email).Must(UniqueEmail).WithMessage(ValidationMessages.EmailNotUnique);
                        break;
                    }
                case ValidationMode.Update:
                    {
                        Initialize();
                        RuleFor(x => x.Id).NotEmpty().WithMessage(ValidationMessages.MissingId);
                        break;
                    }
                case ValidationMode.Delete:
                    {
                        break;
                    }
            }
        }

        private void Initialize()
        {
            RuleFor(x => x).NotNull().WithMessage(ValidationMessages.EmptyUser);

            RuleFor(x => x.Email).NotNull().WithMessage(ValidationMessages.EmptyUser);
            RuleFor(x => x.FirstName).NotNull().WithMessage(ValidationMessages.EmptyFirstName);
            RuleFor(x => x.LastName).NotNull().WithMessage(ValidationMessages.EmptyLastName);
            RuleFor(x => x.Password).NotNull().WithMessage(ValidationMessages.EmptyPassword);
            RuleFor(x => x.PhoneNumber).NotNull().WithMessage(ValidationMessages.EmptyNumber);
            RuleFor(x => x.BirthDate).NotNull().WithMessage(ValidationMessages.EmptyBirthDate);
            RuleFor(x => x.GenderId).NotNull().WithMessage(ValidationMessages.EmptyGender);

            RuleFor(x => x.Email).EmailAddress().WithMessage(ValidationMessages.EmailNotValid);
            RuleFor(x => x.FirstName).MaximumLength(255).WithMessage(ValidationMessages.LongString);
            RuleFor(x => x.LastName).MaximumLength(255).WithMessage(ValidationMessages.LongString);
            RuleFor(x => x.Password).MaximumLength(255).WithMessage(ValidationMessages.LongString);
            RuleFor(x => x.PhoneNumber).MaximumLength(255).WithMessage(ValidationMessages.LongString);
            RuleFor(x => x.Password).MinimumLength(5).WithMessage(ValidationMessages.ShortPassword);


            RuleFor(x => x.PhoneNumber).Must(IsPhoneNumber).WithMessage(ValidationMessages.NotValidPhoneNumber);
            RuleFor(x => x.GenderId).Must(GenderExsist).WithMessage(ValidationMessages.GenderNotExsist);
            RuleFor(x => x.RoleId).Must(RoleExsist).WithMessage(ValidationMessages.RoleNotExsist);
        }

        private bool UniqueEmail(string email)
        {
            return !db.Users.Any(x => x.Email == email);
        }

        private bool GenderExsist(int GenderId)
        {
            return db.Gender.Any(x => x.Id == GenderId);
        }

        private bool RoleExsist(int RoleId)
        {
            return db.Roles.Any(x => x.Id == RoleId);
        }

        private bool IsPhoneNumber(string PhoneNumber)
        {
            return Regex.IsMatch(PhoneNumber, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{4}");
        }
    }
}
