using Facebook.Models.ViewModels;
using Facebook.Utilities.Enums;
using FaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Mappers
{
    public static class ProfilePageDtoMapper
    {
        public static ProfilePageDto Mapper(User From,int UserId)
        {
            ProfilePageDto to = new ProfilePageDto();

            to.UserInfo = mapperInfo(From);
            to.UserProfilePhoto = mapperProfilePhoto(From);
            to.AllProfilePhotos = mapperAllPhoto(From);
            to.CanEditProfileWritePost = mapperCanChange(From, UserId);
            to.FriendRequests = mapperFriendRequest(From, UserId);
            to.NumberOfFriends = mapperNumOfFriends(From);
            to.Posts = mapperPosts(From, UserId);

            return to;
        }

        public static userInfo mapperInfo(User From)
        {
            userInfo info = new userInfo();
            info.FullName = $"{From.FirstName} ${From.LastName}";
            info.BirthDate = From.BirthDate;
            info.GenderName = From.Gender.GenderName;
            info.PhoneNumber = From.PhoneNumber;

            return info;
        }

        public static string mapperProfilePhoto(User From)
        {
            string Url = From.ProfilePhotos.Where(user =>user.IsCurrent==true).Select(pho=>pho.Url).FirstOrDefault();
            return Url;
        }

        public static List<string> mapperAllPhoto(User From)
        {
            List<string> AllPhoto = From.ProfilePhotos.Where(photo => photo.IsDeleted == true).Select(photo => photo.Url).ToList();
            return AllPhoto;
        }

        public static bool mapperCanChange(User From,int id)
        {
            return From.Id == id;
        }

        public static List<FriendRequest> mapperFriendRequest(User From,int id)
        {
            if (id == From.Id) {

                List<FriendRequest> friendRequests = new List<FriendRequest>();

                foreach (var item in From.UserRelationsDesider)
                {
                   
                    if (item.Initiator.IsDeleted == false)
                    {
                        FriendRequest friendRequest = new FriendRequest();
                        friendRequest.FullName = $"{item.Initiator.FirstName} {item.Initiator.LastName}";
                        friendRequest.Photo = item.Initiator.ProfilePhotos.Where(photo => photo.IsCurrent == true).Select(photo => photo.Url).FirstOrDefault();
                        friendRequests.Add(friendRequest);
                    }
                }
                return friendRequests;
            }

            return null;
            
        }

        public static int mapperNumOfFriends(User From)
        {
          int num=  From.UserRelationsDesider.Where(user => user.SocialStatusId == (int)SocialStatuses.Friend && user.IsDeleted==false).Count()+
            From.UserRelationsInitiator.Where(user => user.SocialStatusId == (int)SocialStatuses.Friend&&user.IsDeleted==false).Count();

            return num;
        }

        public static List<userPost> mapperPosts(User From,int id)
        {
            List<userPost> userPosts = new List<userPost>();
            foreach(var item in From.UsersPosts)
            {
                if (item.Post.IsDeleted == false)
                {
                    userPost userPost = new userPost();

                    // mapping
                    userPost.PostContent = item.Post.PostContent;
                    userPost.PostDate = GetPostCreateDate(item.CreatedAt);
                    userPost.CanChange = item.IsCreator;
                    userPost.IsLike = item.Post.Likes.Any(u => u.UserId == id&&u.IsDeleted==false);
                    userPost.numOfLikes = item.Post.Likes.Where(u=>u.IsDeleted==false).Count();
                    userPost.Likes = GetPostLikes(item.Post.Likes.ToList());
                    userPost.Comments = GetPostComments(item.Post.Comments.ToList());
                    userPost.PostPhoto = item.Post.PostPhotos.Select(p => p.Url).FirstOrDefault();

                    userPosts.Add(userPost);
                }
            }
            return userPosts;

        }


        public static List<postComment> GetPostComments(List<Comment> Comments)
        {
            List<postComment> postComments = new List<postComment>();
            foreach (var comment in Comments)
            {
                postComment postComment = new postComment();
                postComment.CommentContent = comment.CommentContent;
                postComment.commentDate = GetPostCreateDate(comment.CreatedAt);
                postComment.CreatorPhoto = comment.User.ProfilePhotos.Where(p => p.IsCurrent == true).Select(p => p.Url).FirstOrDefault();
                postComment.FullNameCreator = $"{comment.User.FirstName}{comment.User.LastName}";
                postComments.Add(postComment);
            }
            return postComments;
        }

        public static List<postLike> GetPostLikes(List<Like> likes)
        {
            List<postLike> postLikes = new List<postLike>();
            foreach(var like in likes )
            {
                postLike postLike = new postLike();
                postLike.FullNameCreatorLike = $"{like.User.FirstName} {like.User.LastName}";
                postLike.PhotoCreatorLike = like.User.ProfilePhotos.Where(photo => photo.IsCurrent == true).Select(photo => photo.Url)
                    .FirstOrDefault();

                postLikes.Add(postLike);
            }

       

            return postLikes;
        }


        public static string GetPostCreateDate(DateTime PostDate)
        {
         
            DateTime requestTime = DateTime.Now;
            var result = requestTime - PostDate;

            if (result.TotalDays > 5) return string.Format("{0}", result.ToString("dd/MM/yyyy"));
            if (result.TotalDays != 0) return string.Format("{0} Days ago", result.TotalDays);
            if (result.TotalHours != 0) return string.Format("{0} Hours ago", result.TotalHours);
            if (result.TotalMinutes != 0) return string.Format("{0} Minutes ago", result.TotalMinutes);
            if(result.TotalSeconds!=0) return string.Format("{0} Seconds ago", result.TotalSeconds);
            return "";
        }


    }
}
