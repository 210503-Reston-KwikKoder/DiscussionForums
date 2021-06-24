using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNHModels
{
    public class Forum
    {
        public Forum(int forumID, string topic, string description)
        {
            this.ForumID = forumID;
            this.Topic = topic;
            this.Description = description;
        }

        public Forum()
        { }

        public int ForumID { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        
        

        
    }
}
