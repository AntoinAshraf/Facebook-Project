using Facebook.Models.ViewModels;
using Facebook.Recources;
using Facebook.Utilities;
using FaceBook.Models;
using FacebookDbContext;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class UserLoginValidator : AbstractValidator<UserLoginDTO>
    {
        private readonly FacebookDataContext db;
        private User user { get; set; }
        public UserLoginValidator(FacebookDataContext _db, User _user)
        {
            db = _db;
            user = _user;
            Initialize();
        }

        private void Initialize()
        {
            RuleFor(x => x).NotNull().WithMessage(ValidationMessages.EmptyUser);

            RuleFor(x => x.Email).NotNull().WithMessage(ValidationMessages.EmptyUser);
            RuleFor(x => x.Password).NotNull().WithMessage(ValidationMessages.EmptyPassword);

            RuleFor(x => x.Email).EmailAddress().WithMessage(ValidationMessages.EmailNotValid);
            RuleFor(x => x.Password).MaximumLength(255).WithMessage(ValidationMessages.LongString);
            RuleFor(x => x.Password).MinimumLength(5).WithMessage(ValidationMessages.ShortPassword);


            RuleFor(x => x.Email).Must(EmailExsist).WithMessage(ValidationMessages.IncorrectEmailOrPassword);
            RuleFor(x => x.Password).Must(CorrectPassword).WithMessage(ValidationMessages.IncorrectEmailOrPassword);
            RuleFor(x => x).Must(IsActive).WithMessage(ValidationMessages.NotActive);

        }

        private bool EmailExsist(string email)
        {
            if (user == null) return false;
            return true;
        }

        private bool CorrectPassword(string password)
        {
            user.Password = Encription.Decrypt(user.Password, "SecretCode_hamed");
            if (user.Password != password)
                return false;
            return true;
        }

        private bool IsActive(UserLoginDTO userLoginDTO)
        {
            if (!user.IsActive)
                return false;
            return true;
        }
    }
}
