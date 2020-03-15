using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Validators
{
    public static class ModelValidators
    {
        //Property Mustnot Start With WhiteSpace
        public static IRuleBuilderOptions<T, string> MustnotStartWithWhiteSpace<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(m => m != null && !m.StartsWith(" ")).WithMessage("'{PropertyName}' should not start with white space");
        }
        //Property mustnot be Empty
        public static IRuleBuilderOptions<T, string> MustnotBeEmpty<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.NotEmpty().WithMessage("'{PropertyName}' is required.");
        }
        //name lengh must be between 3 and 10 characters
        public static IRuleBuilderOptions<T, string> PropLengthRange<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Must(m => m != null && m.Length < 11).WithMessage("'{PropertyName}' should not be more than 10 character");
        }
        //password format
        public static IRuleBuilderOptions<T, string> PassFormate<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.Matches("^(?=.*[0-9])(?=.*[a-zA-Z])([a-zA-Z0-9]+)$").WithMessage("regex error");
        }
        //phone nummber rule
        public static IRuleBuilderOptions<T, string> MatchPhoneNumberRule<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator(@"((?:[0-9]\-?){6,14}[0-9]$)|((?:[0-9]\x20?){6,14}[0-9]$)"));
        }
        //Date Time
        //public static IRuleBuilderOptions<T, string> MatchDateTime<T>(this IRuleBuilder<T, string> ruleBuilder)
        //{
        //    //return ruleBuilder.Must(d => BeAValidDate(d)).WithMessage("Invalid date/time");
        //}

        //Check Box
        //public static IRuleBuilderOptions<T, string> MatchPhoneNumberRule<T>(this IRuleBuilder<T, bool> ruleBuilder)
        //{
        //    return ruleBuilder.Must(x => x.Equals(true)).WithMessage("Full Name is Required");
        //}

    }
}
