﻿using Facebook.Models.ViewModels;
using FaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Mappers
{
    public class SearchUserMapper
    {
        public static IEnumerable<SearchUserDto> Map(IEnumerable<User> from, int currentUerId)
        {
            var to = new List<SearchUserDto>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from)
                {
                    to.Add(Map(item, currentUerId));
                }
            }
            return to;
        }

        public static SearchUserDto Map(User from, int currentUerId)
        {
            if (from == null) return null;

            var to = new SearchUserDto
            {
                Fullname = $"{from.FirstName} {from.LastName}",
                Email = from.Email,
                Id = from.Id,
                ProfilePic = from.ProfilePhotos.Where(x => x.IsCurrent == true).Count() == 0 ? "default.jpg" : from.ProfilePhotos.Where(x => x.IsCurrent == true).FirstOrDefault().Url
            };


            if(from.UserRelationsDesider.Any(x => x.InitiatorId == currentUerId))
            {
                to.SocialStatus = from.UserRelationsDesider.FirstOrDefault(x => x.InitiatorId == currentUerId).SocialStatusId;
                to.Initiator = false;
            }
            else if (from.UserRelationsInitiator.Any(x => x.DesiderId == currentUerId))
            {
                to.SocialStatus = from.UserRelationsInitiator.FirstOrDefault(x => x.DesiderId == currentUerId).SocialStatusId;
                to.Initiator = true;
            }
            else
            {
                to.SocialStatus = 0; //no relation
                to.Initiator = null;
            }
            return to;
        }
    }
}
