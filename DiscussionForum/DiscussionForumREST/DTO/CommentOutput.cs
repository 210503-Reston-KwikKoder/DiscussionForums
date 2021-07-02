using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscussionForumREST.DTO
{
    public class CommentOutput
    {
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public string AuthID { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public string Message { get; set; }
        public bool isLoggedUser { get; set; }
    }
}
