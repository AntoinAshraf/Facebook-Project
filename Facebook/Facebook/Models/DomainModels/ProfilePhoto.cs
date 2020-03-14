using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class ProfilePhoto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsCurrent { get; set; }
        public int UserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User User { get; set; }
    }
}
