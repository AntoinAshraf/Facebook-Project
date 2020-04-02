using FaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Models.ViewModels
{
    public class LayoutUserDto
    {
        public int userId { get; set; }
        public string FullName { get; set; }
        public string ProfilePic { get; set; }
        public List<Actions> actions { get; set; }
    }
}
