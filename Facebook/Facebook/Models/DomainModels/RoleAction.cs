using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class RoleAction
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int ActionId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Actions Action { get; set; }
        public virtual Role Role { get; set; }
    }
}
