using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscussionForumREST.DTO
{
    public class AddForumPostInput
    {
        public string Topic { get; set; }
        public string Description { get; set; }
        public int ForumID { get; set; }
    }
}
