using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFBL
{
    class CommentBL
    {
        public async Task<List<Comments>> GetAllComments()
        {
            return await _repo.GetAllCommentsAsync();
        }

        public async Task<List<Comments>> GetComment(int commentID)
        {
            return await _repo.GetCommentsByIdAsync(commentID);
        }

        public async Task<Comments> AddComment(Comments comment)
        {
            return await _repo.AddCommentsAsync(comment);
        }

        public async Task<Comments> RemoveComments(Comments comments)
        {

            return await _repo.DeleteCommentsAsync(comments);

        }

        public async Task<Comments> UpdateComment(Comments comments)
        {
            return await _repo.UpdateCommentsAsync(comments);
        }
    }
}
