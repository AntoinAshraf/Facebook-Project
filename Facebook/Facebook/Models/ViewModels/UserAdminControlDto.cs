using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Models.ViewModels
{
    public class UserAdminControlDto
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string Fullname { get; set; }
        public string Bio { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
