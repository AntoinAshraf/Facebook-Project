using Facebook.Models.ViewModels;
using FaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Mappers
{
    public class UserMapper
    {
        public static IEnumerable<UserRegisterDTO> Map(IEnumerable<User> from)
        {
            var to = new List<UserRegisterDTO>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from)
                {
                    to.Add(Map(item));
                }
            }
            return to;
        }

        public static UserRegisterDTO Map(User from)
        {
            if (from == null) return null;

            var to = new UserRegisterDTO
            {
                FirstName = from.FirstName,
                LastName = from.LastName,
                Password = from.Password,
                Email = from.Email,
                BirthDate = from.BirthDate,
                PhoneNumber = from.PhoneNumber,
                GenderId = from.GenderId
            };

            return to;
        }


        /////////////////////////////////////////////////////////////////////////////

        public static IEnumerable<User> Map(IEnumerable<UserRegisterDTO> from)
        {
            var to = new List<User>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from)
                {
                    to.Add(Map(item));
                }
            }
            return to;
        }

        public static User Map(UserRegisterDTO from)
        {
            if (from == null) return null;

            var to = new User
            {
                FirstName = from.FirstName,
                LastName = from.LastName,
                Password = from.Password,
                Email = from.Email,
                BirthDate = from.BirthDate,
                PhoneNumber = from.PhoneNumber,
                GenderId = from.GenderId
            };

            return to;
        }
    }
}
