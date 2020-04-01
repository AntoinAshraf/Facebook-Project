using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Models.ViewModels
{
    public class HomePageDto
    {
        public string FullName { get; set; }
        public string ProfilePicUrl { get; set; }
        public int NumberOfFriends { get; set; }

        public List<HomeUserDto> HomeUserDtos { get; set; }
        public List<HomePostDto> HomePostDto { get; set; }
    }

    public class HomeUserDto
    {
        public HomeUserDto(string _FullName, string _ProfilePicUrl) 
            => (FullName, ProfilePicUrl) = (_FullName, _ProfilePicUrl);
        public string FullName { get; set; }
        public string ProfilePicUrl { get; set; }
    }

    public class HomeUserTempDto 
    {
        public string FullName { get; set; }
        public string ProfilePicUrl { get; set; }
        public List<HomePostTempDto> HomePostDto { get; set; }
    }

    public class HomePostDto
    {
        public HomePostDto(string _FullName, string _ProfilePic, string _PostDate, string _PostContent, List<HomeCommentDto> _HomeCommentDto, List<HomeLikeDto> _HomeLikeDto, string _PostPicUrl, int _PostId, bool _CanEditDelete, bool _IsLike) 
            => (FullName, ProfilePic, PostDate, PostContent, HomeCommentDto, HomeLikeDto, PostPicUrl, PostId, CanEditDelete, IsLike) = (_FullName, _ProfilePic, _PostDate, _PostContent, _HomeCommentDto, _HomeLikeDto, _PostPicUrl, _PostId, _CanEditDelete, _IsLike);

        public string FullName { get; set; }
        public string ProfilePic { get; set; }
        public string PostDate { get; set; }
        public string PostContent { get; set; }
        public string PostPicUrl { get; set; }
        public int PostId { get; set; }
        public bool CanEditDelete { get; set; }
        public bool IsLike { get; set; }
        public List<HomeCommentDto> HomeCommentDto { get; set; }
        public List<HomeLikeDto> HomeLikeDto { get; set; }
    }

    public class HomePostTempDto
    {
        public string FullName { get; set; }
        public string ProfilePic { get; set; }
        public string PostDate { get; set; }
        public string PostContent { get; set; }
        public DateTime CreatedAt { get; set; }
        public string PostPicUrl { get; set; }
        public int PostId { get; set; }
        public bool CanEditDelete { get; set; }
        public bool IsLike { get; set; }
        public List<HomeCommentDto> HomeCommentDto { get; set; }
        public List<HomeLikeDto> HomeLikeDto { get; set; }
    }

    public class HomeCommentDto
    {
        public int CommentId { get; set; }
        public string FullName { get; set; }
        public string ProfilePicUrl { get; set; }
        public string CommentContent { get; set; }
        public string CommentDate { get; set; }
        public bool CanDelete { get; set; }
    }

    public class HomeLikeDto
    {
        public int LikeId { get; set; }
        public string FullName { get; set; }
        public string ProfilePicUrl { get; set; }
        public string LikeDate { get; set; }
    }

    public class CreatedPostDto
    {
        public string FullName { get; set; }
        public string ProfilePic { get; set; }
        public string PostDate { get; set; }
        public string PostContent { get; set; }
        public string PostPicUrl { get; set; }
        public int PostId { get; set; }
    }
}
