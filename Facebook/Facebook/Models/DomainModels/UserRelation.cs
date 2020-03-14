using System;
using System.Collections.Generic;

namespace FaceBook.Models
{
    public partial class UserRelation
    {
        public int Id { get; set; }
        public int SocialStatusId { get; set; }
        public int InitiatorId { get; set; }
        public int DesiderId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual User Desider { get; set; }
        public virtual User Initiator { get; set; }
        public virtual SocialStatus SocialStatus { get; set; }
    }
}
