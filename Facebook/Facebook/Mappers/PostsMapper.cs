using Facebook.Contracts;
using Facebook.Models.ViewModels;
using FaceBook.Models;
using FacebookDbContext;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace Facebook.Mappers
{
    public class PostsMapper
    {
        private  FacebookDataContext facebookDataContext;

        public PostsMapper(FacebookDataContext _facebookDataContext)
        {
            facebookDataContext = _facebookDataContext;
        }

        public  IEnumerable<PostsDTO> Map(IEnumerable<UsersPost> from, User user)
        {
            var to = new List<PostsDTO>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from)
                {
                    to.Add(Map(item, user));
                }
            }
            return to;
        }

       
        public  PostsDTO Map(UsersPost from, User user)
        {
            if (from == null) 
                return null;
            var to = new PostsDTO();
            
            to.Id = from.Id;
            to.FirstName = user.FirstName;
            to.LastName = user.LastName;
            to.PostContent = from.Post.PostContent;
            to.CreatedAt = from.CreatedAt;
            to.UrlPost = from.Post.PostPhotos.Where(p => p.PostId == from.PostId).Select(p => p.Url).ToList();
            //to.UrlUser = from.User.ProfilePhotos.Where(u => u.UserId == user.Id).Select(u => u.Url).FirstOrDefault();
            to.UrlUser = facebookDataContext.ProfilePhotos.Where(u => u.UserId == user.Id && u.IsCurrent == true).Select(u => u.Url).FirstOrDefault();
            to.Likes = facebookDataContext.Likes.Where(p => p.PostId == from.PostId).ToList();
            to.Comments = from.Post.Comments.ToList();
           
            return to;
        }

    }
}
