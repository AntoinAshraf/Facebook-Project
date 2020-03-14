using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceBook.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
            ProfilePhotos = new HashSet<ProfilePhoto>();
            UserRelationsDesider = new HashSet<UserRelation>();
            UserRelationsInitiator = new HashSet<UserRelation>();
            UsersPosts = new HashSet<UsersPost>();
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? RecoveryCode { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsCreatedByAdmin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int GenderId { get; set; }
        public string PhoneNumber { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<ProfilePhoto> ProfilePhotos { get; set; }
        public virtual ICollection<UserRelation> UserRelationsDesider { get; set; }
        public virtual ICollection<UserRelation> UserRelationsInitiator { get; set; }
        public virtual ICollection<UsersPost> UsersPosts { get; set; }
    }
}
