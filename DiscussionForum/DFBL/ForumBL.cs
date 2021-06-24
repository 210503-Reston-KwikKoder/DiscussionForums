using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBL
{
    class ForumBL
    {
        public async Task<List<Forum>> GetAllForums()
        {
            return await _repo.GetAllForumsAsync();
        }

        public async Task<Forum> GetForum(int forumID)
        {
            return await _repo.GetForumByIdAsync(forumID);
        }

        public async Task<Forum> AddForum(Forum forum)
        {
            return await _repo.AddForumAsync(forum);
        }

        public async Task<Forum> RemoveForum(Forum forum)
        {
            if (await _repo.GetForumAsync(forum) != null)
                return await _repo.DeleteForumAsync(forum);
            throw new Exception("This Forum does not exist. We may have already processed this request.");
        }

        public async Task<Forum> UpdateForum(Forum forum)
        {
            return await _repo.UpdateForumAsync(forum);
        }
    }
}
