using FaceBook.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(x => x.PostContent).MustnotBeEmpty().MustnotStartWithWhiteSpace();
            RuleFor(x => x.IsDeleted).Must(x => x.Equals(true)).WithMessage("Check is Required");

            //RuleForEach(c => c.Comments).NotEmpty(); 
            //RuleForEach(c => c.PostPhotos).NotEmpty(); 
            RuleForEach(c => c.UsersPosts).NotEmpty();
        }
    }
}
