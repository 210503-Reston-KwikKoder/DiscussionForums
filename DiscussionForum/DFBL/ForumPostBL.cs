using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFModels;
using DFDL;

namespace DFBL
{
   
    class ForumPostBL :IForumPost
    {
        private readonly IRepo _repo;
        public ForumPostBL(DFDBContext context)
        {
            _repo = new Repo(context);
        }
        public async Task<List<Posts>> GetAllPosts()
        {
            return await _repo.GetAllPostsAsync();
        }

        public async Task<List<Posts>> GetPostForForumWithID(int id)
        {
            return await _repo.GetPostForForumWithID(id);
        }

        public async Task<Posts> AddPost(Posts post)
        {
            return await _repo.AddPostsAsync(post);
        }

        public async Task<Posts> RemovePost(Posts post)
        {

            return await _repo.DeletePostsAsync(post);
            //throw new Exception("Looks like this post doesn't exist. We may have already processed this request.");
        }

        public async Task<Posts> UpdatePost(Posts post)
        {
            return await _repo.UpdatePostsAsync(post);
        }
    }
}
