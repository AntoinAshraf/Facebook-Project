using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Models.ViewModels
{
    public class UserPostDTO
    {
        public int Id { get; set; }
        public string PostContent { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
