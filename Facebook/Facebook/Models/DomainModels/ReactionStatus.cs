using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class ReactionStatus
    {
        public ReactionStatus()
        {
            Likes = new HashSet<Like>();
        }
        public int Id { get; set; }
        public string ReactionName { get; set; }
        public bool IsDeleted { get; set; }
        public string IconUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
    }
}
