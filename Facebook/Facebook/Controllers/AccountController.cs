using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Contracts;
using Facebook.Mappers;
using Facebook.Models.ViewModels;
using Facebook.Recources;
using Facebook.Utilities;
using Facebook.Utilities.Enums;
using Facebook.Validators;
using FaceBook.Models;
using FacebookDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Facebook.Controllers
{
    public class AccountController : Controller
    {
        private readonly FacebookDataContext db;
        private readonly IUserData userData;
        private readonly IEmail email;
        private readonly IJwt jwt;
        public IConfiguration Configuration { get; }
        public AccountController(FacebookDataContext _myDbContext, IUserData _userData, IConfiguration _Configuration, IEmail _email, IJwt _jwt)
        {
            db = _myDbContext;
            userData = _userData;
            Configuration = _Configuration;
            email = _email;
            jwt = _jwt;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register([FromBody]UserRegisterDTO userRegisterDto)
        {
            User user = UserMapper.Map(userRegisterDto);
            FillEmptyFields(user);
            UserValidator validator = new UserValidator(ValidationMode.Create, db);
            var result = validator.Validate(user);
            if (!result.IsValid)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = result.Errors });
            }
            user.Password = Encription.Encrypt(user.Password, "SecretCode_hamed");
            db.Add(user);
            db.SaveChanges();
            string token = jwt.GenerateToken(user.Id);
            email.SendAccountActivationEmail(user.Email, "https://localhost:44340/Account/ActivateAccount/?token=" + token);
            return Json(new { statusCode = ResponseStatus.Success, responseMessage = user.Id });
        }

        [HttpPost]
        public IActionResult Login([FromBody]UserLoginDTO userLoginDto)
        {
            User user = db.Users.FirstOrDefault(x => x.Email == userLoginDto.Email);
            if(user == null)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.IncorrectEmailOrPassword });
            }
            UserLoginValidator validator = new UserLoginValidator(db, user);
            var result = validator.Validate(userLoginDto);
            if (!result.IsValid)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = result.Errors });
            }
            userData.SetUser(HttpContext, user);
            List<Actions> actions = db.RoleActions.Where(s => s.RoleId == user.RoleId).Select(s => s.Action).ToList();
            userData.SetActions(HttpContext, actions);
            return Json(new { statusCode = ResponseStatus.Success   });
        }

        public IActionResult ActivateAccount([FromQuery]string token)
        {
            bool TokenIsValid = jwt.ValidateCurrentToken(token);
            if (TokenIsValid)
            {
                int userId = int.Parse(jwt.GetId(token));
                User user = db.Users.Where(s => s.Id == userId).FirstOrDefault();
                if (!user.IsActive)
                {
                    user.IsActive = true;
                    db.Users.Update(user);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Register", "Account");
        }

        /////////////////////////////////////////////////////////////////////////////////////
        //helper

        private void FillEmptyFields(User user)
        {
            user.IsActive = false;
            user.IsDeleted = false;
            user.IsCreatedByAdmin = false;
            user.CreatedAt = DateTime.Now;
            user.RoleId = 1; //normal user
        }
    }
}