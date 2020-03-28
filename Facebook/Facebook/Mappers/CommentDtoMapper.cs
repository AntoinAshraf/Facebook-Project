using Facebook.Models.ViewModels;
using FaceBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Facebook.Mappers
{
    public class CommentDtoMapper
    {
        public static IEnumerable<Comment> Map(IEnumerable<CommentPostDto> from, int userId)
        {
            var to = new List<Comment>();
            if (from != null && from.Count() > 0)
            {
                foreach (var item in from)
                {
                    to.Add(Map(item, userId));
                }
            }
            return to;
        }

        public static Comment Map(CommentPostDto from, int userId)
        {
            if (from == null) return null;

            var to = new Comment
            {
                CommentContent = from.CommentContent,
                IsDeleted = false,
                PostId = from.PostId,
                CreatedAt = DateTime.Now,
                UserId = userId
            };

            return to;
        }
    }
}
