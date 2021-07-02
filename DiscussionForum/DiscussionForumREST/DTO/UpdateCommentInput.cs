using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscussionForumREST.DTO
{
    public class UpdateCommentInput
    {
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public DateTime Created { get; set; }
        public string Message { get; set; }
    }
}
