using FaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Models.ViewModels
{
    public class PostsDTO
    {
        public int Id { get; set; }
        public string PostContent { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get { return $"{ FirstName}  {LastName }";  } } 
        public string Url { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual List<Comment> Comments { get; set; } = new List<Comment>();
        public virtual List<Like> Likes { get; set; } = new List<Like>();
    }
}
