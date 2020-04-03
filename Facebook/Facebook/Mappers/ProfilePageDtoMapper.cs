using Facebook.Models.ViewModels;
using Facebook.Utilities.Enums;
using FaceBook.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Mappers
{
    public static class ProfilePageDtoMapper
    {
        public static ProfilePageDto Mapper(User From,int UserId /*logged-in user*/)
        {
            ProfilePageDto to = new ProfilePageDto();

            to.UserInfo = mapperInfo(From);
            to.UserProfilePhoto = mapperProfilePhoto(From);
            to.AllProfilePhotos = mapperAllPhoto(From);
            to.CanEditProfileWritePost = mapperCanChange(From, UserId);
            to.FriendRequests = mapperFriendRequest(From, UserId);
            to.NumberOfFriends = mapperNumOfFriends(From);
            to.Posts = mapperPosts(From, UserId);
            to.btnRelationStatus = btnRelation(From, UserId);
            to.CurrentUserId = UserId;

            return to;
        }


        public static FriendRelationStatus btnRelation(User From,int UserId)
        {

            //return From.UserRelationsDesider.Any(u => u.InitiatorId == UserId && u.IsDeleted == false)
            //     || (From.UserRelationsInitiator.Any(u => u.DesiderId == UserId && u.IsDeleted == false));

            var isFriends = From.UserRelationsDesider.Any(u => u.InitiatorId == UserId && u.IsDeleted == false && u.SocialStatusId == 1 /*frined*/)
                 || (From.UserRelationsInitiator.Any(u => u.DesiderId == UserId && u.IsDeleted == false && u.SocialStatusId == 1));
            if (isFriends)
                return FriendRelationStatus.Remove;

            var isRequest = From.UserRelationsDesider.Any(u => u.InitiatorId == UserId && u.IsDeleted == false && u.SocialStatusId == 2 /*frined*/)
                 || (From.UserRelationsInitiator.Any(u => u.DesiderId == UserId && u.IsDeleted == false && u.SocialStatusId == 2));
            if (isRequest)
                return FriendRelationStatus.Pending;

            return FriendRelationStatus.Add; // The relation between the initiator and the decider not exists
            
        }
        public static userInfo mapperInfo(User From)
        {
            userInfo info = new userInfo();
            info.FullName = $"{From.FirstName} {From.LastName}";
            info.Bio = From.Bio;
            info.BirthDate = From.BirthDate;
            info.GenderName = From.Gender.GenderName;
            info.PhoneNumber = From.PhoneNumber;
            info.id = From.Id; // userId

            return info;
        }

        public static string mapperProfilePhoto(User From)
        {
            string Url = From.ProfilePhotos.Where(user =>user.IsCurrent==true).Select(pho=>pho.Url).FirstOrDefault();
            return Url;
        }

        public static List<string> mapperAllPhoto(User From)
        {
            List<string> AllPhoto = From.ProfilePhotos.Where(photo => photo.IsDeleted == false).Select(photo => photo.Url).ToList();
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
                   
                    if (item.IsDeleted == false && item.Initiator.IsDeleted == false 
                        && item.SocialStatusId == (int)SocialStatuses.Request)
                    {
                        FriendRequest friendRequest = new FriendRequest();
                        friendRequest.FullName = $"{item.Initiator.FirstName} {item.Initiator.LastName}";
                        friendRequest.Photo = item.Initiator.ProfilePhotos.FirstOrDefault(photo => photo.IsCurrent == true) != null ? item.Initiator.ProfilePhotos.FirstOrDefault(photo => photo.IsCurrent == true).Url : (item.Initiator.GenderId == 1 ? "default.jpg" : "default_female.png");
                        friendRequest.initiatorId = item.InitiatorId;
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

            foreach(var item in From.UsersPosts.OrderByDescending(u=>u.CreatedAt))
            {
                if (item.Post.IsDeleted == false)
                {
                    userPost userPost = new userPost();

                    // mapping
                    userPost.PostContent = item.Post.PostContent;
                    userPost.PostDate = GetPostCreateDate(item.CreatedAt);
                    userPost.CanChange = (item.UserId == id);
                    userPost.IsLike = item.Post.Likes.Any(u => u.UserId == id&&u.IsDeleted==false);
                    userPost.numOfLikes = item.Post.Likes.Where(u=>u.IsDeleted==false).Count();
                    userPost.numOfComments = item.Post.Comments.Where(c=>c.IsDeleted==false).Count();
                    userPost.Likes = GetPostLikes(item.Post.Likes.ToList());
                    userPost.Comments = GetPostComments(item.Post.Comments.Where(c=>c.IsDeleted == false).ToList(), id);
                    userPost.PostPhoto = item.Post.PostPhotos.Select(p => p.Url).FirstOrDefault();
                    userPost.PostId = item.PostId;

                    userPosts.Add(userPost);
                }
            }
            return userPosts;

        }


        public static List<postComment> GetPostComments(List<Comment> Comments, int? loggedUserId)
        {
            List<postComment> postComments = new List<postComment>();
            foreach (var comment in Comments)
            {
                //if(comment.IsDeleted == false)
                //{
                postComment postComment = new postComment();

                postComment.CommentId = comment.Id;
                postComment.CommentCreatorId = comment.UserId;
                postComment.CommentContent = comment.CommentContent;
                postComment.commentDate = GetPostCreateDate(comment.CreatedAt);
                postComment.CreatorPhoto = comment.User.ProfilePhotos.Where(p => p.IsCurrent == true).Select(p => p.Url).FirstOrDefault();
                postComment.FullNameCreator = $"{comment.User.FirstName} {comment.User.LastName}";
                postComment.canRemove = (comment.UserId == loggedUserId);
                postComments.Add(postComment);

                //return postComments;
                //}
            }
            return postComments;
            //return null;
        }

        public static List<postLike> GetPostLikes(List<Like> likes)
        {
            List<postLike> postLikes = new List<postLike>();
            foreach(var like in likes )
            {
                if (like.IsDeleted == false)
                {
                    postLike postLike = new postLike();

                    postLike.LikeId = like.Id;
                    postLike.LikeCreatorId = like.UserId;
                    postLike.FullNameCreatorLike = $"{like.User.FirstName} {like.User.LastName}";
                    postLike.PhotoCreatorLike = like.User.ProfilePhotos.Where(photo => photo.IsCurrent == true).Select(photo => photo.Url)
                        .FirstOrDefault();
                    postLike.DateCreatedLike = $"Liked {GetPostCreateDate(like.CreatedAt)}";
                    postLikes.Add(postLike);
                }
            }
              return postLikes.Count > 0 ? postLikes : null;
        }

        public static string GetPostCreateDate(DateTime PostDate)
        {
         
            DateTime requestTime = DateTime.Now;
            var result = requestTime - PostDate;

            if (result.TotalDays > 5) return string.Format("{0} at {1}", PostDate.ToString("MMMM dd"), PostDate.ToString("hh:mm tt"));/*PostDate.ToString("dd/MM/yyyy"), PostDate.Month, PostDate.Day, result.Hours, result.Minutes);*/
            if(result.TotalHours > 24 && result.TotalHours < 48) return string.Format("Yesterday at {0}", PostDate.ToString("hh:mm tt"));
            if (result.TotalDays > 2) return string.Format("{0} Days ago", result.Days);
            if (result.TotalHours >= 1) return string.Format("{0} Hours ago", result.Hours);
            if (result.TotalMinutes >= 1) return string.Format("{0} Minutes ago", result.Minutes);
            if(result.TotalSeconds!=0) return string.Format("{0} Seconds ago", result.Seconds);
            return "";
        }


    }
}
