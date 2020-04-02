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
using FaceBook.Models;
using FacebookDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Facebook.Controllers
{
    public class CommentLikeController : Controller
    {
        private readonly FacebookDataContext facebookDataContext;
        private readonly IUserData userData;
        private readonly IWebHostEnvironment hostingEnvironment;

        public CommentLikeController(FacebookDataContext _facebookDataContext, IUserData _userData, IWebHostEnvironment hostingEnvironment)
        {
            this.userData = _userData;
            this.hostingEnvironment = hostingEnvironment;
            this.facebookDataContext = _facebookDataContext;
        }

        [HttpPost]
        [AuthorizedAction]
        public IActionResult AddComment([FromBody]CommentPostDto commentPostDto)
        {
            if (commentPostDto == null || commentPostDto.CommentContent == null)
                return Json(new { statusCode = ResponseStatus.ValidationError, responseMessage = ValidationMessages.EmptyComment });
            User user = userData.GetUser(HttpContext);
            Comment comment = CommentDtoMapper.Map(commentPostDto, user.Id);
            facebookDataContext.Comments.Add(comment);
            try { facebookDataContext.SaveChanges(); }
            catch { return Json(new { statusCode = ResponseStatus.Error }); }
            comment = facebookDataContext.Comments.Where(x => x.Id == comment.Id).Include("User.ProfilePhotos").FirstOrDefault();
            return Json(new { statusCode = ResponseStatus.Success, responseMessage = HomePageDtoMapper.Map(comment, hostingEnvironment, user.Id), postId = comment.PostId });
        }

        [HttpGet]
        [AuthorizedAction]
        public IActionResult AddLike([FromQuery]int postId)
        {
            if (postId == 0)
                return Json(new { statusCode = ResponseStatus.ValidationError });
            User user = userData.GetUser(HttpContext);
            Like alreadyExsistLike = facebookDataContext.Likes.Include("User.ProfilePhotos").FirstOrDefault(x => x.UserId == user.Id && x.PostId == postId);
            if (alreadyExsistLike != null)
            {
                alreadyExsistLike.IsDeleted = !alreadyExsistLike.IsDeleted;
                try { facebookDataContext.SaveChanges(); }
                catch { return Json(new { statusCode = ResponseStatus.Error }); }
                return Json(new { statusCode = ResponseStatus.Success, responseMessage = HomePageDtoMapper.Map(alreadyExsistLike, hostingEnvironment), IsLike = alreadyExsistLike.IsDeleted, LikeId = alreadyExsistLike.Id });
            }
            Like like = new Like() { PostId = postId, CreatedAt = DateTime.Now, IsDeleted = false, ReactionStatusId = (int)ReactionStatuses.Like, UserId = user.Id };
            facebookDataContext.Likes.Add(like);
            try { facebookDataContext.SaveChanges(); }
            catch { return Json(new { statusCode = ResponseStatus.Error }); }
            like = facebookDataContext.Likes.Where(x => x.Id == like.Id).Include("User.ProfilePhotos").FirstOrDefault();
            return Json(new { statusCode = ResponseStatus.Success, responseMessage = HomePageDtoMapper.Map(like, hostingEnvironment), IsLike = like.IsDeleted, LikeId = like.Id });
        }


        [HttpGet]
        [AuthorizedAction]
        public IActionResult DeleteComment([FromQuery]int commentId)
        {
            if (commentId == 0)
                return Json(new { statusCode = ResponseStatus.ValidationError });

            Comment comment = facebookDataContext.Comments.FirstOrDefault(x => x.Id == commentId);
            if (comment == null)
                return Json(new { statusCode = ResponseStatus.ValidationError });

            comment.IsDeleted = true;
            try { facebookDataContext.SaveChanges(); }
            catch { return Json(new { statusCode = ResponseStatus.Error }); }
            return Json(new { statusCode = ResponseStatus.Success });
        }
    }
}