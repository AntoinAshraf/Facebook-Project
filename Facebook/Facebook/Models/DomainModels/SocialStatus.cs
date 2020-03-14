using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class SocialStatus
    {
        public SocialStatus()
        {
            UserRelations = new HashSet<UserRelation>();
        }

        public int Id { get; set; }
        public string Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ICollection<UserRelation> UserRelations { get; set; }
    }
}
