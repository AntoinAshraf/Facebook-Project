using FaceBook.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Facebook.Contracts
{
    public interface IUserData
    {
        List<Actions> GetActions(HttpContext httpContext);
        bool IsAuthorize(HttpContext httpContext);
        User GetUser(HttpContext httpContext);
        void SetUser(HttpContext httpContext, User user);
        void SetActions(HttpContext httpContext, List<Actions> actions);
    }
}