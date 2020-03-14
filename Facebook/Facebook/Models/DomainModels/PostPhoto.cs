using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class PostPhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int PostId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Post Post { get; set; }
    }
}
