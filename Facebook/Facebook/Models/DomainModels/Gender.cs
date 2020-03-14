using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Users = new HashSet<User>();
        }
        public int Id { get; set; }
        public string GenderName { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
