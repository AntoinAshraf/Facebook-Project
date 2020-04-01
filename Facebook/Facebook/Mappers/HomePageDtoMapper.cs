using Facebook.Models.ViewModels;
using Facebook.Utilities.Enums;
using FaceBook.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Mappers
{
    public class HomePageDtoMapper
    {
        public static IEnumerable<HomePageDto> Map(IEnumerable<User> from, IWebHostEnvironment hostingEnvironment)
        {
            var to = new List<HomePageDto>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from)
                {
                    to.Add(Map(item, hostingEnvironment));
                }
            }
            return to;
        }

        public static HomePageDto Map(User from, IWebHostEnvironment hostingEnvironment)
        {
            if (from == null) return null;

            List<HomeUserTempDto> homeUserDtos = Map(from.UserRelationsInitiator, from.UserRelationsDesider, hostingEnvironment, from.Id).ToList();
            var to = new HomePageDto
            {
                FullName = $"{from.FirstName} {from.LastName}",
                NumberOfFriends = from.UserRelationsDesider.Where(x => x.SocialStatusId == (int)SocialStatuses.Friend).Count()
                                    + from.UserRelationsInitiator.Where(x => x.SocialStatusId == (int)SocialStatuses.Friend).Count(),
                HomeUserDtos = homeUserDtos.Select(x=> new HomeUserDto(x.FullName, x.ProfilePicUrl)).ToList(),
                HomePostDto = GetAllPosts(homeUserDtos, from.UsersPosts, hostingEnvironment, from.Id).Select(x=> new HomePostDto(x.FullName, x.ProfilePic, x.PostDate, x.PostContent, x.HomeCommentDto, x.HomeLikeDto, x.PostPicUrl, x.PostId, x.CanEditDelete, x.IsLike)).ToList(),
            };
            string defaultPic = "";
            if (from.GenderId == 1 /* Male */)
                defaultPic = "default.jpg";
            else
                defaultPic = "default_female.png";

            string path = hostingEnvironment.WebRootPath + "/ProfilePics/" + (from.ProfilePhotos.Where(x => x.IsCurrent).Select(x => x.Url).FirstOrDefault() ?? defaultPic);
            byte[] b = System.IO.File.ReadAllBytes(path);
            to.ProfilePicUrl = "data:image/png;base64," + Convert.ToBase64String(b);

            return to;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        
        public static IEnumerable<HomePostTempDto> Map(IEnumerable<UsersPost> from, IWebHostEnvironment hostingEnvironment, int currentUserId)
        {
            var to = new List<HomePostTempDto>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from.Where(x=>x.Post.IsDeleted == false))
                {
                    to.Add(Map(item, hostingEnvironment, currentUserId));
                }
            }

            return to;
        }

        public static HomePostTempDto Map(UsersPost from, IWebHostEnvironment hostingEnvironment, int currentUserId)
        {
            if (from == null) return null;

            var to = new HomePostTempDto
            {
                FullName = $"{from.User.FirstName} {from.User.LastName}",
                PostContent = from.Post.PostContent,
                CreatedAt = from.CreatedAt,
                HomeCommentDto = Map(from.Post.Comments.OrderByDescending(x => x.CreatedAt), hostingEnvironment, currentUserId).ToList(),
                HomeLikeDto = Map(from.Post.Likes.OrderByDescending(x => x.CreatedAt), hostingEnvironment).ToList(),
                PostId = from.PostId,
                IsLike = from.Post.Likes.Any(x=>x.PostId == from.PostId && x.UserId == currentUserId && x.IsDeleted == false)
            };

            string path = hostingEnvironment.WebRootPath + "/ProfilePics/" + (from.User.ProfilePhotos.Where(x => x.IsCurrent).Select(x => x.Url).FirstOrDefault() ?? "default.jpg") ;
            byte[] b = System.IO.File.ReadAllBytes(path);
            to.ProfilePic = "data:image/png;base64," + Convert.ToBase64String(b);

            if(from.Post.PostPhotos.Select(x => x.Url).FirstOrDefault() != null)
            {
                string path2 = hostingEnvironment.WebRootPath + "/PostPics/" + from.Post.PostPhotos.Select(x => x.Url).FirstOrDefault();
                byte[] b2 = System.IO.File.ReadAllBytes(path2);
                to.PostPicUrl = "data:image/png;base64," + Convert.ToBase64String(b2);
            }

            TimeSpan? DateDifference = DateTime.Now - from.CreatedAt;
            if (DateDifference.Value.Days != 0) { to.PostDate = string.Format("posted {0} days ago", (DateDifference.Value.Days)); }
            if (DateDifference.Value.Days > 30) { to.PostDate = string.Format("from {0}", from.CreatedAt.ToString("dd/MM/yyyy")); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours != 0) { to.PostDate = string.Format("posted {0} h ago", DateDifference.Value.Hours); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes != 0) { to.PostDate = string.Format("posted {0} min ago", DateDifference.Value.Minutes); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes == 0) { to.PostDate = ("posted few sec ago "); }

            return to;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        public static IEnumerable<HomeUserTempDto> Map(IEnumerable<UserRelation> fromInitiator, IEnumerable<UserRelation> fromDecider, IWebHostEnvironment hostingEnvironment, int currentUserId)
        {
            var to = new List<HomeUserTempDto>();
            if (fromInitiator != null && fromInitiator.Count() > 0)
            {
                foreach (var item in fromInitiator.Where(x=>x.SocialStatusId == (int)SocialStatuses.Friend))
                {
                    to.Add(MapInitiator(item, hostingEnvironment, currentUserId));
                }
            }

            if (fromDecider != null && fromDecider.Count() > 0)
            {
                foreach (var item in fromDecider.Where(x => x.SocialStatusId == (int)SocialStatuses.Friend))
                {
                    to.Add(MapDecider(item, hostingEnvironment, currentUserId));
                }
            }
            return to;
        }

        public static HomeUserTempDto MapInitiator(UserRelation from, IWebHostEnvironment hostingEnvironment, int currentUserId)
        {
            if (from == null) return null;

            var to = new HomeUserTempDto
            {
                FullName = $"{from.Desider.FirstName} {from.Desider.LastName}",
                HomePostDto = Map(from.Desider.UsersPosts, hostingEnvironment, currentUserId).ToList(),
            };

            string path = hostingEnvironment.WebRootPath + "/ProfilePics/" + (from.Desider.ProfilePhotos.Where(x => x.IsCurrent).Select(x => x.Url).FirstOrDefault() ?? "default.jpg");
            byte[] b = System.IO.File.ReadAllBytes(path);
            to.ProfilePicUrl = "data:image/png;base64," + Convert.ToBase64String(b);

            return to;
        }

        public static HomeUserTempDto MapDecider(UserRelation from, IWebHostEnvironment hostingEnvironment,int currentUserId)
        {
            if (from == null) return null;

            var to = new HomeUserTempDto
            {
                FullName = $"{from.Initiator.FirstName} {from.Initiator.LastName}",
                HomePostDto = Map(from.Initiator.UsersPosts, hostingEnvironment, currentUserId).ToList(),
            };

            string path = hostingEnvironment.WebRootPath + "/ProfilePics/" + (from.Initiator.ProfilePhotos.Where(x => x.IsCurrent).Select(x => x.Url).FirstOrDefault() ?? "default.jpg");
            byte[] b = System.IO.File.ReadAllBytes(path);
            to.ProfilePicUrl = "data:image/png;base64," + Convert.ToBase64String(b);

            return to;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////

        public static IEnumerable<HomeCommentDto> Map(IEnumerable<Comment> from, IWebHostEnvironment hostingEnvironment, int currentUserId)
        {
            var to = new List<HomeCommentDto>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from.Where(x=>x.IsDeleted == false))
                {
                    to.Add(Map(item, hostingEnvironment, currentUserId));
                }
            }

            return to;
        }

        public static HomeCommentDto Map(Comment from, IWebHostEnvironment hostingEnvironment, int currentUserId)
        {
            if (from == null) return null;

            var to = new HomeCommentDto
            {
                FullName = $"{from.User.FirstName} {from.User.LastName}",
                CommentContent = from.CommentContent,
                CommentId = from.Id,
                CanDelete = from.UserId == currentUserId
            };

            string path = hostingEnvironment.WebRootPath + "/ProfilePics/" + (from.User.ProfilePhotos.Where(x => x.IsCurrent).Select(x => x.Url).FirstOrDefault() ?? "default.jpg");
            byte[] b = System.IO.File.ReadAllBytes(path);
            to.ProfilePicUrl =  "data:image/png;base64," + Convert.ToBase64String(b);

            TimeSpan? DateDifference = DateTime.Now - from.CreatedAt;
            if (DateDifference.Value.Days != 0) { to.CommentDate = string.Format("Commented {0} days ago", (DateDifference.Value.Days)); }
            if (DateDifference.Value.Days > 30) { to.CommentDate = string.Format("from {0}", from.CreatedAt.ToString("dd/MM/yyyy")); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours != 0) { to.CommentDate = string.Format("Commented {0} h ago", DateDifference.Value.Hours); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes != 0) { to.CommentDate = string.Format("Commented {0} min ago", DateDifference.Value.Minutes); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes == 0) { to.CommentDate = ("Commented few sec ago "); }

            return to;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////

        public static IEnumerable<HomeLikeDto> Map(IEnumerable<Like> from, IWebHostEnvironment hostingEnvironment)
        {
            var to = new List<HomeLikeDto>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from.Where(x=>x.IsDeleted == false))
                {
                    to.Add(Map(item, hostingEnvironment));
                }
            }

            return to;
        }

        public static HomeLikeDto Map(Like from, IWebHostEnvironment hostingEnvironment)
        {
            if (from == null) return null;

            var to = new HomeLikeDto
            {
                FullName = $"{from.User.FirstName} {from.User.LastName}",
                LikeId = from.Id
            };

            string path = hostingEnvironment.WebRootPath + "/ProfilePics/" + (from.User.ProfilePhotos.Where(x => x.IsCurrent).Select(x => x.Url).FirstOrDefault() ?? "default.jpg");
            byte[] b = System.IO.File.ReadAllBytes(path);
            to.ProfilePicUrl = "data:image/png;base64," + Convert.ToBase64String(b);

            TimeSpan? DateDifference = DateTime.Now - from.CreatedAt;
            if (DateDifference.Value.Days != 0) { to.LikeDate = string.Format("Liked {0} days ago", (DateDifference.Value.Days)); }
            if (DateDifference.Value.Days > 30) { to.LikeDate = string.Format("from {0}", from.CreatedAt.ToString("dd/MM/yyyy")); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours != 0) { to.LikeDate = string.Format("Liked {0} h ago", DateDifference.Value.Hours); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes != 0) { to.LikeDate = string.Format("Liked {0} min ago", DateDifference.Value.Minutes); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes == 0) { to.LikeDate = ("Liked few sec ago "); }

            return to;
        }

        /// /////////////////////////////////////////////////////////////////////////////////////////////////////////


        public static List<HomePostTempDto> GetAllPosts(List<HomeUserTempDto> homeUserDtos, IEnumerable<UsersPost> from, IWebHostEnvironment hostingEnvironment, int currentUserId)
        {
            List<HomePostTempDto> all = new List<HomePostTempDto>();
            List<HomePostTempDto> userPosts = Map(from, hostingEnvironment, currentUserId).ToList();

            foreach (var post in userPosts)
            {
                post.CanEditDelete = true;
                all.Add(post);
            }

            foreach (var user in homeUserDtos)
            {
                foreach (var post in user.HomePostDto)
                {
                    post.CanEditDelete = false;
                    all.Add(post);
                }
            }

            return all.OrderByDescending(x => x.CreatedAt).ToList();
        }
    }
}
