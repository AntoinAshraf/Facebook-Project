using Facebook.Contracts;
using Facebook.Models.ViewModels;
using FaceBook.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Utilities
{
    public class UserData : IUserData
    {
        public void SetActions(HttpContext httpContext, List<Actions> actions)
        {
            httpContext.Session.SetComplexData("Actions", actions);
        }
        public List<Actions> GetActions(HttpContext httpContext)
        {
            if (IsAuthorize(httpContext) == true)
            {
                return httpContext.Session.GetComplexData<List<Actions>>("Actions");
            }
            return null;
        }

        public void SetUser(HttpContext httpContext, User user)
        {
            httpContext.Session.SetComplexData("User", user);
        }

        public User GetUser(HttpContext httpContext)
        {
            if (IsAuthorize(httpContext) == true)
            {
                return httpContext.Session.GetComplexData<User>("User");
            }
            return null;
        }

        public LayoutUserDto GetLayoutData(HttpContext httpContext)
        {
            User user = GetUser(httpContext);
            string profilePic = user.ProfilePhotos.FirstOrDefault(x => x.UserId == user.Id) == null ? (user.GenderId == 1 ? "default.jpg" : "default_female.png") : user.ProfilePhotos.FirstOrDefault(x => x.UserId == user.Id).Url;
            return new LayoutUserDto() { userId = user.Id, FullName = $"{user.FirstName}", ProfilePic = profilePic, actions = GetActions(httpContext) };
        }

        public bool IsAuthorize(HttpContext httpContext)
        {
            if (httpContext.Session.GetComplexData<User>("User") == null || httpContext.Session.GetComplexData<User>("User").IsActive == false)
            {
                return false;
            }
            return true;
        }

        public void clearData(HttpContext httpContext)
        {
            httpContext.Session.Clear();
        }

    }
}
