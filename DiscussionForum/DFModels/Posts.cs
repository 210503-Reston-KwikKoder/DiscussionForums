using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFModels
{
    public class Posts
    {
        public Posts(int postID, string topic, string userCreator, int forumID)
        {
            this.PostID = postID;
            this.Topic = topic;
            this.UserCreator = userCreator;
            this.ForumID = forumID;
        }
        public Posts()
        {

        }

        public int PostID { get; set; }
        public string AuthID { get; set; }
        public string Topic { get; set; }
        public string UserCreator { get; set; }
        public int ForumID { get; set; }

        public override string ToString()
        {
            return "PostID: " + this.PostID + " Topic: " + this.Topic + "  ForumID " + this.ForumID + " User: " + this.UserCreator;
        }
    }
}
