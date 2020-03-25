using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Models.ViewModels
{
    public class CommentPostDto
    {
        public int PostId { get; set; }
        public string CommentContent { get; set; }
    }
}
