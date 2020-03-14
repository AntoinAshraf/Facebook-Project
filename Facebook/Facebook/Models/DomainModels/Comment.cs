using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string CommentContent { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
