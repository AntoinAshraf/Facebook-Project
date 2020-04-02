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
using Microsoft.EntityFrameworkCore;
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
            User user = db.Users.Include(x=>x.ProfilePhotos).FirstOrDefault(x => x.Email == userLoginDto.Email && x.IsDeleted == false);
            if (user == null)
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
            return Json(new { statusCode = ResponseStatus.Success });
        }

        public IActionResult ActivateAccount([FromQuery]string token)
        {
            bool TokenIsValid = jwt.ValidateCurrentToken(token);
            if (TokenIsValid)
            {
                int userId = int.Parse(jwt.GetId(token));
                User user = db.Users.Include(x=>x.ProfilePhotos).Where(s => s.Id == userId).FirstOrDefault();
                if (!user.IsActive)
                {
                    user.IsActive = true;
                    db.Users.Update(user);
                    db.SaveChanges();
                    userData.SetUser(HttpContext, user);
                }
            }
            return RedirectToAction("Register", "Account");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        public ActionResult SendForgetPasswordCode([FromQuery]string Email)
        {
            var user = db.Users.Include(x => x.ProfilePhotos).Where(s => s.Email == Email).FirstOrDefault();
            if (user == null)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.EmailNotExsist });
            }
            int code = new Random().Next(5000, 50000);
            user.RecoveryCode = code;
            db.SaveChanges();
            userData.SetUser(HttpContext, user);
            string token = jwt.GenerateToken(user.Id);
            email.SendRecoveryPasswordEmail(user.Email, code, "https://localhost:44340/Account/RecoverPassword/?token=" + token);
            return Json(new { statusCode = ResponseStatus.Success });
        }

        public ActionResult RecoverPassword([FromQuery]string token)
        {
            bool TokenIsValid = jwt.ValidateCurrentToken(token);
            ViewData["TokenIsValid"] = TokenIsValid;
            if (TokenIsValid)
            {
                int userId = int.Parse(jwt.GetId(token));
                User user = db.Users.Where(s => s.Id == userId && s.IsDeleted == false).FirstOrDefault();
                return View(user);
            }
            return View();
        }

        [HttpPost]
        public ActionResult RecoverPassword([FromBody]UserLoginDTO userLoginDTO, [FromQuery]string code)
        {
            User user = db.Users.Include(x => x.ProfilePhotos).Where(s => s.Email == userLoginDTO.Email && s.IsDeleted == false).FirstOrDefault();
            if (user == null)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.EmailNotExsist });
            }
            if (user.RecoveryCode.ToString() != code)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.WrongCode });
            }
            if (userLoginDTO.Password.Length < 5)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.ShortPassword });
            }
            user.Password = Encription.Encrypt(userLoginDTO.Password, "SecretCode_hamed");
            user.RecoveryCode = null;
            db.Update(user);
            db.SaveChanges();
            userData.SetUser(HttpContext, user);
            return Json(new { statusCode = ResponseStatus.Success });
        }

        [AuthorizedAction]
        public ActionResult ChangePassword()
        {
            ViewData["LayoutData"] = userData.GetLayoutData(HttpContext);
            return View();
        }

        [HttpPost]
        [AuthorizedAction]
        public ActionResult ChangePassword([FromBody]ChangePasswordDto changePasswordDto)
        {
            User user = userData.GetUser(HttpContext);
            user.Password = Encription.Decrypt(user.Password, "SecretCode_hamed");
            if (user.Password != changePasswordDto.OldPassword)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.IncorrectPassword });
            }
            if (changePasswordDto.NewPassword.Length < 5)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.ShortPassword });
            }
            user.Password = Encription.Encrypt(changePasswordDto.NewPassword, "SecretCode_hamed");
            db.Users.Update(user);
            db.SaveChanges();
            userData.SetUser(HttpContext, user);
            return Json(new { statusCode = ResponseStatus.Success });
        }

        public ActionResult Logout()
        {
            userData.clearData(HttpContext);
            return RedirectToAction("Register");
        }

        [AuthorizedAction]
        public IActionResult AdminControl()
        {
            ViewData["LayoutData"] = userData.GetLayoutData(HttpContext);
            User user = userData.GetUser(HttpContext);
            List<UserAdminControlDto> users = new List<UserAdminControlDto>(); 
            if (user.RoleId == (int)UserType.SuperAdmin)
            {
                ViewData["Roles"] = db.Roles.Where(x=>x.Id != (int)UserType.SuperAdmin).ToList();
                users = UserToUserBanDtoMapper.Map(db.Users.Where(x=>x.Id != user.Id && x.IsDeleted == false).Include(x => x.ProfilePhotos).ToList());
            }
            else if (user.RoleId == (int)UserType.Admin)
            {
                ViewData["Roles"] = db.Roles.Where(x => x.Id != (int)UserType.SuperAdmin && x.Id != (int)UserType.Admin).ToList();
                users = UserToUserBanDtoMapper.Map(db.Users.Where(x=>x.RoleId != (int)UserType.SuperAdmin && x.RoleId != (int)UserType.Admin && x.IsDeleted == false).Include(x => x.ProfilePhotos).ToList());
            }
            return View(users);
        }

        [AuthorizedAction]
        [HttpGet("Account/UserBan/{userId}")]
        public IActionResult UserBan([FromRoute]int userId, [FromQuery]bool ban)
        {
            User currentUser = userData.GetUser(HttpContext);
            if (currentUser.RoleId == (int)UserType.SuperAdmin || currentUser.RoleId == (int)UserType.Admin)
            {
                User user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (user != null && ban)
                    user.IsActive = false;
                else if(user != null && !ban)
                    user.IsActive = true;
                else
                    return Json(new { statusCode = ResponseStatus.NoDataFound });

                db.SaveChanges();
                return Json(new { statusCode = ResponseStatus.Success });
            }
            return Json(new { statusCode = ResponseStatus.Unauthorized });
        }

        [AuthorizedAction]
        [HttpGet("Account/ChangeRole/{userId}")]
        public IActionResult ChangeRole([FromRoute]int userId, [FromQuery]int roleId)
        {
            User currentUser = userData.GetUser(HttpContext);
            if (currentUser.RoleId == (int)UserType.SuperAdmin || currentUser.RoleId == (int)UserType.Admin)
            {
                User user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (user == null || !db.Roles.Any(x => x.Id == roleId))
                    return Json(new { statusCode = ResponseStatus.ValidationError });

                user.RoleId = roleId;
                db.SaveChanges();
                return Json(new { statusCode = ResponseStatus.Success });
            }
            return Json(new { statusCode = ResponseStatus.Unauthorized });
        }
        /////////////////////////////////////////////////////////////////////////////////////
        //helper

        private void FillEmptyFields(User user)
        {
            user.IsActive = false;
            user.IsDeleted = false;
            user.IsCreatedByAdmin = false;
            user.CreatedAt = DateTime.Now;
            user.RoleId = (int)UserType.User; //normal user
        }
    }
}