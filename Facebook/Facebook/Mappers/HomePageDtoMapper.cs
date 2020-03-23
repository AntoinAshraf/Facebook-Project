using Facebook.Models.ViewModels;
using Facebook.Utilities.Enums;
using FaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Mappers
{
    public class HomePageDtoMapper
    {
        public static IEnumerable<HomePageDto> Map(IEnumerable<User> from)
        {
            var to = new List<HomePageDto>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from)
                {
                    to.Add(Map(item));
                }
            }
            return to;
        }

        public static HomePageDto Map(User from)
        {
            if (from == null) return null;

            List<HomeUserTempDto> homeUserDtos = Map(from.UserRelationsInitiator, from.UserRelationsDesider).ToList();
            var to = new HomePageDto
            {
                FullName = $"{from.FirstName} {from.LastName}",
                ProfilePicUrl  = from.ProfilePhotos.Where(x=>x.IsCurrent).Select(x=>x.Url).FirstOrDefault(),
                NumberOfFriends = from.UserRelationsDesider.Where(x => x.SocialStatusId == (int)SocialStatuses.Friend).Count()
                                    + from.UserRelationsInitiator.Where(x => x.SocialStatusId == (int)SocialStatuses.Friend).Count(),
                HomeUserDtos = homeUserDtos.Select(x=> new HomeUserDto(x.FullName, x.ProfilePicUrl)).ToList(),
                HomePostDto = GetAllPosts(homeUserDtos, from.UsersPosts).Select(x=> new HomePostDto(x.FullName, x.ProfilePic, x.PostDate, x.PostContent, x.HomeCommentDto, x.HomeLikeDto, x.PostPicUrl, x.PostId)).ToList(),
            };

            return to;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////
        
        public static IEnumerable<HomePostTempDto> Map(IEnumerable<UsersPost> from)
        {
            var to = new List<HomePostTempDto>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from)
                {
                    to.Add(Map(item));
                }
            }

            return to;
        }

        public static HomePostTempDto Map(UsersPost from)
        {
            if (from == null) return null;

            var to = new HomePostTempDto
            {
                FullName = $"{from.User.FirstName} {from.User.LastName}",
                PostContent = from.Post.PostContent,
                PostPicUrl = from.Post.PostPhotos.Select(x => x.Url).FirstOrDefault(),
                CreatedAt = from.CreatedAt,
                HomeCommentDto = Map(from.Post.Comments.OrderByDescending(x => x.CreatedAt)).ToList(),
                HomeLikeDto = Map(from.Post.Likes.OrderByDescending(x => x.CreatedAt)).ToList(),
                ProfilePic = from.User.ProfilePhotos.Where(x => x.IsCurrent).Select(x => x.Url).FirstOrDefault(),
                PostId = from.PostId
            };

            TimeSpan? DateDifference = DateTime.Now - from.CreatedAt;
            if (DateDifference.Value.Days != 0) { to.PostDate = string.Format("posted {0} days ago", (DateDifference.Value.Days)); }
            if (DateDifference.Value.Days > 30) { to.PostDate = string.Format("from {0}", from.CreatedAt.ToString("dd/MM/yyyy")); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours != 0) { to.PostDate = string.Format("posted {0} h ago", DateDifference.Value.Hours); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes != 0) { to.PostDate = string.Format("posted {0} min ago", DateDifference.Value.Minutes); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes == 0) { to.PostDate = ("posted few sec ago "); }

            return to;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        public static IEnumerable<HomeUserTempDto> Map(IEnumerable<UserRelation> fromInitiator, IEnumerable<UserRelation> fromDecider)
        {
            var to = new List<HomeUserTempDto>();
            if (fromInitiator != null && fromInitiator.Count() > 0)
            {
                foreach (var item in fromInitiator)
                {
                    to.Add(MapInitiator(item));
                }
            }

            if (fromDecider != null && fromDecider.Count() > 0)
            {
                foreach (var item in fromDecider)
                {
                    to.Add(MapDecider(item));
                }
            }
            return to;
        }

        public static HomeUserTempDto MapInitiator(UserRelation from)
        {
            if (from == null) return null;

            var to = new HomeUserTempDto
            {
                FullName = $"{from.Desider.FirstName} {from.Desider.LastName}",
                HomePostDto = Map(from.Desider.UsersPosts).ToList(),
                ProfilePicUrl = from.Desider.ProfilePhotos.Where(x=>x.IsCurrent).Select(x=>x.Url).FirstOrDefault()
            };

            return to;
        }

        public static HomeUserTempDto MapDecider(UserRelation from)
        {
            if (from == null) return null;

            var to = new HomeUserTempDto
            {
                FullName = $"{from.Initiator.FirstName} {from.Initiator.LastName}",
                HomePostDto = Map(from.Initiator.UsersPosts).ToList(),
                ProfilePicUrl = from.Initiator.ProfilePhotos.Where(x => x.IsCurrent).Select(x => x.Url).FirstOrDefault()

            };

            return to;
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////

        public static IEnumerable<HomeCommentDto> Map(IEnumerable<Comment> from)
        {
            var to = new List<HomeCommentDto>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from)
                {
                    to.Add(Map(item));
                }
            }

            return to;
        }

        public static HomeCommentDto Map(Comment from)
        {
            if (from == null) return null;

            var to = new HomeCommentDto
            {
                FullName = $"{from.User.FirstName} {from.User.LastName}",
                CommentContent = from.CommentContent,
                ProfilePicUrl = from.User.ProfilePhotos.Where(x=>x.IsCurrent).Select(x=>x.Url).FirstOrDefault()
            };

            TimeSpan? DateDifference = DateTime.Now - from.CreatedAt;
            if (DateDifference.Value.Days != 0) { to.CommentDate = string.Format("posted {0} days ago", (DateDifference.Value.Days)); }
            if (DateDifference.Value.Days > 30) { to.CommentDate = string.Format("from {0}", from.CreatedAt.ToString("dd/MM/yyyy")); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours != 0) { to.CommentDate = string.Format("posted {0} h ago", DateDifference.Value.Hours); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes != 0) { to.CommentDate = string.Format("posted {0} min ago", DateDifference.Value.Minutes); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes == 0) { to.CommentDate = ("posted few sec ago "); }

            return to;
        }


        ///////////////////////////////////////////////////////////////////////////////////////////

        public static IEnumerable<HomeLikeDto> Map(IEnumerable<Like> from)
        {
            var to = new List<HomeLikeDto>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from)
                {
                    to.Add(Map(item));
                }
            }

            return to;
        }

        public static HomeLikeDto Map(Like from)
        {
            if (from == null) return null;

            var to = new HomeLikeDto
            {
                FullName = $"{from.User.FirstName} {from.User.LastName}",
                ProfilePicUrl = from.User.ProfilePhotos.Where(x=>x.IsCurrent).Select(x=>x.Url).FirstOrDefault()
            };

            TimeSpan? DateDifference = DateTime.Now - from.CreatedAt;
            if (DateDifference.Value.Days != 0) { to.LikeDate = string.Format("Liked {0} days ago", (DateDifference.Value.Days)); }
            if (DateDifference.Value.Days > 30) { to.LikeDate = string.Format("from {0}", from.CreatedAt.ToString("dd/MM/yyyy")); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours != 0) { to.LikeDate = string.Format("Liked {0} h ago", DateDifference.Value.Hours); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes != 0) { to.LikeDate = string.Format("Liked {0} min ago", DateDifference.Value.Minutes); }
            if (DateDifference.Value.Days == 0 && DateDifference.Value.Hours == 0 && DateDifference.Value.Minutes == 0) { to.LikeDate = ("Liked few sec ago "); }

            return to;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////

        public static List<HomePostTempDto> GetAllPosts(List<HomeUserTempDto> homeUserDtos, IEnumerable<UsersPost> from)
        {
            List<HomePostTempDto> all = new List<HomePostTempDto>();
            List<HomePostTempDto> userPosts = Map(from).ToList();

            foreach (var post in userPosts)
            {
                all.Add(post);
            }

            foreach (var user in homeUserDtos)
            {
                foreach (var post in user.HomePostDto)
                {
                    all.Add(post);
                }
            }

            return all.OrderByDescending(x => x.CreatedAt).ToList();
        }
    }
}
