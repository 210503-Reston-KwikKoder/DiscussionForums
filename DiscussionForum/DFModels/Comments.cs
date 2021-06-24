using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNHModels
{
    public class Comments
    {
        public Comments(int commentID, int postID, string userName, DateTime created, string message)
        {
            this.CommentID = commentID;
            this.PostID = postID;
            this.UserName = userName;
            this.Created = created;
            this.Message = message;
        }
        public Comments()
        { }
        public int CommentID { get; set; }
        public int PostID { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
        public string Message { get; set; }

    }
    
}
