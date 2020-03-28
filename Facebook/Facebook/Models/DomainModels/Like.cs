using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class Like
    {
        public int Id { get; set; }
        public int ReactionStatusId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ReactionStatus ReactionStatus { get; set; }
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}
