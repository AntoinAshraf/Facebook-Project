using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
            PostPhotos = new HashSet<PostPhoto>();
            UsersPosts = new HashSet<UsersPost>();
        }
        public int Id { get; set; }
        public string PostContent { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PostPhoto> PostPhotos { get; set; }
        public virtual ICollection<UsersPost> UsersPosts { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
    }
}
