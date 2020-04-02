using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Facebook.Contracts;
using Facebook.Mappers;
using Facebook.Models.ViewModels;
using Facebook.Recources;
using Facebook.Utilities;
using Facebook.Utilities.Enums;
using FaceBook.Models;
using FacebookDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
////DESKTOP-J75213F\SQLEXPRESS
namespace Facebook.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ILogger<ProfileController> _logger;
        private readonly FacebookDataContext facebookDataContext;
        private readonly IUserData userData;
        private readonly IHostingEnvironment hostingEnvironment;

        public ProfileController(ILogger<ProfileController> logger, FacebookDataContext _facebookDataContext, IUserData _userData
              , IHostingEnvironment hostingEnvironment)
        {
            userData = _userData;
            this.hostingEnvironment = hostingEnvironment;
            _logger = logger;
            facebookDataContext = _facebookDataContext;
        }

        [AuthorizedAction]
        [HttpGet]
        public IActionResult Profile(int? id)
        {
            ViewData["LayoutData"] = userData.GetLayoutData(HttpContext);

            //ViewData["Actions"] = userData.GetActions(HttpContext);
            ViewData["Users"] = userData.GetUser(HttpContext);


            User currentUser = userData.GetUser(HttpContext);

            if (id == null)
                return RedirectToAction("Login", "Account");

            //to confirm Valid Id
            if (id != currentUser.Id)
            {
                var user = facebookDataContext.Users.Any(u => u.IsDeleted == false && u.Id == id);
                if (!user)
                {
                    return RedirectToAction("Login", "Account");
                }
            }

            var AllUserdata = facebookDataContext.Users.Where(user => user.Id == id)
                .Include("Gender")
                //.Include("ProfilePhotos")//profilePhotos
                .Include("UserRelationsDesider.Initiator.ProfilePhotos")//requests&countFriend
                .Include("UserRelationsDesider.Desider.ProfilePhotos")
                .Include("UserRelationsInitiator")//CountFriend
                .Include("UsersPosts.Post.Comments.User.ProfilePhotos")//commenets
                .Include("UsersPosts.Post.Likes.User.ProfilePhotos")//postsAndLikes/user
                .Include("UsersPosts.Post.PostPhotos").SingleOrDefault();//postsAndPhoto


            ProfilePageDto profilePageDto = ProfilePageDtoMapper.Mapper(AllUserdata, (int)currentUser.Id);

            ViewBag.Gender = new SelectList(facebookDataContext.Gender, "Id", "GenderName", AllUserdata.GenderId);


            return View(profilePageDto);
        }

        [AuthorizedAction]
        [HttpDelete]
        public IActionResult rejectRequest([FromQuery]int? intiatorId, [FromQuery] int? deciderId)
        {
            if (intiatorId == null || deciderId == null)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError });
            }
            try
            {
                var result = facebookDataContext.UserRelations.
                    Where(R => R.InitiatorId == intiatorId && R.DesiderId == deciderId && R.IsDeleted == false
                    && R.SocialStatusId == (int)SocialStatuses.Request).FirstOrDefault();
                result.IsDeleted = true;
                facebookDataContext.SaveChanges();
                return Json(new { statusCode = ResponseStatus.Success });
            }
            catch
            {
                return Json(new { statusCode = ResponseStatus.Error });
            }

        }

        [AuthorizedAction]
        [HttpPut]
        public IActionResult acceptRequest([FromQuery]int? intiatorId, [FromQuery] int? deciderId)
        {
            if (intiatorId == null || deciderId == null)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError });
            }
            try
            {
                var resultFriendRequest = facebookDataContext.UserRelations.
                    Where(R => R.InitiatorId == intiatorId && R.DesiderId == deciderId && R.IsDeleted == false
                    && R.SocialStatusId == (int)SocialStatuses.Request).FirstOrDefault();
                resultFriendRequest.SocialStatusId = 1; // Friend
                facebookDataContext.SaveChanges();
                return Json(new { statusCode = ResponseStatus.Success });
            }
            catch
            {
                return Json(new { statusCode = ResponseStatus.Error });
            }

        }

        [AuthorizedAction]
        [HttpPut]
        public IActionResult EditInfo([FromBody] userInfo userInfoToUpdate)
        {
            if (userInfoToUpdate == null || userInfoToUpdate.id == 0)
            {
                return Json(new { statusCode = ResponseStatus.ValidationError });
            }


            try
            {
                User user = facebookDataContext.Users.Where(u => u.Id == userInfoToUpdate.id).FirstOrDefault();

                if (user == null)
                    return Json(new { statusCode = ResponseStatus.NoDataFound });


                user.Bio = userInfoToUpdate.Bio;
                user.PhoneNumber = userInfoToUpdate.PhoneNumber;
                user.BirthDate = userInfoToUpdate.BirthDate;

                if (userInfoToUpdate.GenderName == "Male")
                    user.GenderId = 1;
                else // female
                    user.GenderId = 2;

                string[] nameSplitted = userInfoToUpdate.FullName.Split(" "); // to get first, last name
                user.FirstName = nameSplitted[0];
                user.LastName = nameSplitted[1];

                facebookDataContext.SaveChanges();

                // Saving user for session
                userData.SetUser(HttpContext, user);
                return Json(new { statusCode = ResponseStatus.Success });
            }
            catch (Exception)
            {
                return Json(new { statusCode = ResponseStatus.Error });
            }
        }

        [AuthorizedAction]
        [HttpPost]
        public IActionResult AddFriend([FromQuery] int? initiatorId, [FromQuery] int? deciderId)
        {
            if (initiatorId == null || deciderId == null)
                return Json(new { statusCode = ResponseStatus.ValidationError });
            try
            {
                if (!facebookDataContext.Users.Any(u => u.Id == initiatorId) ||
                    !facebookDataContext.Users.Any(u => u.Id == deciderId))
                    return Json(new { statusCode = ResponseStatus.NoDataFound });

                UserRelation newUserRelation = new UserRelation()
                {
                    InitiatorId = (int)initiatorId,
                    DesiderId = (int)deciderId,
                    SocialStatusId = (int)SocialStatuses.Request,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                };
                facebookDataContext.UserRelations.Add(newUserRelation);
                facebookDataContext.SaveChanges();
                return Json(new { statusCode = ResponseStatus.Success });
            }
            catch (Exception)
            {
                return Json(new { statusCode = ResponseStatus.Error });
            }
        }

        [AuthorizedAction]
        [HttpPut]
        public IActionResult RemoveFriend([FromQuery] int? initiatorId, [FromQuery] int? deciderId)
        {
            if (initiatorId == null || deciderId == null)
                return Json(new { statusCode = ResponseStatus.ValidationError });
            try
            {
                if (!facebookDataContext.Users.Any(u => u.Id == initiatorId) ||
                    !facebookDataContext.Users.Any(u => u.Id == deciderId))
                    return Json(new { statusCode = ResponseStatus.NoDataFound });

                var relationToRemove = facebookDataContext.UserRelations.Where(u => u.InitiatorId == initiatorId && u.DesiderId == deciderId
                 && u.SocialStatusId == (int)SocialStatuses.Friend && u.IsDeleted == false).FirstOrDefault();

                if (relationToRemove == null)
                    relationToRemove = facebookDataContext.UserRelations.Where(u => u.InitiatorId == deciderId && u.DesiderId == initiatorId
                 && u.SocialStatusId == (int)SocialStatuses.Friend && u.IsDeleted == false).FirstOrDefault();

                if (relationToRemove == null)
                {
                    return Json(new { statusCode = ResponseStatus.NoDataFound });
                }
                relationToRemove.IsDeleted = true; // remove the relationship
                facebookDataContext.UserRelations.Remove(relationToRemove);
                facebookDataContext.SaveChanges();

                return Json(new { statusCode = ResponseStatus.Success });
            }
            catch (Exception)
            {
                return Json(new { statusCode = ResponseStatus.Error });
            }
        }

        [AuthorizedAction]
        [HttpPut]
        public IActionResult ChangeProfilePhoto(/*[FromBody]*/IFormFile profileImage, [FromQuery] int? userId)
        {
            string fileName = "";
            DateTime dateTimeNow = DateTime.Now;

            User user = userData.GetUser(HttpContext);

            if (profileImage == null)
                return Json(new { statusCode = ResponseStatus.Error });

            if (profileImage != null)
            {
                if (profileImage.ContentType != "image/jpeg" && profileImage.ContentType != "image/jpg" && profileImage.ContentType != "image/png")
                    return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.WrongFormat });

                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "ProfilePics");
                fileName = profileImage.FileName.Split(".")[0] + "_" + DateTime.Now.ToFileTime() + "." + profileImage.FileName.Split(".")[1];
                var filePath = Path.Combine(uploads, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    profileImage.CopyTo(fileStream);
                }
            }

            if (profileImage != null)
            {
                // Set the changed profile photo from isCurrent = ture -> false
                ProfilePhoto oldProfilePhoto = facebookDataContext.ProfilePhotos.
                    Where(p => p.UserId == userId && p.IsCurrent == true).FirstOrDefault();
                if (oldProfilePhoto != null)
                {
                    oldProfilePhoto.IsCurrent = false;
                    try { facebookDataContext.SaveChanges(); }
                    catch { return Json(new { statusCode = ResponseStatus.Error }); }
                }

                // Assigning values of the new profile photo
                ProfilePhoto newProfilePhoto = new ProfilePhoto()
                {
                    UserId = (int)userId,
                    Url = fileName,
                    IsCurrent = true,
                    CreatedAt = dateTimeNow,
                    IsDeleted = false
                };

                if (user.ProfilePhotos.FirstOrDefault(p => p.UserId == userId && p.IsCurrent == true) != null)
                    user.ProfilePhotos.FirstOrDefault(p => p.UserId == userId && p.IsCurrent == true).Url = fileName;

                facebookDataContext.ProfilePhotos.Add(newProfilePhoto);
                try
                {
                    facebookDataContext.SaveChanges();
                    userData.SetUser(HttpContext, user);
                }
                catch { return Json(new { statusCode = ResponseStatus.Error }); }
            }

            return Json(new { statusCode = ResponseStatus.Success });
        }
    }
}