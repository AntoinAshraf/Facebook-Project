using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Models.ViewModels
{
    public class ProfilePageDto
    {
        //Info
       public  userInfo UserInfo { get; set; }
        //Photo
       public string UserProfilePhoto { get; set; }
        //numOfFriends
       public int NumberOfFriends { get; set; }
        //Ability Edit
       public bool CanEditProfileWritePost { get; set; }
        //userposts
       public List<userPost> Posts { get; set; } = new List<userPost>();
        //AllUserProfiePhotos
       public List<string> AllProfilePhotos { get; set; } = new List<string>();
        //friendRequest
       public List<FriendRequest> FriendRequests { get; set; } = new List<FriendRequest>();

    }

    public class userInfo
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string GenderName { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class userPost
    {
        public int PostId { get; set; }
        public string PostContent { get; set; }
        public string PostPhoto { get; set; }
        public string PostDate { get; set; }
        public bool CanChange { get; set; }
        public bool IsLike { get; set; }
        public int numOfLikes { get; set; }
        public int numOfComments { get; set; }
        //comments
        public List<postComment> Comments { get; set; } = new List<postComment>();
        //likes
        public List<postLike> Likes { get; set; } = new List<postLike>();
    }

    public class postComment
    {
        public string CommentContent { get; set; }
        public string commentDate { get; set; }
        public string FullNameCreator { get; set; }
        public string CreatorPhoto { get; set; }
        public bool canRemove { get; set; }
       
    }

    public class postLike
    {
        public string FullNameCreatorLike { get; set; }
        public string PhotoCreatorLike { get; set; }
       // public string DateCreatedLike { get; set; }
        
    }

    public class FriendRequest
    {
        public string FullName { get; set; }
        public string Photo { get; set; }
    }
}
