using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFModels;
using DFDL;

namespace DFBL
{
    public class CommentBL :IComment
    {
        private readonly IRepo _repo;
        public CommentBL(IRepo repo)
        {
            _repo = repo;
        }
        public async Task<List<Comments>> GetAllComments()
        {
           return  await _repo.GetAllCommentsAsync();
        }

        public async Task<List<Comments>> GetCommentsByPostID(int PostID)
        {
            return await _repo.GetCommentsByIdAsync(PostID);
        }
        public async Task<Comments> GetCommentByID(int CommentID)
        {
            return await _repo.GetCommentByCommentID(CommentID);
        }
        public async Task<Comments> AddComment(Comments comment)
        {
            return await _repo.AddCommentsAsync(comment);
        }

        public async Task<int> RemoveComments(Comments comment)
        {

            return await _repo.DeleteCommentsAsync(comment);

        }

        public async Task<Comments> UpdateComment(Comments comments)
        {
            return await _repo.UpdateCommentsAsync(comments);
        }
    }
}
