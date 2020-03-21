using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Models.ViewModels
{
   
    public class UserPostDTO
    {
        public int Id { get; set; }
        public string PostContent { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped]
        public IFormFile image { get; set; }
    }
}
