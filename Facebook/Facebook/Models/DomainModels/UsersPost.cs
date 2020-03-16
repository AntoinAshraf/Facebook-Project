using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class UsersPost
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsCreator { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
