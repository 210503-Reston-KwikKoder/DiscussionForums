using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscussionForumREST.DTO
{
    public class ForumPostOutput
    {
        public int PostID { get; set; }
        public string Topic { get; set; }
        public string UserName { get; set; }
        public string ImgURL { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public int ForumID { get; set; }
        public bool isUser { get; set; }
}
}
