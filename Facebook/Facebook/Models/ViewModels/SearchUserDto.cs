using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Models.ViewModels
{
    public class SearchUserDto
    {
        public int Id { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public int SocialStatus { get; set; }
        public bool? Initiator { get; set; }
        public string ProfilePic { get; set; }
    }
}
