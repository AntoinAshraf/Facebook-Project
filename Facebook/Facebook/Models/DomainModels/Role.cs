using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class Role
    {
        public Role()
        {
            RoleActions = new HashSet<RoleAction>();
            Users = new HashSet<User>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<RoleAction> RoleActions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
