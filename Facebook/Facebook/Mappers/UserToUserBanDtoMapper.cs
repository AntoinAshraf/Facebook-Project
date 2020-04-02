using Facebook.Models.ViewModels;
using FaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Mappers
{
    public class UserToUserBanDtoMapper
    {
        public static List<UserAdminControlDto> Map(IEnumerable<User> from)
        {
            var to = new List<UserAdminControlDto>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from)
                {
                    to.Add(Map(item));
                }
            }
            return to;
        }

        public static UserAdminControlDto Map(User from)
        {
            if (from == null) return null;

            var to = new UserAdminControlDto
            {
                Id = from.Id,
                RoleId = from.RoleId,
                Bio = from.Bio,
                Fullname = $"{from.FirstName} {from.LastName}",
                IsActive = from.IsActive,
                ProfilePhotoUrl = from.ProfilePhotos.Where(x=>x.IsCurrent == true).Count() == 0 ? (from.GenderId == 1 ? "default.jpg" : "default_female.png") : from.ProfilePhotos.Where(x => x.IsCurrent == true).FirstOrDefault().Url
            };

            return to;
        }

    }
}
